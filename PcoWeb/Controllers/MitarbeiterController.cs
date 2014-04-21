using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PcoWeb.Controllers
{
    [Authorize]
    public class MitarbeiterController : Controller
    {
        //
        // GET: /Mitarbeiter/

        public ActionResult Index()
        {
            return View();
        }

    }
}
