using Data.Interfaces;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using System.Configuration;

namespace GameService
{
    public class LeagueService : ILeagueService
    {
        private readonly IUnitOfWork _db;

        public LeagueService(IDataContextConnection db)
        {
            _db = db.Database;
        }
        public void CreateLeague(Guid competitionId, Guid playerIdOwner, string leagueName)
        {
            var league = new League
            {
                CompetitionId = competitionId,
                Name = leagueName,
                InviteCode = GenerateInviteCode()
            };

            _db.Leagues.Add(league);

            var playerLeague = new PlayerLeagues
            {
                League = league,
                PlayerId = playerIdOwner
            };

            _db.PlayerLeagues.Add(playerLeague);

            var leagueowner = new LeagueOwners
            {
                League = league,
                PlayerId = playerIdOwner
            };

            _db.LeagueOwners.Add(leagueowner);

            _db.SaveChanges();
        }

        public IEnumerable<Competition> GetAvailableCompetitions()
        {
            var currentSeasonYear = int.Parse(ConfigurationManager.AppSettings["CurrentSeasonYear"]);
            return _db.Competitions.Get(c => c.Season.CurrentSeasonYear == currentSeasonYear);
        }

        public League GetLeague(Guid leagueId)
        {
            return _db.Leagues.FirstOrDefault(l => l.Id == leagueId);
        }

        public IEnumerable<League> GetPlayerLeagues(Guid playerId)
        {
            return _db.Leagues.Where(l => l.PlayerLeagues.Any(pl => pl.PlayerId == playerId)).ToList();
        }

        public bool IsInviteCodeValid(Guid playerId, string code)
        {
            var league = _db.Leagues.SingleOrDefault(l => l.InviteCode == code);
            if(league != null)
            {
                var playerIdsInLeague = league.PlayerLeagues.Select(pl => pl.PlayerId);
                if (!playerIdsInLeague.Contains(playerId))
                {
                    return true;
                }
            }
            return false;
        }

        public void JoinLeague(Guid playerId, string inviteCode)
        {
            var league = _db.Leagues.FirstOrDefault(l => l.InviteCode == inviteCode);
            if(league == null)
            {
                throw new ArgumentOutOfRangeException(nameof(inviteCode));
            }

            _db.PlayerLeagues.Add(new PlayerLeagues
            {
                League = league,
                PlayerId = playerId
            });

            _db.SaveChanges();
        }

        private string GenerateInviteCode()
        {
            Random r = new Random();
            var randNum = r.Next(100000);
            var inviteCode = randNum.ToString("D5");
            var existingCodes = _db.Leagues.Get().Select(l => l.InviteCode).ToList();
            while(existingCodes.Contains(inviteCode))
            {
                randNum = r.Next(100000);
                inviteCode = randNum.ToString("D5");
            }

            return inviteCode;
        }
    }
}
