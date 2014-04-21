using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;
using PcoBase;

namespace PcoWeb
{
    public class PcoOauthClient : OAuthClient
    {
        private const string AuthorizationServiceEndpoint = "https://www.planningcenteronline.com/oauth/authorize";
        private const string AccessTokenServiceEndpoint = "https://www.planningcenteronline.com/oauth/access_token";
        private const string RequestTokenServiceEndpoint = "https://www.planningcenteronline.com/oauth/request_token";
        private const string UserInfoServiceEndpoint = "https://www.planningcenteronline.com/me.json";

        private readonly string appId;
        private readonly string appSecret;

        /// Describes the OAuth service provider endpoints for LinkedIn.
        public static readonly ServiceProviderDescription PcoServiceDescription =
                new ServiceProviderDescription
                {
                    AccessTokenEndpoint =
                            new MessageReceivingEndpoint(AccessTokenServiceEndpoint,
                            HttpDeliveryMethods.PostRequest),
                    RequestTokenEndpoint =
                            new MessageReceivingEndpoint(RequestTokenServiceEndpoint,
                            HttpDeliveryMethods.PostRequest),
                    UserAuthorizationEndpoint =
                            new MessageReceivingEndpoint(AuthorizationServiceEndpoint,
                            HttpDeliveryMethods.PostRequest),
                    TamperProtectionElements =
                            new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
                    //ProtocolVersion = ProtocolVersion.V10a
                };

        public PcoOauthClient(string appId, string appSecret)
            : base("pco", PcoServiceDescription, new SimpleConsumerTokenManager(appId, appSecret, new CookieOAuthTokenManager()))
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }

        public PcoOauthClient(string appId, string appSecret, IOAuthTokenManager tokenManager)
            : base("pco", PcoServiceDescription, new SimpleConsumerTokenManager(appId, appSecret, tokenManager))
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }

        protected override AuthenticationResult VerifyAuthenticationCore(AuthorizedTokenResponse response)
        {
            string accessToken = response.AccessToken;

            var profileEndpoint = new MessageReceivingEndpoint(UserInfoServiceEndpoint, HttpDeliveryMethods.GetRequest);

            try
            {
                var imoatm = new InMemoryOAuthTokenManager(this.appId, this.appSecret);
                imoatm.ExpireRequestTokenAndStoreNewAccessToken(String.Empty, String.Empty, accessToken, (response as ITokenSecretContainingMessage).TokenSecret);
                var w = new WebConsumer(PcoServiceDescription, imoatm);

                HttpWebRequest request = w.PrepareAuthorizedRequest(profileEndpoint, accessToken);

                using (WebResponse profileResponse = request.GetResponse())
                {
                    string jsonResponse;
                    using (Stream responseStream = profileResponse.GetResponseStream())
                    {
                        using (var streamReader = new StreamReader(responseStream))
                        {
                            jsonResponse = streamReader.ReadToEnd();
                        }
                    }

                    Person person = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(jsonResponse);

                    if (person == null)
                        return new AuthenticationResult(false);

                    var extraData = new Dictionary<string, string>
                        {
                            { "email", person.contact_data.email_addresses.First().address },
                        };

                    return new AuthenticationResult(
                        isSuccessful: true, 
                        provider: this.ProviderName, 
                        providerUserId: person.id.ToString(), 
                        userName: person.name, 
                        extraData: extraData);
                }
            }
            catch (Exception exception)
            {
                return new AuthenticationResult(exception);
            }
        }
    }
}