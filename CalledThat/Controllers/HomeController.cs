using CalledThat.ViewModels;
using Data.Interfaces;
using Data.Repository;
using DataAPI.Implementations;
using GameService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalledThat.Controllers
{
    public class HomeController : AsyncController
    {
        private readonly IDataContextConnection _db;
        private readonly IGameService _gameService;

        public HomeController(IDataContextConnection unitOfWork, IGameService gameService)
        {
            _db = unitOfWork;
            _gameService = gameService;
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
            //var admin = new Admin(_db);
            //admin.Initialise();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult UnityTest()
        {
            var db = _db.Database;
            var teams = db.Teams.Get();
            var viewModel = teams.Select(x => new TeamViewModel { TeamName = x.Name, BadgeUrl = x.BadgeUrl }).ToList();


            return View(viewModel);
        }

        public JsonResult GameServiceTest()
        {
            _gameService.UpdateAll();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Fixtures()
        {

        }
    }
}