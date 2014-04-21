using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using Newtonsoft.Json;
using PcoBase;
using PcoWeb.Models;

namespace PcoWeb
{
    [Authorize]
    public class Service
    {
        private InMemoryOAuthTokenManager tokenManager;

        private const string OrganizationLink = "https://www.planningcenteronline.com/organization.json";
        private const string PlansLink = "https://www.planningcenteronline.com/service_types/{0}/plans.json";
        private const string PlanLink = "https://www.planningcenteronline.com/plans/{0}.json";
        private const string SongLink = "https://www.planningcenteronline.com/songs/{0}.json";
        private const string ArrangementLink = "https://www.planningcenteronline.com/arrangements/{0}.json";

        public Service(string appId, string appSecret)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                using (var db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == HttpContext.Current.User.Identity.Name.ToLower());
                    // Überprüfen, ob der Benutzer bereits vorhanden ist
                    if (user != null)
                    {
                        AccessToken = user.PcoAccessToken;
                        TokenSecret = user.PcoTokenSecret;
                    }
                }
            }

            this.tokenManager = new InMemoryOAuthTokenManager(appId, appSecret);
            tokenManager.ExpireRequestTokenAndStoreNewAccessToken(string.Empty, string.Empty, this.AccessToken, this.TokenSecret);
        }

        protected string AccessToken
        {
            get; private set;
        }

        protected string TokenSecret
        {
            get; private set;
        }

        public Organization GetOrganisation()
        {
            var c = new WebConsumer(PcoOauthClient.PcoServiceDescription, this.tokenManager);

            var organizationEndpoint = new MessageReceivingEndpoint(OrganizationLink, HttpDeliveryMethods.GetRequest);
            HttpWebRequest request = c.PrepareAuthorizedRequest(organizationEndpoint, this.AccessToken);
            
            string jsonResponse;

            using (WebResponse profileResponse = request.GetResponse())
            {
                using (Stream responseStream = profileResponse.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        jsonResponse = streamReader.ReadToEnd();
                    }
                }
            }

            return JsonConvert.DeserializeObject<Organization>(jsonResponse);
        }

        public IList<PlanIndex> GetPlans(int serviceType, bool all = false)
        {
            var c = new WebConsumer(PcoOauthClient.PcoServiceDescription, this.tokenManager);

            var plansEndpoint = new MessageReceivingEndpoint(string.Format(PlansLink, serviceType) + (all ? "?all=true" : string.Empty), HttpDeliveryMethods.GetRequest);
            HttpWebRequest request = c.PrepareAuthorizedRequest(plansEndpoint, this.AccessToken);

            string jsonResponse;

            using (WebResponse profileResponse = request.GetResponse())
            {
                using (Stream responseStream = profileResponse.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        jsonResponse = streamReader.ReadToEnd();
                    }
                }
            }

            return JsonConvert.DeserializeObject<List<PlanIndex>>(jsonResponse);
        }

        public Plan GetPlan(int planId)
        {
            var c = new WebConsumer(PcoOauthClient.PcoServiceDescription, this.tokenManager);

            var planEndpoint = new MessageReceivingEndpoint(string.Format(PlanLink, planId), HttpDeliveryMethods.GetRequest);
            HttpWebRequest request = c.PrepareAuthorizedRequest(planEndpoint, this.AccessToken);

            string jsonResponse;

            using (WebResponse profileResponse = request.GetResponse())
            {
                using (Stream responseStream = profileResponse.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        jsonResponse = streamReader.ReadToEnd();
                    }
                }
            }

            return JsonConvert.DeserializeObject<Plan>(jsonResponse);
        }

        public Song GetSong(int songId)
        {
            var c = new WebConsumer(PcoOauthClient.PcoServiceDescription, this.tokenManager);

            var planEndpoint = new MessageReceivingEndpoint(string.Format(SongLink, songId), HttpDeliveryMethods.GetRequest);
            HttpWebRequest request = c.PrepareAuthorizedRequest(planEndpoint, this.AccessToken);

            string jsonResponse;

            using (WebResponse profileResponse = request.GetResponse())
            {
                using (Stream responseStream = profileResponse.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        jsonResponse = streamReader.ReadToEnd();
                    }
                }
            }

            return JsonConvert.DeserializeObject<Song>(jsonResponse);
        }

        public Arrangement GetArrangement(int id)
        {
            var c = new WebConsumer(PcoOauthClient.PcoServiceDescription, this.tokenManager);

            var planEndpoint = new MessageReceivingEndpoint(string.Format(ArrangementLink, id), HttpDeliveryMethods.GetRequest);
            HttpWebRequest request = c.PrepareAuthorizedRequest(planEndpoint, this.AccessToken);

            string jsonResponse;

            using (WebResponse profileResponse = request.GetResponse())
            {
                using (Stream responseStream = profileResponse.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        jsonResponse = streamReader.ReadToEnd();
                    }
                }
            }

            return JsonConvert.DeserializeObject<Arrangement>(jsonResponse);
        }

        public List<MinistryPositionsResult> GetMinistryPositions(string query)
        {
            var c = new WebConsumer(PcoOauthClient.PcoServiceDescription, this.tokenManager);

            var endpoint = new MessageReceivingEndpoint(
                string.Format(
                    @"https://planningcenteronline.com/ministry_positions.json?name={0}&with_categories=1&in_ministry_path=&excluded_position_ids=&excluded_category_ids=",
                    query),
                HttpDeliveryMethods.GetRequest);
            HttpWebRequest request = c.PrepareAuthorizedRequest(endpoint, this.AccessToken);

            string jsonResponse;

            using (WebResponse profileResponse = request.GetResponse())
            {
                using (Stream responseStream = profileResponse.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        jsonResponse = streamReader.ReadToEnd();
                    }
                }
            }

            return JsonConvert.DeserializeObject<List<MinistryPositionsResult>>(jsonResponse);
        }
    }
}