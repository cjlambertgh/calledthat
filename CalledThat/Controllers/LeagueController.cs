using AppServices;
using CalledThat.ViewModels.League;
using GameService;
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

        public LeagueController(ILeagueService leagueService, IUserService userService)
            : base(userService)
        {
            _leagueService = leagueService;
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
        public ActionResult Join()
        {
            return View();
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

            var leagueRows = _leagueService.GetLeagueTable(leagueId);

            var model = new ViewSingleLeagueViewModel
            {
                LeagueName = league.Name,
                LeagueId = league.Id,
                LeagueTableRows = leagueRows.Select(lr => new LeagueTableRow
                {
                    PlayerName = lr.PlayerName,
                    GameweekPoints = lr.GameweekPoints,
                    TotalPoints = lr.TotalPoints,
                    PlayerId = lr.PlayerId,
                    GameWeek = lr.GameweekNumber
                }).ToList()
            };
            return View(model);
        }
    }
}