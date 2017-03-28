using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PcoBase;
using PcoBase.Virtual;
using PcoWeb.Models;

namespace PcoWeb.Controllers
{
    [Authorize]
    public class VeranstaltungenController : Controller
    {
        //
        // GET: /Veranstaltungen/

        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return View(HomeController.Organization.ServiceTypes);

            var service = new Service(AuthConfig.ConsumerKey, AuthConfig.ConsumerSecret);

            var organization = service.GetOrganisation();
            var plans = service.GetPlans(id.Value);

            return this.View(
                "Plans", 
                new ServiceTypeModel
                {
                    ServiceType = organization.ServiceTypes.FirstOrDefault(x => x.Id == id),
                    Plans = plans,
                });
        }

        public ActionResult Auswertung()
        {
            var service = new Service(AuthConfig.ConsumerKey, AuthConfig.ConsumerSecret);
            
            var plansMorgen = service.GetPlans(308904, true);
            var plansAbend = service.GetPlans(200602, true);

            var startDate = new DateTime(2000, 1, 1);

            var morgenListe = new List<PlanAuswertungModel>();
            var abendListe = new List<PlanAuswertungModel>();

            Func<PlanIndex, bool> planSelector = p =>
            {
                var date = ViewHelpers.FormtatedDate(p.Dates);
                return date.HasValue && date.Value >= startDate && date.Value < DateTime.Now;
            };

            foreach (var op in plansMorgen.Where(planSelector))
            {
                Plan plan = service.GetPlan(op.Id);
                var model = new LivePlanModel(plan, i => null);

                morgenListe.Add(WerteAus(model));
            }

            foreach (var op in plansAbend.Where(planSelector))
            {
                Plan plan = service.GetPlan(op.Id);
                var model = new LivePlanModel(plan, i => null);

                abendListe.Add(WerteAus(model));
            }

            return this.View(new AuswertungModel
            {
                Morgens = morgenListe,
                Abends = abendListe
            });
        }

        private static PlanAuswertungModel WerteAus(LivePlanModel model)
        {
            return new PlanAuswertungModel
            {
                Date = model.StartTime.Value.Date,
                Band = model.Items.Where(i => !i.Item.IsPostservice && !i.Item.IsPreservice && (i.ItemType == PlanItemType.Song || i.Akteur == "Band")).Sum(i => i.Item.Length),
                NichtBand = model.Items.Where(i => !i.Item.IsPostservice && !i.Item.IsPreservice && i.ItemType != PlanItemType.Song && i.Akteur != "Band").Sum(i => i.Item.Length)
            };
        }

        public ActionResult Plan(int id)
        {
            var service = new Service(AuthConfig.ConsumerKey, AuthConfig.ConsumerSecret);

            var plan = service.GetPlan(id);

            var categories = PlanCategory.FromPlan(plan);

            return this.View(new PlanModel
            {
                Plan = plan,
                Categories = categories,
            });
        }

        public ActionResult Live(int id)
        {
            var service = new Service(AuthConfig.ConsumerKey, AuthConfig.ConsumerSecret);

            var plan = service.GetPlan(id);

            return this.View(new LivePlanModel(plan, service.GetArrangement));
        }

        public FileContentResult Pdf(int id)
        {
            byte[] data;
            string name = "PDF";
            using (var web = new PcoWebClient())
            {
                web.Login();

                var plan = web.GetPlan(id);

                name = plan.ServiceType.Name 
                    + "_" 
                    + plan.ServiceTimes.Select(t => ViewHelpers.ConvertToTimeZone(DateTime.Parse(t.StartsAt).ToUniversalTime()).ToString("yyyyMMdd_hhmm")).FirstOrDefault() 
                    + ".pdf";

                data = web.Print(id);

                web.Logout();
            }

            return this.File(data, "application/pdf", name);
        }

        public ActionResult PdfUpload(int id)
        {
            using (var web = new PcoWebClient())
            {
                web.Login();

                return this.PdfUpload(id, web);
            }
        }

        public ActionResult PdfUpload(int id, PcoWebClient web)
        {
            string key = ConfigurationManager.AppSettings["FilePostKey"];
            string url = ConfigurationManager.AppSettings["FileProgramPostUrl"];

            var plan = web.GetPlan(id);

            string name = plan.ServiceType.Name.Split(' ').First()
                + "_"
                + plan.ServiceTimes.Select(t => ViewHelpers.ConvertToTimeZone(DateTime.Parse(t.StartsAt).ToUniversalTime()).ToString("yyyyMMdd_HHmm")).FirstOrDefault();

            byte[] data = web.Print(id);

            //
            // Upload

            var uploadParams = new Dictionary<string, object>();

            uploadParams.Add("name", name);

            using (var md5 = MD5.Create())
            {
                string hash = DienstplanController.ByteArrayToHexString(md5.ComputeHash(data));
                uploadParams.Add("program", new FormUpload.FileParameter(data, name, "application/pdf"));

                uploadParams.Add(
                    "data",
                    DienstplanController.ByteArrayToHexString(md5.ComputeHash(Encoding.ASCII.GetBytes(name + key + hash))));
            }

            var response = FormUpload.MultipartFormDataPost(url, "feggiessen-pco.azurewebsites.net", uploadParams);

            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    return this.Content(reader.ReadToEnd());
                }
            }
        }

        [AllowAnonymous]
        public ActionResult UploadPdfsScheduler()
        {
            Task.Factory.StartNew(() => this.UploadPdfs());

            return this.Content(string.Empty);
        }

        public ActionResult UploadPdfs()
        {
            string key = ConfigurationManager.AppSettings["FilePostKey"];
            string url = ConfigurationManager.AppSettings["FileProgramPostUrl"];

            var uploadParams = new Dictionary<string, object>();

            using (var web = new PcoWebClient())
            {
                web.Login();

                var org = web.GetOrganisation();

                IEnumerable<string> serviceTypes = ConfigurationManager.AppSettings["FilePostServiceTypes"].Split(',');

                var plans = new List<int>();

                foreach (var st in serviceTypes)
                {
                    var allPlans = web.GetPlans(int.Parse(st), false).Take(5).Select(p => p.Id);

                    plans.AddRange(allPlans);
                }

                foreach (var planId in plans)
                {
                    this.PdfUpload(planId, web);
                }
            }

            return this.Content("OK");
        }
        

        public class ServiceTypeModel
        {
            public ServiceType ServiceType { get; set; }

            public IList<PlanIndex> Plans { get; set; }
        }

        public class PlanModel
        {
            public Plan Plan { get; set; }

            public IList<PlanCategory> Categories { get; set; }
        }
    }
}
