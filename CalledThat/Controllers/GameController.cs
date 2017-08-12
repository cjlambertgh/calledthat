using CalledThat.ViewModels;
using GameService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalledThat.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // GET: Game
        [Authorize]
        public ActionResult Index()
        {
            var fixtures = _gameService.GetGameWeekFixtures();
            var playerId = Guid.Empty;
            var viewModel = new AddPicksViewModel
            {
                PlayerId = playerId
            };

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

            return View(viewModel);
        }
    }
}