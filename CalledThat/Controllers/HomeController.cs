using DataAPI.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalledThat.Controllers
{
    public class HomeController : AsyncController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ApiTest()
        {
            var data = new DataService();
            var comps = data.GetCompetition();
            var league = comps.Where(c => c.League == "PL").FirstOrDefault();
            var teams = data.GetTeams(league.Id);
            return Json(teams, JsonRequestBehavior.AllowGet);
        }
    }
}