using Data.Interfaces;
using Data.Models;
using Data.Models.Procs;
using Data.Repository;
using DataAPI.Helpers;
using DataAPI.Implementations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace GameServices
{
    public class GameService : IGameService
    {
        private readonly string Competition = "PL";
        private readonly IUnitOfWork _db;
        private readonly IReminderService _reminderService;

        public GameService(IDataContextConnection db, IReminderService reminderService)
        {
            _db = db.Database;
            _reminderService = reminderService;
        }

        public void Initialise()
        {
            if (!_db.Competitions.Any())
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
                    _db.Seasons.Add(season);

                    var competition = new Competition
                    {
                        Season = season,
                        Name = prem.Caption,
                        CurrentGameWeekNumber = 0,
                        LeagueApiLink = Competition
                    };
                    _db.Competitions.Add(competition);

                    _db.SaveChanges();

                    var teamApi = new TeamAPI(prem.Id);
                    var teams = teamApi.Get();
                    teams.ForEach(t =>
                    {
                        _db.Teams.Add(new Team
                        {
                            Name = t.Name,
                            BadgeUrl = t.BadgeUrl,
                            Competition = competition
                        });
                    });

                }

                _db.SaveChanges();
            }

            
        }

        /// <summary>
        /// Update score by adding PickResult entries for every pick that doesn't have a result.
        /// </summary>
        public void UpdateResults()
        {
            _db.BeginTransaction();
            try
            {
                var pickResults = _db.PickResults.All().Select(pr => pr.PickId);
                var picksToUpdate = _db.Picks.Where(p => !pickResults.Contains(p.Id)).ToList();
                var fixtureIds = picksToUpdate.Select(p => p.FixtureId).ToList();
                var results = _db.Results.Get(r => fixtureIds.Contains(r.FixtureId)).ToList();

                foreach (var pick in picksToUpdate)
                {
                    if (!_db.PickResults.Any(pr => pr.PickId == pick.Id))
                    {
                        var result = results.SingleOrDefault(r => r.FixtureId == pick.FixtureId);
                        if (result != null)
                        {
                            var score = CalulcateScore(result, pick);
                            _db.PickResults.Add(new PickResult
                            {
                                PickId = pick.Id,
                                Points = score
                            });
                        }
                    }
                }

                _db.SaveChanges();
                _db.CommitTransaction();
            }
            catch(Exception)
            {
                _db.RollbackTransaction();
                throw;
            }
        }

        private int CalulcateScore(Result result, Pick pick)
        {
            var score = 0;

            //correct pick
            if(result.AwayScore == pick.AwayScore && result.HomeScore == pick.HomeScore)
            {
                score = int.Parse(ConfigurationManager.AppSettings["Game.CorrectScorePoints"]);
            }
            //home win predicted
            else if(result.HomeScore > result.AwayScore && pick.HomeScore > pick.AwayScore )
            {
                score = int.Parse(ConfigurationManager.AppSettings["Game.CorrectResultPoints"]);
            }
            //away win predicted
            else if (result.HomeScore < result.AwayScore && pick.HomeScore < pick.AwayScore)
            {
                score = int.Parse(ConfigurationManager.AppSettings["Game.CorrectResultPoints"]);
            }
            //draw predicted
            else if (result.HomeScore == result.AwayScore && pick.HomeScore == pick.AwayScore)
            {
                score = int.Parse(ConfigurationManager.AppSettings["Game.CorrectResultPoints"]);
            }
            //predicted a win and ended draw
            else if (result.HomeScore == result.AwayScore && !(pick.HomeScore == pick.AwayScore))
            {
                score = int.Parse(ConfigurationManager.AppSettings["Game.IncorrectEndedInDraw"]);
            }
            //incorrect result
            else
            {
                //banker limits loss to zero
                if (pick.Banker)
                {
                    score = int.Parse(ConfigurationManager.AppSettings["Game.BankerScore"]);
                }
                else
                {
                    score = int.Parse(ConfigurationManager.AppSettings["Game.InCorrectResultPoints"]);
                }
            }

            if(pick.Double)
            {
                score *= int.Parse(ConfigurationManager.AppSettings["Game.DoubleBonusMultiplier"]);
            }

            return score;
        }

        public void UpdateApiData(string reminderEmailUrl)
        {
            var gameweekAdded = false;

            _db.BeginTransaction();
            try
            {
                var season = _db.Seasons.SingleOrDefault(s => (s.StartDate < DateTime.Now && s.EndDate >= DateTime.Now || s.CurrentSeasonYear == DateTime.Now.Year));
                var comp = season.Competitions.Single(c => c.LeagueApiLink == Competition);

                var compApi = new CompetitionAPI();
                var currentSeasonComp = compApi.Get().Single(c => c.Caption == comp.Name);

                UpdateExistingFixtureResults(currentSeasonComp.Id);                

                comp.CurrentGameWeekNumber = currentSeasonComp.CurrentMatchDay;
                if (!comp.GameWeeks.Any(gw => gw.Number == currentSeasonComp.CurrentMatchDay))
                {
                    comp.GameWeeks.Add(new GameWeek
                    {
                        Number = currentSeasonComp.CurrentMatchDay,
                        Competition = comp
                    });

                    gameweekAdded = true;
                }

                var gameWeek = comp.GameWeeks.First(gw => gw.Number == currentSeasonComp.CurrentMatchDay);

                var fixtureApi = new MatchdayFixtureApi(currentSeasonComp.Id, currentSeasonComp.CurrentMatchDay);
                var fixtures = fixtureApi.Get().Where(f => f.MatchDay == currentSeasonComp.CurrentMatchDay);

                foreach (var fix in fixtures)
                {
                    if (gameWeek.Fixtures == null || !gameWeek.Fixtures.Any(gw => gw?.HomeTeam?.Name == fix.HomeTeamName && gw?.AwayTeam?.Name == fix.AwayTeamName))
                    {
                        gameWeek.Fixtures.Add(new Fixture
                        {
                            HomeTeam = _db.Teams.FirstOrDefault(t => t.Name == fix.HomeTeamName),
                            AwayTeam = _db.Teams.FirstOrDefault(t => t.Name == fix.AwayTeamName),
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

                            //_db.SaveChanges();
                        }
                    }
                }

                if (gameweekAdded)
                {
                    gameWeek.PickOpenDateTime = GetPreviousGameweekCloseDateTime(comp, gameWeek);
                    gameWeek.PickCloseDateTime = gameWeek.Fixtures.Min(f => f.KickOffDateTime).AddMinutes(-15);
                }

                _db.SaveChanges();
                _db.CommitTransaction();
            }
            catch(Exception)
            {
                _db.RollbackTransaction();
                throw;
            }

            if(gameweekAdded)
            {
                _reminderService.SendNewGameweekReminder(reminderEmailUrl);
            }
                     
        }

        private void UpdateExistingFixtureResults(int apiCompetitionId)
        {
            var resultIds = _db.Results.All().Select(res => res.FixtureId);
            var fixturesToUpdate = _db.Fixtures.Where(fix => !resultIds.Contains(fix.Id)).ToList();
            if(fixturesToUpdate.Any())
            {
                var gameWeeksRequired = fixturesToUpdate.GroupBy(fix => fix.GameWeekId).Select(g => g.First()).Select(f => f.GameWeek.Number);
                foreach(var gameWeekNumber in gameWeeksRequired)
                {
                    var fixtureApi = new MatchdayFixtureApi(apiCompetitionId, gameWeekNumber);
                    var apiFixtures = fixtureApi.Get();
                    foreach(var fixture in fixturesToUpdate)
                    {
                        var apiFixture = apiFixtures.SingleOrDefault(api => fixture.HomeTeam.Name == api.HomeTeamName && fixture.AwayTeam.Name == api.AwayTeamName);
                        if(apiFixture != null && FixtureHelper.IsFixtureInFinished(apiFixture))
                        {
                            if (apiFixture.Result != null
                                && apiFixture.Result.GoalsHomeTeam != null
                                && apiFixture.Result.GoalsAwayTeam != null)
                                _db.Results.Add(new Result
                                {
                                    FixtureId = fixture.Id,
                                    HomeScore = (int)apiFixture.Result.GoalsHomeTeam,
                                    AwayScore = (int)apiFixture.Result.GoalsAwayTeam
                                });
                        }
                    }
                }

                _db.SaveChanges();
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

        public IEnumerable<Fixture> GetGameWeekFixtures()
        {
            var comp = _db.Competitions.SingleOrDefault(c => c.LeagueApiLink == Competition);
            var gameWeek = comp.GameWeeks.Single(gw => gw.Number == comp.CurrentGameWeekNumber);
            return gameWeek.Fixtures.ToList();
        }

        public IEnumerable<Pick> GetPlayerPicks(Guid playerId, int gameweek)
        {
            if (playerId == null || playerId == Guid.Empty) throw new ArgumentNullException(nameof(playerId));

            return _db.Picks.Get(pp => pp.PlayerId == playerId && pp.Fixture.GameWeek.Number == gameweek).ToList();
        }

        public void AddPick(Guid playerId, Guid fixtureId, int homeScore, int awayScore, bool banker, bool doubleScore)
        {
            if (playerId == null) throw new ArgumentNullException(nameof(playerId));
            if (fixtureId == null) throw new ArgumentNullException(nameof(fixtureId));

            if(!_db.Players.Any(p => p.Id == playerId))
            {
                _db.Players.Add(new Player
                {
                    Id = playerId
                });
            }

            var existing = _db.Picks.FirstOrDefault(p => p.FixtureId == fixtureId && p.PlayerId == playerId);
            if (existing == null)
            {
                var pick = new Pick
                {
                    FixtureId = fixtureId,
                    PlayerId = playerId,
                    HomeScore = homeScore,
                    AwayScore = awayScore,
                    Banker = banker,
                    Double = doubleScore
                };

                _db.Picks.Add(pick);
            }
            else
            {
                existing.HomeScore = homeScore;
                existing.AwayScore = awayScore;
                existing.Banker = banker;
                existing.Double = doubleScore;
            }
            _db.SaveChanges();
        }

        public int GetCurrentGameweek()
        {
            return _db.Competitions.FirstOrDefault()?.CurrentGameWeekNumber ?? 1;
        }

        public bool IsGameweekOpen(int gameweekNumber)
        {
            var gameWeek = _db.GameWeeks.SingleOrDefault(gw => gw.Number == gameweekNumber);
            return IsGameweekOpen(gameWeek);
        }

        public bool IsCurrentGameweekOpen()
        {
            var gameWeekNumber = GetCurrentGameweek();
            return IsGameweekOpen(gameWeekNumber);
        }

        private static bool IsGameweekOpen(GameWeek gameWeek)
        {
            return (DateTime.Now > gameWeek.PickOpenDateTime && DateTime.Now < gameWeek.PickCloseDateTime);
        }

        public void PopulatePickOpenCloseDates(out DateTime openDate, out DateTime closeDate)
        {
            var currentGameweek = GetCurrentGameweek();
            var gameweek = _db.GameWeeks.FirstOrDefault(gw => gw.Number == currentGameweek);
            openDate = gameweek.PickOpenDateTime;
            closeDate = gameweek.PickCloseDateTime;
        }

        public IEnumerable<PlayerResults> GetPlayerResults(Guid playerId, int? gameWeek = default(int?))
        {
            var res = _db.SqlQuery<PlayerResults>("uspGetPlayerResults @playerId", new SqlParameter("@playerId", playerId));
            if(gameWeek != null)
            {
                return res.Where(item => item.GameweekNumber == gameWeek);
            }
            return res;
        }

        private IReadOnlyCollection<Player> GetPlayersWithoutGameweekPredictions()
        {

            return null;
        }
    }
}
