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
        void CreatePlayer(AppUser user, string name);
        void UpdatePlayerName(Guid playerID, string newName);
        Player GetPlayerByUserId(string userId);
    }
}
