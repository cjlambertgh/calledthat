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
            var competition = comps.Where(c => c.League == "PL").FirstOrDefault();
            var league = data.GetLeagues(competition.Id).FirstOrDefault();
            var fixtures = data.GetMatchdayFixtures(competition.Id, league.MatchDay);
            var teams = data.GetTeams(competition.Id);
            return Json(teams, JsonRequestBehavior.AllowGet);
        }
    }
}