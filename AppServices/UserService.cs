using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DAL.Identity;
using Data.DAL;
using Data.Models;

namespace AppServices
{
    public class UserService : IUserService
    {
        private readonly DataContextConnection context;

        public UserService()
        {
            context = new DataContextConnection();
        }
        public void CreatePlayer(string userId, string name, bool emailAlerts)
        {
            using (var db = context.Database)
            {
                db.Players.Add(new Player
                {
                    UserId = userId,
                    Name = name,
                    EmailAlerts = emailAlerts
                });
                db.SaveChanges();
            }
        }

        public Player GetPlayerByUserId(string userId)
        {
            using (var db = context.Database)
            {
                return db.Players.Get(p => p.UserId == userId).FirstOrDefault();
            }
        }

        public void UpdatePlayerName(Guid playerID, string newName)
        {
            using (var db = context.Database)
            {
                var player = db.Players.GetById(playerID);
                if (player != null)
                {
                    player.Name = newName;
                }
                db.SaveChanges();
            }
        }
    }
}
