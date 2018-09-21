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
        private readonly FootballDataApiV2.Interfaces.ICompetitionApi _competitionApi;
        private readonly FootballDataApiV2.Interfaces.ITeamApi _teamApi;
        private readonly FootballDataApiV2.Interfaces.IMatchApi _matchApi;

        public GameService(IDataContextConnection db, IReminderService reminderService, 
            FootballDataApiV2.Interfaces.ICompetitionApi competitionApi,
            FootballDataApiV2.Interfaces.ITeamApi teamApi,
            FootballDataApiV2.Interfaces.IMatchApi matchApi)
        {
            _db = db.Database;
            _reminderService = reminderService;
            _competitionApi = competitionApi;
            _teamApi = teamApi;
            _matchApi = matchApi;
        }

        public Season CurrentSeason => _db.Seasons.SingleOrDefault(s => (s.StartDate < DateTime.Now && s.EndDate >= DateTime.Now || s.CurrentSeasonYear == DateTime.Now.Year));

        public void Initialise()
        {
            try
            {
                var season = _db.Seasons.SingleOrDefault(s => (s.StartDate < DateTime.Now && s.EndDate >= DateTime.Now || s.CurrentSeasonYear == DateTime.Now.Year));
                var year = DateTime.Now.Year;
                if (season == null)
                {
                    season = new Season
                    {
                        CurrentSeasonYear = year,
                        StartDate = new DateTime(year, 8, 1),
                        EndDate = new DateTime(year + 1, 7, 31)
                    };
                    _db.Seasons.Add(season);
                    _db.SaveChanges();
                }
                if (!_db.Competitions.Any(c => c.Season.CurrentSeasonYear == year && c.Name == "Premier League"))
                {
                    var comps = _competitionApi.Get();
                    var prem = comps.Single(c => c.Area.Name == "England" && c.Name == "Premier League");
                    if (prem != null)
                    {
                        _db.BeginTransaction();
                        var competition = new Competition
                        {
                            Season = season,
                            Name = prem.Name,
                            CurrentGameWeekNumber = 0,
                            LeagueApiLink = prem.Id.ToString()
                        };
                        _db.Competitions.Add(competition);

                        _db.SaveChanges();

                        _teamApi.CompetitionId = Int32.Parse(competition.LeagueApiLink);
                        var teams = _teamApi.Get();
                        
                        teams.ToList().ForEach(t =>
                        {
                            _db.Teams.Add(new Team
                            {
                                Name = t.Name,
                                BadgeUrl = null,
                                Competition = competition
                            });
                        });

                    }

                    _db.SaveChanges();
                    _db.CommitTransaction();
                }
            }
            catch(Exception)
            {
                _db.RollbackTransaction();
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
            var gameweekOpen = false;

            _db.BeginTransaction();
            try
            {
                var season = _db.Seasons.SingleOrDefault(s => (s.StartDate < DateTime.Now && s.EndDate >= DateTime.Now || s.CurrentSeasonYear == DateTime.Now.Year));
                foreach(var comp in season.Competitions)
                {
                    var apiCompetition = _competitionApi.Get().SingleOrDefault(c => c.Id.ToString() == comp.LeagueApiLink);
                    _matchApi.CompetitionId = apiCompetition.Id;

                    UpdateExistingFixtureResults(apiCompetition.Id);

                    comp.CurrentGameWeekNumber = apiCompetition.CurrentSeason.CurrentMatchday ?? 1;
                    _matchApi.MatchDay = comp.CurrentGameWeekNumber;
                    gameweekAdded = AddGameweekIfNeeded(comp, comp.CurrentGameWeekNumber);
                    var gameWeek = comp.GameWeeks.First(gw => gw.Number == comp.CurrentGameWeekNumber);

                    AddRequiredFixtures(gameWeek);

                    if (gameweekAdded)
                    {
                        gameWeek.PickOpenDateTime = GetPreviousGameweekCloseDateTime(comp, gameWeek);
                        gameWeek.PickCloseDateTime = gameWeek.Fixtures.Min(f => f.KickOffDateTime).AddMinutes(-15);
                    }

                    gameweekOpen = IsGameweekOpen(gameWeek);

                    _db.SaveChanges();
                }

                //var compApi = new CompetitionAPI();
                //var currentSeasonComp = compApi.Get().Single(c => c.Caption == comp.Name);

                //UpdateExistingFixtureResults(currentSeasonComp.Id);                

                //comp.CurrentGameWeekNumber = currentSeasonComp.CurrentMatchDay;
                //if (!comp.GameWeeks.Any(gw => gw.Number == currentSeasonComp.CurrentMatchDay))
                //{
                //    comp.GameWeeks.Add(new GameWeek
                //    {
                //        Number = currentSeasonComp.CurrentMatchDay,
                //        Competition = comp
                //    });

                //    gameweekAdded = true;
                //}

                //var gameWeek = comp.GameWeeks.First(gw => gw.Number == currentSeasonComp.CurrentMatchDay);
                
                //var fixtureApi = new MatchdayFixtureApi(currentSeasonComp.Id, currentSeasonComp.CurrentMatchDay);
                //var fixtures = fixtureApi.Get().Where(f => f.MatchDay == currentSeasonComp.CurrentMatchDay);

                //foreach (var fix in fixtures)
                //{
                //    if (gameWeek.Fixtures == null || !gameWeek.Fixtures.Any(gw => gw?.HomeTeam?.Name == fix.HomeTeamName && gw?.AwayTeam?.Name == fix.AwayTeamName))
                //    {
                //        gameWeek.Fixtures.Add(new Fixture
                //        {
                //            HomeTeam = _db.Teams.FirstOrDefault(t => t.Name == fix.HomeTeamName),
                //            AwayTeam = _db.Teams.FirstOrDefault(t => t.Name == fix.AwayTeamName),
                //            KickOffDateTime = DateTime.Parse(fix.Date)
                //        });
                //    }

                //    var gameweekFixture = gameWeek.Fixtures.Single(gw => gw.HomeTeam.Name == fix.HomeTeamName && gw.AwayTeam.Name == fix.AwayTeamName);

                //    if (FixtureHelper.IsFixtureInFinished(fix))
                //    {
                //        if (!gameweekFixture.Results.Any())
                //        {
                //            if (fix.Result != null && fix.Result.GoalsAwayTeam != null && fix.Result.GoalsHomeTeam != null)
                //            {
                //                var result = new Result
                //                {
                //                    HomeScore = (int)(fix.Result.GoalsHomeTeam),
                //                    AwayScore = (int)(fix.Result.GoalsAwayTeam)
                //                };

                //                gameweekFixture.Results.Add(result);
                //            }
                //            else
                //            {
                //                //TODO: completed fixture but result or scores null!?
                //            }

                //            //_db.SaveChanges();
                //        }
                //    }
                //}

                //if (gameweekAdded)
                //{
                //    gameWeek.PickOpenDateTime = GetPreviousGameweekCloseDateTime(comp, gameWeek);
                //    gameWeek.PickCloseDateTime = gameWeek.Fixtures.Min(f => f.KickOffDateTime).AddMinutes(-15);
                //}

                //gameweekOpen = IsGameweekOpen(gameWeek);

                //_db.SaveChanges();
                _db.CommitTransaction();
            }
            catch(Exception)
            {
                _db.RollbackTransaction();
                throw;
            }

            if(gameweekAdded)
            {
                //_reminderService.SendNewGameweekReminder(reminderEmailUrl);
            }

            if(gameweekOpen && !gameweekAdded)
            {
                //_reminderService.SendGameweekPicksNotEnteredReminder(reminderEmailUrl, GetPlayersEmailsWithGameweekPredictions());
            }
            
                     
        }

        private void AddRequiredFixtures(GameWeek gameWeek)
        {
            var fixtures = _matchApi.Get().Where(m => m.Matchday == gameWeek.Number);
            foreach (var fix in fixtures)
            {
                if (gameWeek.Fixtures == null 
                    || !gameWeek.Fixtures.Any(gw => gw?.HomeTeam?.Name == fix.HomeTeam.Name && gw?.AwayTeam?.Name == fix.AwayTeam.Name))
                {
                    gameWeek.Fixtures.Add(new Fixture
                    {
                        HomeTeam = gameWeek.Competition.Teams.FirstOrDefault(t => t.Name == fix.HomeTeam.Name),
                        AwayTeam = gameWeek.Competition.Teams.FirstOrDefault(t => t.Name == fix.AwayTeam.Name),
                        KickOffDateTime = fix.UtcDate
                    });
                }

                var gameweekFixture = gameWeek.Fixtures.Single(gw => gw.HomeTeam.Name == fix.HomeTeam.Name && gw.AwayTeam.Name == fix.AwayTeam.Name);

                if (fix.IsFixtureInFinished)
                {
                    if (!gameweekFixture.Results.Any())
                    {
                        if (fix.Score?.FullTime != null && fix.Score?.FullTime?.AwayTeam != null && fix.Score?.FullTime?.HomeTeam != null)
                        {
                            var result = new Result
                            {
                                HomeScore = (int)(fix.Score.FullTime.HomeTeam),
                                AwayScore = (int)(fix.Score.FullTime.AwayTeam)
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
        }

        private bool AddGameweekIfNeeded(Competition comp, int currentMatchday)
        {
            if (!comp.GameWeeks.Any(gw => gw.Number == currentMatchday))
            {
                comp.GameWeeks.Add(new GameWeek
                {
                    Number = currentMatchday,
                    Competition = comp
                });

                return true;
            }
            return false;
        }

        private void UpdateExistingFixtureResults(int apiCompetitionId)
        {
            var fixturesToUpdate = _db.Fixtures.Where(fi => !fi.Results.Any()).ToList();
            if(fixturesToUpdate.Any())
            {
                var gameWeeksRequired = fixturesToUpdate.GroupBy(fix => fix.GameWeekId).Select(g => g.First()).Select(f => f.GameWeek.Number);
                foreach(var gameWeekNumber in gameWeeksRequired)
                {
                    _matchApi.MatchDay = gameWeekNumber;
                    _matchApi.CompetitionId = apiCompetitionId;
                    var apiFixtures = _matchApi.Get();
                    foreach(var fixture in fixturesToUpdate)
                    {
                        var apiFixture = apiFixtures.SingleOrDefault(api => fixture.HomeTeam.Name == api.HomeTeam.Name && fixture.AwayTeam.Name == api.AwayTeam.Name);
                        if(apiFixture != null && apiFixture.IsFixtureInFinished)
                        {
                            if (apiFixture.Score?.FullTime != null
                                && apiFixture.Score.FullTime.HomeTeam != null
                                && apiFixture.Score.FullTime.AwayTeam != null)
                                _db.Results.Add(new Result
                                {
                                    FixtureId = fixture.Id,
                                    HomeScore = (int)apiFixture.Score.FullTime.HomeTeam,
                                    AwayScore = (int)apiFixture.Score.FullTime.AwayTeam
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
            var comp = CurrentSeason.Competitions.SingleOrDefault();
            var gameWeek = comp.GameWeeks.Single(gw => gw.Number == comp.CurrentGameWeekNumber);
            return gameWeek.Fixtures.ToList();
        }

        public IEnumerable<Pick> GetPlayerPicks(Guid playerId, int gameweek)
        {
            if (playerId == null || playerId == Guid.Empty) throw new ArgumentNullException(nameof(playerId));

            return _db.Picks.Get(pp => pp.PlayerId == playerId && pp.Fixture.GameWeek.Number == gameweek).ToList();
        }

        public IEnumerable<Pick> GetAllPlayerPicks(int gameweek)
        {
            return _db.Picks.Where(p => p.Fixture.GameWeek.Number == gameweek).ToList();
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
            return CurrentSeason.Competitions.FirstOrDefault()?.CurrentGameWeekNumber ?? 1;
        }

        public bool IsGameweekOpen(int gameweekNumber)
        {
            return IsGameweekOpen(GetCurrentGameweek());
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
            var res = _db.SqlQuery<PlayerResults>("uspGetPlayerResults @playerId, @seasonId", new SqlParameter("@playerId", playerId), new SqlParameter("@seasonId", CurrentSeason.Id));
            if(gameWeek != null)
            {
                return res.Where(item => item.GameweekNumber == gameWeek);
            }
            return res;
        }

        private IEnumerable<string> GetPlayersEmailsWithGameweekPredictions()
        {
            if (!IsCurrentGameweekOpen())
            {
                new List<string>();
            }

            var picks = GetAllPlayerPicks(GetCurrentGameweek());
            return picks.Select(p => p.Player.AppUser.Email).Distinct().ToList();
        }
    }
}
