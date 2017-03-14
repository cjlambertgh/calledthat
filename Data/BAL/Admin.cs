using Data.Models;
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

        public void Initialise()
        {
            using (var db = new DataContext())
            {
                if (!db.Competitions.Any())
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
                        db.Seasons.Add(season);

                        var competition = new Competition
                        {
                            Season = season,
                            Name = prem.Caption
                        };
                        db.Competitions.Add(competition);

                        db.SaveChanges();

                        var teamApi = new TeamAPI(prem.Id);
                        var teams = teamApi.Get();
                        teams.ForEach(t =>
                        {
                            db.Teams.Add(new Team
                            {
                                Name = t.Name,
                                BadgeUrl = t.BadgeUrl,
                                Competition = competition
                            });
                        });

                    }
                }
                db.SaveChanges();
            }
        }
    }
}
