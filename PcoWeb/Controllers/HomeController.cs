using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using Microsoft.Web.WebPages.OAuth;
using Newtonsoft.Json;
using PcoBase;
using PcoWeb.Filters;
using PcoWeb.Models;
using WebMatrix.WebData;
using IsolationLevel = System.Data.IsolationLevel;

namespace PcoWeb.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        public static Organization Organization 
        {
            get 
            { 
                var org = System.Web.HttpContext.Current.Session["organization"] as Organization; 

                if (org == null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var service = new Service(AuthConfig.ConsumerKey, AuthConfig.ConsumerSecret);
                    org = Organization = service.GetOrganisation();
                }

                return org;
            }

            set 
            { 
                System.Web.HttpContext.Current.Session["organization"] = value; 
            }
        }

        public static List<MinistryPositionsResult> MinistryPositions
        {
            get { return System.Web.HttpContext.Current.Session["MinistryPositions"] as List<MinistryPositionsResult>; }
            set { System.Web.HttpContext.Current.Session["MinistryPositions"] = value; }
        }

        public static NameValueCollection AppSettings
        {
            get { return ConfigurationManager.AppSettings; }
        }

        //
        // GET: /Home/

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var service = new Service(AuthConfig.ConsumerKey, AuthConfig.ConsumerSecret);
                Organization = service.GetOrganisation();

                //if (PcoWebClient.IsAvailable(Organization))
                //{
                //    using (var web = new PcoWebClient())
                //    {
                //        MinistryPositions = web.GetMinistryPositions("");
                //    }
                //}

                return this.RedirectToAction("Index", "Dienstplan");
            }

            return View();
        }

        [HttpPost]
        public ActionResult PostMinistryPositions(string jsdata)
        {
            MinistryPositions = JsonConvert.DeserializeObject<List<MinistryPositionsResult>>(jsdata);

            return Json(new { result = true });
        }

        //
        // GET: /Home/Login

        [AllowAnonymous]
        public ActionResult Login(string returlUrl)
        {
            var client = OAuthWebSecurity.RegisteredClientData.First();

            return new ExternalLoginResult(
                client.AuthenticationClient.ProviderName,
                Url.Action("ExternalLoginCallback", new { ReturnUrl = string.IsNullOrWhiteSpace(returlUrl) ? Url.Action("Index") : returlUrl }));
        }

        //
        // POST: /Account/LogOff

        public ActionResult Logout()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            Session["accesstoken"] = result.ExtraData["accesstoken"];

            using (var db = new UsersContext())
            {
                UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == result.UserName.ToLower());
                // Überprüfen, ob der Benutzer bereits vorhanden ist
                if (user != null)
                {
                    user.PcoAccessToken = result.ExtraData["accesstoken"];
                    user.PcoTokenSecret = new CookieOAuthTokenManager().GetTokenSecret(result.ExtraData["accesstoken"]);

                    db.SaveChanges();
                }
            }
            
            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // Wenn der aktuelle Benutzer angemeldet ist, das neue Konto hinzufügen
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // Der Benutzer ist neu. Nach dem gewünschten Mitgliedschaftsnamen fragen.
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return this.RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                // Neuen Benutzer in die Datenbank einfügen
                using (var db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Überprüfen, ob der Benutzer bereits vorhanden ist
                    if (user == null)
                    {
                        string accessToken = Session["accesstoken"].ToString();
                        // Name in die Profiltabelle einfügen
                        db.UserProfiles.Add(new UserProfile
                        {
                            UserName = model.UserName,
                            PcoAccessToken = accessToken,
                            PcoTokenSecret = new CookieOAuthTokenManager().GetTokenSecret(accessToken)
                        });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "Der Benutzername ist bereits vorhanden. Geben Sie einen anderen Benutzernamen an.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return this.RedirectToAction("Index");
        }

        #region Hilfsprogramme
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }
        #endregion
    }
}
