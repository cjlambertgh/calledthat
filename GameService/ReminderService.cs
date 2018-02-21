using EmailService;
using GameServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServices
{
    public class ReminderService : IReminderService
    {
        private readonly IGameEmailService _gameEmailService;
        private readonly IPlayerService _playerService;
        private readonly IGameService _gameService;

        public ReminderService(IGameEmailService gameEmailService, IPlayerService playerService,
            IGameService gameService)
        {
            _gameEmailService = gameEmailService;
            _playerService = playerService;
            _gameService = gameService;
        }

        public void SendGameweekPicksNotEnteredReminder(string url)
        {
            var picks = _gameService.GetAllPlayerPicks(_gameService.GetCurrentGameweek());
            var playerEmailsWithPicks = picks.Select(p => p.Player.AppUser.Email).Distinct();
            var gameweekOpenEmailRecipients = _playerService.GetPlayersEmailsAcceptedAlerts();
            var mailsForPlayersNotMadePicks = gameweekOpenEmailRecipients.Except(playerEmailsWithPicks);
            Parallel.ForEach(mailsForPlayersNotMadePicks, (address) =>
            {
                _gameEmailService.SendPicksNotEnteredEmail(address, url);
            });
        }

        public void SendNewGameweekReminder(string url)
        {
            var gameweekOpenEmailRecipients = _playerService.GetPlayersEmailsAcceptedAlerts();
            Parallel.ForEach(gameweekOpenEmailRecipients, (address) =>
            {
                _gameEmailService.SendGameweekOpenEmail(address, url);
            });
        }
    }
}
