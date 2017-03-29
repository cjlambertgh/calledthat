using Data.Interfaces;
using Data.Models;
using Data.Repository;
using DataAPI.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BAL
{
    public class Admin
    {
        private readonly string Competition = "PL";
        private readonly IDataContextConnection _db;

        public Admin(IDataContextConnection db)
        {
            _db = db;
        }

        public void Initialise()
        {
            if (!_db.Database.Competitions.Any())
            {
                var api = new CompetitionAPI();
                var comps = api.Get();
                var prem = comps.Single(c => c.League == Competition);
                if (prem != null)
                {
                    var year = int.Parse(prem.Year);
                    var season = new Season
                    {
                        CurrentSeasonYear = year,
                        StartDate = new DateTime(year, 8, 1),
                        EndDate = new DateTime(year+1, 7, 31)
                    };
                    _db.Database.Seasons.Add(season);

                    var competition = new Competition
                    {
                        Season = season,
                        Name = prem.Caption
                    };
                    _db.Database.Competitions.Add(competition);

                    _db.Database.SaveChanges();

                    var teamApi = new TeamAPI(prem.Id);
                    var teams = teamApi.Get();
                    teams.ForEach(t =>
                    {
                        _db.Database.Teams.Add(new Team
                        {
                            Name = t.Name,
                            BadgeUrl = t.BadgeUrl,
                            Competition = competition
                        });
                    });

                }
            }

            _db.Database.SaveChanges();
        }

        public void Update()
        {
            var season = _db.Database.Seasons.SingleOrDefault(s => s.StartDate > DateTime.Now && s.EndDate <= DateTime.Now);
            var comp = season.Competitions.Single();

            var compApi = new CompetitionAPI();
            var currentSeasonComp = compApi.Get().Single(c => c.Caption == comp.Name);
            if(!comp.GameWeeks.Any(gw => gw.Number == currentSeasonComp.CurrentMatchDay))
            {
                comp.GameWeeks.Add(new GameWeek
                {
                    Number = currentSeasonComp.CurrentMatchDay,
                    Competition = comp
                });
            }

            var gameWeek = comp.GameWeeks.First(gw => gw.Number == currentSeasonComp.CurrentMatchDay);
            if(!gameWeek.Fixtures.Any())
            {
                var fixtureApi = new MatchdayFixtureApi(currentSeasonComp.Id, currentSeasonComp.CurrentMatchDay);
                var fixtures = fixtureApi.Get();
                foreach(var fix in fixtures)
                {
                    gameWeek.Fixtures.Add(new Fixture
                    {
                        HomeTeam = _db.Database.Teams.FirstOrDefault(t => t.Name == fix.HomeTeamName),
                        AwayTeam = _db.Database.Teams.FirstOrDefault(t => t.Name == fix.AwayTeamName),
                    });
                }

            }

            
        }
        

        //public void Test()
        //{
        //    using (var db = new UnitOfWork())
        //    {
        //        if(!db.Competitions.Any())
        //        {
        //            var api = new CompetitionAPI();
        //            var comps = api.Get();
        //            var prem = comps.Single(c => c.League == Competition);
        //            if (prem != null)
        //            {
        //                var year = int.Parse(prem.Year);
        //                var season = new Season
        //                {
        //                    CurrentSeasonYear = year,
        //                    StartDate = new DateTime(year, 8, 1),
        //                    EndDate = new DateTime(year + 1, 7, 31)
        //                };
        //                db.Seasons.Add(season);

        //                var competition = new Competition
        //                {
        //                    Season = season,
        //                    Name = prem.Caption
        //                };
        //                db.Competitions.Add(competition);

        //                db.SaveChanges();

        //                var teamApi = new TeamAPI(prem.Id);
        //                var teams = teamApi.Get();
        //                teams.ForEach(t =>
        //                {
        //                    db.Teams.Add(new Team
        //                    {
        //                        Name = t.Name,
        //                        BadgeUrl = t.BadgeUrl,
        //                        Competition = competition
        //                    });
        //                });

        //            }
        //        }
        //        db.SaveChanges();
        //    }
        //}
    }
}
