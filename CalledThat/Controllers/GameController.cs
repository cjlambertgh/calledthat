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
    }
}