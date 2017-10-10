using Data.Models;
using System;

namespace GameService
{
    public interface IPlayerService
    {
        string GetPlayerName(Guid playerId);
        Player GetPlayerById(Guid playerId);
    }
}
