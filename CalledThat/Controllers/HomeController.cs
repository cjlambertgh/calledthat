using CalledThat.ViewModels;
using Data.Interfaces;
using DataAPI.Implementations;
using GameService;
using System;
using System.Linq;
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
            _gameService.UpdateApiData();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Fixtures()
        {
            var fixtures = _gameService.GetGameWeekFixtures();
            return View(fixtures);
        }

        [HttpGet]
        public ActionResult AddPicks()
        {
            //TODO load picks if exist for current week
            var fixtures = _gameService.GetGameWeekFixtures();
            var playerId = Guid.Empty;
            var viewModel = new AddPicksViewModel
            {
                PlayerId = playerId
            };

            foreach(var fixture in fixtures)
            {
                viewModel.PickItems.Add(new PickItem
                {
                    HomeTeamName = fixture.HomeTeam.Name,
                    HomeTeamBadgeUrl = fixture.HomeTeam.BadgeUrl,
                    AwayTeamName = fixture.AwayTeam.Name,
                    AwayTeamBadgeUrl = fixture.AwayTeam.BadgeUrl,
                    KickOffTime = fixture.KickOffDateTime,
                    FixtureId = fixture.Id
                });
            }
            
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddPicks(AddPicksViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return View(viewModel);
            }

            viewModel.PickItems.ForEach(item =>
            {
                _gameService.AddPick(viewModel.PlayerId, item.FixtureId, int.Parse(item.HomeScore), int.Parse(item.AwayScore), item.Banker, item.Double);
            });

            return View(viewModel);
        }
    }
}