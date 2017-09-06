using AppServices;
using CalledThat.ViewModels;
using CalledThat.ViewModels.Game;
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
        private const string GameweekResultPartial = "~/Views/Game/Partials/_GameweekREsults.cshtml";

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
        [Route("game/results/{week?}/{playerId?}")]
        [OutputCache(Duration = 300, VaryByParam = "week;playerId")]
        public ActionResult Results(int? week = null, Guid? playerId = null)
        {
            if (playerId == null)
            {
                var player = CurrentUser.Players.FirstOrDefault();                
                if (player == null)
                {
                    throw new ArgumentNullException(nameof(player));
                }
                playerId = player.Id;
            }
            var currentGameweek = _gameService.GetCurrentGameweek();
            if (week == null)
            {
                week = currentGameweek;
            }
            var results = _gameService.GetPlayerResults((Guid)playerId, week);
            var viewModel = new ResultsViewModel
            {
                PlayerResults = results.ToList(),
                Gameweek = (int)week,
                TotalGameweeks = currentGameweek,
                PlayerId = (Guid)playerId
            };
            if(Request.IsAjaxRequest())
            {
                return PartialView(GameweekResultPartial, viewModel);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Stats()
        {
            return View();
        }
    }
}