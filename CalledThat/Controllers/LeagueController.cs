using AppServices;
using CalledThat.ViewModels.Game;
using CalledThat.ViewModels.League;
using EmailService;
using GameServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalledThat.Controllers
{
    public class LeagueController : BaseController
    {
        private readonly ILeagueService _leagueService;
        private readonly IGameService _gameService;
        private readonly IPlayerService _playerService;
        private const string GameweekResultPartial = "~/Views/Game/Partials/_GameweekREsults.cshtml";

        public LeagueController(ILeagueService leagueService, IUserService userService, IMailService mailService, IGameService gameService,
            IPlayerService playerService)
            : base(userService, mailService)
        {
            _leagueService = leagueService;
            _gameService = gameService;
            _playerService = playerService;
        }
        // GET: League
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var playerLeagues = _leagueService.GetPlayerLeagues(CurrentPlayerId);
            var viewModel = new ViewLeaguesViewModel
            {
                LeagueItems = playerLeagues.Select(l => new LeagueItem
                {
                    NumberOfPlayers = l.PlayerLeagues.Count,
                    LeagueId = l.Id,
                    LeagueName = l.Name,
                    Competition = l.Competition.Name,
                    LeagueOwner = l.LeagueOwners.Any(lo => lo.PlayerId == CurrentPlayerId)
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CreateLeagueViewModel
            {
                Competitions = _leagueService.GetAvailableCompetitions().ToDictionary(k => k.Id, v => v.Name)
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateLeagueViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            _leagueService.CreateLeague(model.SelectedCompetitionId, CurrentPlayerId, model.LeagueName);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Join(string leagueCode)
        {
            if(string.IsNullOrEmpty(leagueCode))
            {
                return View();
            }
            if (!_leagueService.IsInviteCodeValid(CurrentPlayerId, leagueCode))
            {
                AddError("Unable to join league. Ether league code is invalid, or you have already joined this league.");
                return RedirectToAction("Join");
            }

            //TODO: maybe not auto join here - return Join view with code pre-populated
            _leagueService.JoinLeague(CurrentPlayerId, leagueCode);

            AddSuccess("Successfully joined league");

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Join(JoinLeagueViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            if (!_leagueService.IsInviteCodeValid(CurrentPlayerId, model.LeagueCode))
            {
                AddError("Unable to join league. Ether league code is invalid, or you have already joined this league.");
                return View(model);                
            }

            _leagueService.JoinLeague(CurrentPlayerId, model.LeagueCode);

            AddSuccess("Successfully joined league");

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult View(Guid leagueId)
        {
            var league = _leagueService.GetLeague(leagueId);
            if(league == null)
            {
                AddError("Error opening league details");
                RedirectToAction("Index");
            }

            var isOwner = _leagueService.IsPlayerALeagueOwner(CurrentPlayerId, league);

            var leagueRows = _leagueService.GetLeagueTable(leagueId);

            var leagueStats = _leagueService.GetLeagueStats(leagueId);

            var model = new ViewSingleLeagueViewModel
            {
                LeagueName = league.Name,
                LeagueId = league.Id,
                IsLeagueOwner = isOwner,
                InviteCode = league.InviteCode,
                LeagueTableRows = leagueRows.Select(lr => new LeagueTableRow
                {
                    PlayerName = lr.PlayerName,
                    GameweekPoints = lr.GameweekPoints,
                    TotalPoints = lr.TotalPoints,
                    PlayerId = lr.PlayerId,
                    GameWeek = lr.GameweekNumber
                }).ToList(),
                LeagueStats = leagueStats.ToList()
            };
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Results(int week, Guid playerId, int? totalWeeks)
        {
            var player = _playerService.GetPlayerById(playerId);
            var playerName = player.Name;
            var results = _gameService.GetPlayerResults(playerId, week).ToList();
            if(totalWeeks == null)
            {
                totalWeeks = _gameService.GetCurrentGameweek();
            }
            var viewModel = new ResultsViewModel
            {
                PlayerResults = results,
                Gameweek = week,
                TotalGameweeks = (int)totalWeeks,
                PlayerId = playerId,
                PlayerName = playerName
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView(GameweekResultPartial, viewModel);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Manage(Guid leagueId)
        {
            var league = _leagueService.GetLeague(leagueId);
            var vm = new ManageLeagueViewModel
            {
                League = league,
                AvailableGameweeks = league.Competition.GameWeeks.OrderBy(gw => gw.Number).Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Number.ToString()
                }),
                SelectedStartWeek = league.GameweekIdScoringStarts ?? Guid.Empty
        };
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateStartweek(ManageLeagueViewModel model)
        {
            _leagueService.UpdateStartweek(model.League.Id, model.SelectedStartWeek);
            AddSuccess("Starting Gameweek Updated");
            return RedirectToAction("Manage", new { leagueId = model.League.Id });
        }

        [HttpPost]
        [Authorize]
        public JsonResult RemovePlayerFromLeague(Guid leagueId, Guid playerId)
        {
            _leagueService.RemovePlayerFromLeague(leagueId, playerId);
            return Json("ok");
        }
    }
}