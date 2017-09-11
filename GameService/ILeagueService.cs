using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    public interface ILeagueService
    {
        void CreateLeague(Guid competitionId, Guid playerIdOwner, string leagueName);
        IEnumerable<Competition> GetAvailableCompetitions();
        IEnumerable<League> GetPlayerLeagues(Guid playerId);
        void JoinLeague(Guid playerId, string inviteCode);
        bool IsInviteCodeValid(Guid playerId, string code);
        League GetLeague(Guid leagueId);
    }
}
