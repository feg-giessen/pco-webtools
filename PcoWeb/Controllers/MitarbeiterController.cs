using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PcoBase;

namespace PcoWeb.Controllers
{
    [Authorize]
    public class MitarbeiterController : Controller
    {
        //
        // GET: /Mitarbeiter/

        public ActionResult Index()
        {
            List<MinistryPositionsResult> ministryPositions = null;

            if (PcoWebClient.IsAvailable(HomeController.Organization))
            {
                using (var web = new PcoWebClient())
                {
                    ministryPositions = web.GetMinistryPositions("Licht");
                }
            }

            return this.View(ministryPositions);
        }
    }
}
