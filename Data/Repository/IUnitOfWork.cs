using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IUnitOfWork
    {
        Repository<Team> Teams { get; }
        Repository<Season> Seasons { get; }
        Repository<Fixture> Fixtures { get; }
        Repository<GameWeek> GameWeeks { get; }
        Repository<League> Leagues { get; }
        Repository<LeagueOwners> LeagueOwners { get; }
        Repository<Player> Players { get; }
        Repository<PlayerLeagues> PlayerLeagues { get; }
        Repository<Result> Results { get; }
        Repository<Pick> Picks { get; }
        Repository<PickResult> PickResults { get; }
        Repository<Competition> Competitions { get; }

        List<T> SqlQuery<T>(string query, IEnumerable<object> param);

        void SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
