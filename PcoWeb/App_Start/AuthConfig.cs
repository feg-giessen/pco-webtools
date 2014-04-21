using System;
using System.Configuration;
using Microsoft.Web.WebPages.OAuth;

namespace PcoWeb
{
    public static class AuthConfig
    {
        public static string ConsumerKey
        {
            get { return ConfigurationManager.AppSettings["PcoOauthKey"]; }
        }

        public static string ConsumerSecret
        {
            get { return ConfigurationManager.AppSettings["PcoOauthSecret"]; }
        }

        public static void RegisterAuth()
        {
            string key = ConsumerKey;
            string secret = ConsumerSecret;

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(secret))
                throw new InvalidOperationException("PCO Oauth access not configured.");

            OAuthWebSecurity.RegisterClient(
                new PcoOauthClient(key, secret),
                "Planning Center Online", 
                null);
        }
    }
}
