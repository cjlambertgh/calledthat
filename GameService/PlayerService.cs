using Data.Interfaces;
using Data.Models;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _db;

        public PlayerService(IDataContextConnection db)
        {
            _db = db.Database;
        }
        public string GetPlayerName(Guid playerId)
        {
            var player = GetPlayer(playerId);
            if(player != null)
            {
                return player.Name;
            }
            return null;
        }

        private Player GetPlayer(Guid playerId)
        {
            return _db.Players.GetById(playerId);
        }

        public Player GetPlayerById(Guid playerId)
        {
            return GetPlayer(playerId);
        }
    }
}
