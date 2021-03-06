﻿using AppServices;
using CalledThat.ViewModels;
using Data.Interfaces;
using DataAPI.Implementations;
using EmailService;
using GameServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CalledThat.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IDataContextConnection _db;
        private readonly IGameService _gameService;
        private readonly IMailService _mailService;
        private readonly IGameEmailService _gameEmailService;
        private readonly IPlayerService _playerService;

        public HomeController(IDataContextConnection unitOfWork, IGameService gameService, IUserService userService, 
            IMailService mailService, IGameEmailService gameEmailService, IPlayerService playerService)
            :base(userService, mailService)
        {
            _db = unitOfWork;
            _gameService = gameService;
            _mailService = mailService;
            _gameEmailService = gameEmailService;
            _playerService = playerService;
        }

        // GET: Home
        public ActionResult Index()
        {
            //_mailService.Send("phateuk@gmail.com", "test subject", "test body");
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

        public JsonResult Test2()
        {
            //var api = new FootballDataApiV2.Implementations.CompetitionApi();
            //var api = new FootballDataApiV2.Implementations.TeamApi();
            var api = new FootballDataApiV2.Implementations.MatchApi();
            api.CompetitionId = 2021;
            api.MatchDay = 1;
            var d = api.Get();
            return Json(d, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AdminInit()
        {
            //var admin = new Admin(_db);
            //admin.Initialise();

            _gameService.Initialise();
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
            var gameweekOpenEmailRecipients = _playerService.GetPlayersEmailsAcceptedAlerts();

            _gameService.UpdateApiData(Url.Action("Index", "Home", null, Request.Url.Scheme));
            _gameService.UpdateResults();
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