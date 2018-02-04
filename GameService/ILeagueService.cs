using Data.Models;
using Data.Models.Procs;
using System;
using System.Collections.Generic;

namespace GameServices
{
    public interface ILeagueService
    {
        void CreateLeague(Guid competitionId, Guid playerIdOwner, string leagueName);
        IEnumerable<Competition> GetAvailableCompetitions();
        IEnumerable<League> GetPlayerLeagues(Guid playerId);
        void JoinLeague(Guid playerId, string inviteCode);
        bool IsInviteCodeValid(Guid playerId, string code);
        League GetLeague(Guid leagueId);
        League GetLeagueByInviteCode(string inviteCode);
        IEnumerable<LeagueTable> GetLeagueTable(Guid leagueId);
        bool IsPlayerALeagueOwner(Guid playerId, League league);
        IEnumerable<LeagueStats> GetLeagueStats(Guid leagueId);
        void RemovePlayerFromLeague(Guid leagueId, Guid playerId);
        void UpdateStartweek(Guid leagueId, Guid gameweekId);
    }
}
