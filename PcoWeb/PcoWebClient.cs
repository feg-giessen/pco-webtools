using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using PcoBase;

namespace PcoWeb
{
    public class PcoWebClient : IDisposable
    {
        private const string PlansLink = "https://www.planningcenteronline.com/service_types/{0}/plans.json";
        
        private const string PlanLink = "https://www.planningcenteronline.com/plans/{0}.json";

        private const string OrganizationLink = "https://www.planningcenteronline.com/organization.json";

        private const string PersonsLink = "https://www.planningcenteronline.com/people.json";

        private const string PdfNewLink = "https://planningcenteronline.com/plans/{0}/print/new";

        private const string PdfLink = "https://planningcenteronline.com/plans/{0}/print.pdf";

        private readonly CookieContainer cookies;

        private bool disposed;

        private bool login;

        public PcoWebClient()
        {
            this.cookies = new CookieContainer();
        }

        public static bool IsAvailable(Organization organisation)
        {
            return organisation != null
                && ConfigurationManager.AppSettings["WebApiOrganization"] == organisation.Id.ToString()
                && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebApiEmail"])
                && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebApiPassword"]);
        }

        public void Login()
        {
            string content = this.Get(@"https://accounts.planningcenteronline.com/");

            var hiddenInput = new Regex(@"\<input[^\>]+type\=" + "\"hidden\"" + @"[^\>]*\>", RegexOptions.IgnoreCase);
            var nameAttr = new Regex(@"name\=" + "\"(?<value>[^\"]*)\"", RegexOptions.IgnoreCase);
            var valueAttr = new Regex(@"value\=" + "\"(?<value>[^\"]*)\"", RegexOptions.IgnoreCase);
            var hiddenFields = hiddenInput.Matches(content);

            var values = new Dictionary<string, string>();

            foreach (Match match in hiddenFields) {
                var name = nameAttr.Match(match.Value);
                var value = valueAttr.Match(match.Value);

                if (name.Success && value.Success) {
                    values.Add(name.Groups["value"].Value, value.Groups["value"].Value);
                }
            }

            values.Add("email", ConfigurationManager.AppSettings["WebApiEmail"]);
            values.Add("password", ConfigurationManager.AppSettings["WebApiPassword"]);
            values.Add("Submit", "Go");

            this.Post("https://accounts.planningcenteronline.com/login", values);

            this.login = true;
        }

        public void Logout()
        {
            this.Get(@"https://accounts.planningcenteronline.com/logout");
            this.login = false;
        }

        public byte[] Print(int id)
        {
            // https://planningcenteronline.com/plans/27766677/print/new
            string content = this.Get(string.Format(PdfNewLink, id));

            var match1 = Regex.Match(content, "name\\=\"authenticity_token\" value\\=\"(?<value>[^\"]+)\"", RegexOptions.IgnoreCase);
            var match2 = Regex.Matches(content, @"time_(?<id>\d+)\[print\]", RegexOptions.IgnoreCase);
            
            if (match1.Success)
            {
                string token = match1.Groups["value"].Value;
                
                var values = new Dictionary<string, string>
                {
                    { "authenticity_token", token },
                    { "ministry[print_to]", "pdf" },
                    { "ministry[print_page_size]", "A4" },
                    { "ministry[print_orientation]", "Portrait" },
                    { "ministry[print_margin]", "0.25in" },
                    { "ministry[print_font_size]", "8pt" },
                    { "ministry[print_logo]", "0" },
                    { "ministry[print_length]", "1" },
                    { "ministry[print_item_detail]", "0" },
                    { "ministry[print_in_color]", "0" },
                    { "ministry[print_media]", "0" },
                    { "ministry[print_rehearsal_times]", "0" },
                    { "ministry[print_other_times]", "0" },
                    { "show_items_without_times", "0" },
                    { "ministry[print_sequences]", "1" },
                    { "ministry[print_columns]", "true" }
                };

                string timeId = match2.OfType<Match>().Select(m => m.Groups["id"].Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(timeId))
                {
                    values.Add("time_" + timeId + "[print]", "1");
                }

                var match3 = Regex.Matches(content, @"ministry\[plan_item_note_categories\]\[(?<id>\d+)\]\[print\]", RegexOptions.IgnoreCase);
                var categories = match3.OfType<Match>().Select(m => m.Groups["id"].Value).Distinct().ToList();

                foreach (string catId in categories)
                {
                    values.Add("ministry[plan_item_note_categories][" + catId + "][print]", "1");
                }

                return this.PostByte(string.Format(PdfLink, id), values);
            }

            return new byte[0];
        }

        public Organization GetOrganisation()
        {
            var content = this.Get(OrganizationLink);

            return JsonConvert.DeserializeObject<Organization>(content);
        }

        public IList<PlanIndex> GetPlans(int serviceType, bool all = false)
        {
            var plansEndpoint = string.Format(PlansLink, serviceType) + (all ? "?all=true" : string.Empty);

            var content = this.Get(plansEndpoint);

            return JsonConvert.DeserializeObject<List<PlanIndex>>(content);
        }

        public Plan GetPlan(int planId)
        {
            var content = this.Get(string.Format(PlanLink, planId));

            return JsonConvert.DeserializeObject<Plan>(content);
        }

        public IList<Person> GetPersons()
        {
            var content = this.Get(PersonsLink);

            return JsonConvert.DeserializeObject<PersonsResponse>(content).People;
        }

        public List<MinistryPositionsResult> GetMinistryPositions(string query)
        {
            if (!this.login)
            {
                this.Login();
            }

            var content = this.Get(string.Format(
                    @"https://planningcenteronline.com/ministry_positions.json?name={0}&with_categories=1&in_ministry_path=&excluded_position_ids=&excluded_category_ids=",
                    query));

            return JsonConvert.DeserializeObject<List<MinistryPositionsResult>>(content);
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                if (this.login)
                {
                    this.Logout();
                }

                this.disposed = true;
            }
        }

        private string Get(string url)
        {
            var request = HttpWebRequest.CreateHttp(url);
            this.ConfigureRequest(request);

            var response = request.GetResponse();

            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private string Post(string url, IDictionary<string, string> values)
        {
            var request = HttpWebRequest.CreateHttp(url);
            this.ConfigureRequest(request);

            request.Method = "POST";
            
            string postData = string.Join(
                "&", 
                values.Select(kv => string.Format("{0}={1}", HttpUtility.UrlEncode(kv.Key), HttpUtility.UrlEncode(kv.Value))));

            byte[] data = Encoding.ASCII.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            var response = request.GetResponse();

            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private byte[] PostByte(string url, IDictionary<string, string> values)
        {
            var request = HttpWebRequest.CreateHttp(url);
            this.ConfigureRequest(request);

            request.Method = "POST";

            string postData = string.Join(
                "&",
                values.Select(kv => string.Format("{0}={1}", HttpUtility.UrlEncode(kv.Key), HttpUtility.UrlEncode(kv.Value))));

            byte[] data = Encoding.ASCII.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            var response = request.GetResponse();

            using (var stream = response.GetResponseStream())
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);

                    return ms.ToArray();
                }
            }
        }

        private void ConfigureRequest(HttpWebRequest request)
        {
            request.UserAgent = string.Format("Mozilla/5.0 (Windows NT 6.1; rv:27.0) Gecko/20100101 Firefox/27.0");
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Headers[HttpRequestHeader.AcceptLanguage] = "de-de,de;q=0.8,en-us;q=0.5,en;q=0.3";
            request.KeepAlive = true;

            request.CookieContainer = this.cookies;
        }
    }
}