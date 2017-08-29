using AppServices;
using CalledThat.ViewModels;
using GameService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalledThat.Controllers
{
    public class GameController : BaseController
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService, IUserService userService)
            :base(userService)
        {
            _gameService = gameService;
        }

        // GET: Game
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var playerId = CurrentUser.Players.FirstOrDefault()?.Id;

            if (playerId == null)
            {
                throw new ArgumentNullException(nameof(playerId));
            }

            var viewModel = new AddPicksViewModel();
            var currentGameweek = _gameService.GetCurrentGameweek();
            viewModel.Gameweek = currentGameweek;
            DateTime openDate, closeDate;
            _gameService.PopulatePickOpenCloseDates(out openDate, out closeDate);
            viewModel.OpenDateTime = openDate;
            viewModel.CloseDateTime = closeDate;

            var fixtures = _gameService.GetGameWeekFixtures();

            foreach (var fixture in fixtures)
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

            var picks = _gameService.GetPlayerPicks((Guid)playerId, currentGameweek);
            if (picks.Any())
            {
                foreach (var pick in picks)
                {
                    var pickitem = viewModel.PickItems.Where(pi => pi.FixtureId == pick.FixtureId).SingleOrDefault();
                    if(pickitem != null)
                    {
                        pickitem.HomeScore = pick.HomeScore.ToString();
                        pickitem.AwayScore = pick.AwayScore.ToString();
                        pickitem.Banker = pick.Banker;
                        pickitem.Double = pick.Double;
                    }
                }
            }

            viewModel.ReadOnly = !_gameService.IsGameweekOpen(currentGameweek);            

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AddPicksViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var playerId = CurrentUser.Players.FirstOrDefault()?.Id;

            if(playerId == null)
            {
                throw new ArgumentNullException(nameof(playerId));
            }

            

            model.PickItems.ForEach(item =>
            {
                _gameService.AddPick((Guid)playerId, item.FixtureId, int.Parse(item.HomeScore), int.Parse(item.AwayScore), item.Banker, item.Double);
            });

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Results(int? week = null)
        {

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Stats()
        {
            return View();
        }
    }
}