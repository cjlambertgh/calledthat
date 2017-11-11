using Data.Models;
using System;
using System.Collections.Generic;

namespace GameService
{
    public interface IPlayerService
    {
        string GetPlayerName(Guid playerId);
        Player GetPlayerById(Guid playerId);
        IEnumerable<string> GetPlayersEmailsAcceptedAlerts();
    }
}
