using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using PcoBase;
using PcoWeb.Export;
using PcoWeb.Models;

namespace PcoWeb.Controllers
{
    [Authorize]
    public class DienstplanController : Controller
    {
        public ActionResult Index(DienstplanMatrixModel model, string type)
        {
            var service = new Service(AuthConfig.ConsumerKey, AuthConfig.ConsumerSecret);

            Organization org = service.GetOrganisation();

            if (model == null)
            {
                model = new DienstplanMatrixModel();
            }

            if (!model.Start.HasValue && !model.Ende.HasValue)
            {
                var startDate = DateTime.Today.AddMonths(-1 * ((DateTime.Today.Month - 1) % 3));
                var quartal = new Quartal(startDate);
                model.Start = quartal.Start;
                model.Ende = quartal.End;
            }

            model.ServiceTypeList.AddRange(org.ServiceTypes);

            if (model.ServiceTypes != null)
            {
                string permalink = this.Url.Action(
                    "Index",
                    "Dienstplan",
                    new
                    {
                        Start = model.Start.HasValue ? model.Start.Value.ToString("d", CultureInfo.InvariantCulture) : string.Empty,
                        Ende = model.Ende.HasValue ? model.Ende.Value.ToString("d", CultureInfo.InvariantCulture) : string.Empty,
                    },
                    HttpContext.Request.Url.Scheme);
                permalink += "&ServiceTypes=" + string.Join("&ServiceTypes=", model.ServiceTypes.Where(t => t != "false"));
                ViewBag.permalink = HttpUtility.UrlDecode(permalink);

                var plans = new List<Plan>();

                foreach (var st in model.ServiceTypeList)
                {
                    if (!model.ServiceTypes.Contains(st.Id.ToString(CultureInfo.InvariantCulture)))
                        continue;

                    var allPlans = service.GetPlans(st.Id, true);

                    plans.AddRange(allPlans
                        .Select(p => new { Plan = p, Date = ViewHelpers.FormtatedDate(p.Dates) })
                        .Where(p => p.Date.HasValue && p.Date >= model.Start && p.Date <= model.Ende)
                        .Select(p => service.GetPlan(p.Plan.Id)));
                }

                var planModels = plans.Select(p => new MatrixPlan(p)).OrderBy(p => p.Date).ToList();

                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("de-DE");

                string daterange = string.Format("{0:ddMM}-{1:ddMM-yyyy}", model.Start, model.Ende);

                if (type != null && type.IndexOf("Excel", StringComparison.InvariantCultureIgnoreCase) > -1 && model.Start.HasValue && model.Ende.HasValue)
                {
                    var stream = ExcelMatrix.Generate(org, model.Start.Value, model.Ende.Value, planModels);

                    return this.File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Dienstplan_" + daterange + ".xlsx");
                }
                else if (type != null && type.IndexOf("Veranstaltungen", StringComparison.InvariantCultureIgnoreCase) > -1 && model.Start.HasValue && model.Ende.HasValue)
                {
                    var stream = ExcelTopics.Generate(planModels);

                    return this.File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Veranstaltungen_" + daterange + ".xlsx");
                }
                else if (type != null && type.IndexOf("a3", StringComparison.InvariantCultureIgnoreCase) > -1 && model.Start.HasValue && model.Ende.HasValue)
                {
                    var stream = PdfMatrix.Generate(org, model.Start.Value, model.Ende.Value, planModels, true);

                    return this.File(stream, "application/pdf", "Dienstplan_A3_" + daterange + ".pdf");
                }
                else if (type != null && type.IndexOf("Pdf", StringComparison.InvariantCultureIgnoreCase) > -1 && model.Start.HasValue && model.Ende.HasValue)
                {
                    var stream = PdfMatrix.Generate(org, model.Start.Value, model.Ende.Value, planModels, false);

                    return this.File(stream, "application/pdf", "Dienstplan_" + daterange + ".pdf");
                }
                else
                {
                    ViewBag.Links = true;

                    model.Items = planModels;
                }
            }

            return this.View(model);
        }

        [AllowAnonymous]
        public ActionResult Generate()
        {
            var quartal = new Quartal();

            var reponse = this.Upload(quartal);
            string content;

            using (var reader = new StreamReader(reponse.GetResponseStream()))
            {
                content = reader.ReadToEnd();
            }

            if (content != "OK")
                throw new HttpException(500, content);

            quartal = new Quartal(quartal.Start.AddMonths(3));
            reponse = this.Upload(quartal);

            using (var reader = new StreamReader(reponse.GetResponseStream()))
            {
                content = reader.ReadToEnd();
            }

            if (content != "OK")
                throw new HttpException(500, content);
            
            return this.Content(string.Empty);
        }

        [AllowAnonymous]
        public ActionResult GenerateScheduler()
        {
            Task.Factory.StartNew(() => this.Generate());

            return this.Content(string.Empty);
        }

        private HttpWebResponse Upload(Quartal quartal)
        {
            string key = ConfigurationManager.AppSettings["FilePostKey"];
            string url = ConfigurationManager.AppSettings["FilePostUrl"];

            var uploadParams = new Dictionary<string, object>();
            byte[] file;

            using (var web = new PcoWebClient())
            {
                web.Login();

                var org = web.GetOrganisation();

                IEnumerable<string> serviceTypes = ConfigurationManager.AppSettings["FilePostServiceTypes"].Split(',');

                var plans = new List<Plan>();

                foreach (var st in serviceTypes)
                {
                    var allPlans = web.GetPlans(int.Parse(st), true);

                    plans.AddRange(allPlans
                        .Select(p => new { Plan = p, Date = ViewHelpers.FormtatedDate(p.Dates) })
                        .Where(p => p.Date.HasValue && p.Date >= quartal.Start && p.Date <= quartal.End)
                        .Select(p => web.GetPlan(p.Plan.Id)));
                }

                var planModels = plans.Select(p => new MatrixPlan(p)).OrderBy(p => p.Date).ToList();

                uploadParams.Add("year", quartal.Year.ToString());
                uploadParams.Add("quartal", quartal.Nummer.ToString());

                file = PdfMatrix.Generate(org, quartal.Start, quartal.End, planModels, false);
                string a4Hash = ByteArrayToHexString(MD5CryptoServiceProvider.Create().ComputeHash(file));
                uploadParams.Add("a4", new FormUpload.FileParameter(file, "Dienstplan_A4", "application/pdf"));

                file = PdfMatrix.Generate(org, quartal.Start, quartal.End, planModels, true);
                string a3Hash = ByteArrayToHexString(MD5CryptoServiceProvider.Create().ComputeHash(file));
                uploadParams.Add("a3", new FormUpload.FileParameter(file, "Dienstplan_A3", "application/pdf"));

                uploadParams.Add("data", ByteArrayToHexString(
                    MD5CryptoServiceProvider.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(quartal.Year.ToString() + quartal.Nummer + key + a4Hash + a3Hash))));

                return FormUpload.MultipartFormDataPost(url, "feggiessen-pco.azurewebsites.net", uploadParams);
            }
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private class Quartal
        {
            public Quartal(int year, int quartal)
            {
                this.Start = new DateTime(year, ((quartal - 1) * 3) + 1, 1, 0, 0, 0, 0);
                this.End = this.Start.AddMonths(3).AddDays(-1);

                this.Year = year;
                this.Nummer = quartal;                    
            }

            public Quartal(DateTime startDate)
            {
                this.Start = new DateTime(startDate.Year, startDate.Month, 1, 0, 0, 0, 0); ;
                this.Year = startDate.Year;

                this.End = this.Start.AddMonths(3).AddDays(-1);
                this.Nummer = (int)Math.Round((startDate.Month + 1) / 3m, 0, MidpointRounding.AwayFromZero);
            }

            public Quartal()
                : this(DateTime.Today.AddMonths(-1 * ((DateTime.Today.Month - 1) % 3)))
            {
            }

            public int Year { get; private set; }

            public int Nummer { get; private set; }

            public DateTime Start { get; private set; }

            public DateTime End { get; private set; }
        }
    }
}
