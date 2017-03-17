using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class UnitOfWork : IDisposable
    {
        private bool disposed;

        private DataContext context = new DataContext();

        private Repository<Competition> _competitions;
        public Repository<Competition> Competitions => _competitions ?? (_competitions = new Repository<Competition>(context));

        private Repository<Team> _teams;
        public Repository<Team> Teams => _teams ?? (_teams = new Repository<Team>(context));

        private Repository<Season> _seasons;
        public Repository<Season> Seasons => _seasons ?? (_seasons = new Repository<Season>(context));

        private Repository<Fixture> _fixtures;
        public Repository<Fixture> Fixtures => _fixtures ?? (_fixtures = new Repository<Fixture>(context));

        private Repository<GameWeek> _gameWeeks;
        public Repository<GameWeek> GameWeeks => _gameWeeks ?? (_gameWeeks = new Repository<GameWeek>(context));

        private Repository<League> _leagues;
        public Repository<League> Leagues => _leagues ?? (_leagues = new Repository<League>(context));

        private Repository<LeagueOwners> _leagueOwners;
        public Repository<LeagueOwners> LeagueOwners => _leagueOwners ?? (_leagueOwners = new Repository<LeagueOwners>(context));

        private Repository<Player> _players;
        public Repository<Player> Players => _players ?? (_players = new Repository<Player>(context));

        private Repository<PlayerLeagues> _playerLeagues;
        public Repository<PlayerLeagues> PlayerLeagues => _playerLeagues ?? (_playerLeagues = new Repository<PlayerLeagues>(context));

        private Repository<Result> _results;
        public Repository<Result> Results => _results ?? (_results = new Repository<Result>(context));

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
