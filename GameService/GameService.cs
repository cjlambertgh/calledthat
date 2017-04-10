﻿using Data.Interfaces;
using Data.Models;
using DataAPI.Helpers;
using DataAPI.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    public class GameService : IGameService
    {
        private readonly string Competition = "PL";
        private readonly IDataContextConnection _db;

        public GameService(IDataContextConnection db)
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
                        EndDate = new DateTime(year + 1, 7, 31)
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

                _db.Database.SaveChanges();
            }

            
        }
        public void UpdateAll()
        {
            UpdateApiData();
            UpdateDatabase();
        }

        private void UpdateApiData()
        {
            using (var db = _db.Database)
            {
                var season = db.Seasons.SingleOrDefault(s => s.StartDate < DateTime.Now && s.EndDate >= DateTime.Now);
                var comp = season.Competitions.Single();

                var compApi = new CompetitionAPI();
                var currentSeasonComp = compApi.Get().Single(c => c.Caption == comp.Name);
                if (!comp.GameWeeks.Any(gw => gw.Number == currentSeasonComp.CurrentMatchDay))
                {
                    comp.GameWeeks.Add(new GameWeek
                    {
                        Number = currentSeasonComp.CurrentMatchDay,
                        Competition = comp
                    });
                }

                var gameWeek = comp.GameWeeks.First(gw => gw.Number == currentSeasonComp.CurrentMatchDay);

                var fixtureApi = new MatchdayFixtureApi(currentSeasonComp.Id, currentSeasonComp.CurrentMatchDay);
                var fixtures = fixtureApi.Get().Where(f => f.MatchDay == currentSeasonComp.CurrentMatchDay);

                foreach (var fix in fixtures)
                {
                    if (gameWeek.Fixtures == null || !gameWeek.Fixtures.Any(gw => gw.HomeTeam.Name == fix.HomeTeamName && gw.AwayTeam.Name == fix.AwayTeamName))
                    {
                        gameWeek.Fixtures.Add(new Fixture
                        {
                            HomeTeam = db.Teams.FirstOrDefault(t => t.Name == fix.HomeTeamName),
                            AwayTeam = db.Teams.FirstOrDefault(t => t.Name == fix.AwayTeamName),
                            KickOffDateTime = DateTime.Parse(fix.Date)
                        });
                    }

                    var gameweekFixture = gameWeek.Fixtures.Single(gw => gw.HomeTeam.Name == fix.HomeTeamName && gw.AwayTeam.Name == fix.AwayTeamName);

                    if (FixtureHelper.IsFixtureInFinished(fix))
                    {
                        if (!gameweekFixture.Results.Any())
                        {
                            if (fix.Result != null && fix.Result.GoalsAwayTeam != null && fix.Result.GoalsHomeTeam != null)
                            {
                                var result = new Result
                                {
                                    HomeScore = (int)(fix.Result.GoalsHomeTeam),
                                    AwayScore = (int)(fix.Result.GoalsAwayTeam)
                                };

                                gameweekFixture.Results.Add(result);
                            }
                            else
                            {
                                //TODO: completed fixture but result or scores null!?
                            }
                        }
                    }
                }

                if(gameWeek.PickOpenDateTime == DateTime.MinValue || gameWeek.PickCloseDateTime == DateTime.MinValue)
                {
                    gameWeek.PickOpenDateTime = GetPreviousGameweekCloseDateTime(comp, gameWeek);
                    gameWeek.PickCloseDateTime = gameWeek.Fixtures.Min(f => f.KickOffDateTime).AddMinutes(-15);
                }

                db.SaveChanges();
            }          
        }

        private static DateTime GetPreviousGameweekCloseDateTime(Competition comp, GameWeek currentGameWeek)
        {
            var previousGameWeek = comp.GameWeeks.FirstOrDefault(gw => gw.Number == currentGameWeek.Number - 1);
            if(previousGameWeek == null)
            {
                return DateTime.Now;
            }

            return previousGameWeek.Fixtures.Max(f => f.KickOffDateTime).AddMinutes(15);
        }

        private static DateTime GetCurrentGameweekCloseDateTime(GameWeek currentGameWeek)
        {
            return currentGameWeek.Fixtures.Min(f => f.KickOffDateTime).AddMinutes(-15);
        }

        private void UpdateDatabase()
        {

        }
    }
}