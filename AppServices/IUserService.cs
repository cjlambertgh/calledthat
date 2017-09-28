using Data.DAL.Identity;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServices
{
    public interface IUserService
    {
        void CreatePlayer(string userId, string name, bool emailAlerts);
        void UpdatePlayerName(Guid playerID, string newName);
        Player GetPlayerByUserId(string userId);
    }
}
