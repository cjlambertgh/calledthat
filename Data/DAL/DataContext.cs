using Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Data.DAL.Identity;
using Data.Interfaces;
using System.Linq;
using System.Data.Entity.Validation;
using System.Data.SqlClient;

namespace Data
{
    public class DataContext : IdentityDbContext<AppUser>, IDataContext
    {
        public DataContext() : base("DataContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Data.Migrations.Configuration>());
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            //add unique constraints
            modelBuilder.Entity<PickResult>().HasIndex(i => i.PickId).IsUnique();
            modelBuilder.Entity<Result>().HasIndex(i => i.FixtureId).IsUnique();
            modelBuilder.Entity<Fixture>().HasIndex(i => new { i.GameWeekId, i.HomeTeamId, i.AwayTeamId }).IsUnique();
            modelBuilder.Entity<GameWeek>().HasIndex(i => new { i.CompetitionId, i.Number }).IsUnique();
        }

        public override int SaveChanges()
        {
            ChangeTracker.Entries<IModel>().Where(x => x.State == EntityState.Added).ToList().ForEach(item =>
            {
                if (item.Entity.Id == null || item.Entity.Id == Guid.Empty)
                {
                    item.Entity.Id = Guid.NewGuid();
                }                
            });

            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var res = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    res += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        res += string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                var r = res;
                throw;
            }
        }

        public List<T> SqlQuery<T>(string query, params object[] param)
        {
            List<T> result = Database.SqlQuery<T>(query, param).ToList();
            return result;
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<GameWeek> GameWeeks { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<LeagueOwners> LeagueOwners { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerLeagues> PlayerLeagues { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Pick> Picks { get; set; }
        public DbSet<PickResult> PickResults { get; set; }
    }
}
