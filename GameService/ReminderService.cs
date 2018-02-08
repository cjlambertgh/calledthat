﻿using EmailService;
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

        public void SendGameweekPicksNotEnteredReminder()
        {
            throw new NotImplementedException();
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
