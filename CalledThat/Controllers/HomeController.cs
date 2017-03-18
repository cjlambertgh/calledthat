using Data.BAL;
using Data.Repository;
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
        private readonly IUnitOfWork _db;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _db = unitOfWork;
        }

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

        public JsonResult AdminInit()
        {
            var admin = new Admin();
            admin.Initialise();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnityTest()
        {
            var teams = _db.Teams.Get();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}