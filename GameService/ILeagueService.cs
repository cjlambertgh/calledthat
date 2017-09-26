using Data.Models;
using Data.Models.Procs;
using System;
using System.Collections.Generic;

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
        IEnumerable<LeagueTable> GetLeagueTable(Guid leagueId);
        bool IsPlayerALeagueOwner(Guid playerId, League league);
    }
}
