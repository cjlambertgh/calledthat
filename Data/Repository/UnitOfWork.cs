using Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private bool disposed;
        private DbContextTransaction transaction;

        private DataContext context = new DataContext();

        public List<T> SqlQuery<T>(string query, params object[] param)
        {
            return context.SqlQuery<T>(query, param);
        }

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

        private Repository<Pick> _picks;
        public Repository<Pick> Picks => _picks ?? (_picks = new Repository<Pick>(context));

        private Repository<PickResult> _pickResults;
        public Repository<PickResult> PickResults => _pickResults ?? (_pickResults = new Repository<PickResult>(context));

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

        public void BeginTransaction()
        {
            if(transaction != null)
            {
                throw new Exception("Trying to create transaction on context with an existing transation");
            }
            transaction = context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
            }
            transaction = null;
        }

        public void RollbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }
            transaction = null;
        }
    }
}
