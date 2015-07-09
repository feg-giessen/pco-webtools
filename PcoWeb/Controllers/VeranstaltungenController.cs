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
