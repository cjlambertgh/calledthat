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

        public ReminderService(IGameEmailService gameEmailService, IPlayerService playerService)
        {
            _gameEmailService = gameEmailService;
            _playerService = playerService;
        }

        public void SendGameweekPicksNotEnteredReminder(string url, IEnumerable<string> emailsThatHaveEnteredPicks)
        {
            var gameweekOpenEmailRecipients = _playerService.GetPlayersEmailsAcceptedAlerts();
            var mailsForPlayersNotMadePicks = gameweekOpenEmailRecipients.Except(emailsThatHaveEnteredPicks);
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
