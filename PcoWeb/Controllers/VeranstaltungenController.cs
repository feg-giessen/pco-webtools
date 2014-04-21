using System;
using System.Collections.Generic;
using System.Linq;
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
                return View(HomeController.Organization.service_types);

            var service = new Service(AuthConfig.ConsumerKey, AuthConfig.ConsumerSecret);

            var organization = service.GetOrganisation();
            var plans = service.GetPlans(id.Value);

            return this.View(
                "Plans", 
                new ServiceTypeModel
                {
                    ServiceType = organization.service_types.FirstOrDefault(x => x.id == id),
                    Plans = plans,
                });
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
