using DataAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataAPI.Models;

namespace DataAPI.Implementations
{
    public class DataService: IDataService
    {

        public IList<Competition> GetCompetition()
        {
            var api = new CompetitionAPI();
            var data = api.Get();
            return data;
        }

        public IList<League> GetLeagues(int competitionId)
        {
            var api = new LeagueApi(competitionId);
            var data = api.Get();
            return data;
        }

        public IList<Fixture> GetMatchdayFixtures(int competitionId, int matchDay)
        {
            var api = new MatchdayFixtureApi(competitionId, matchDay);
            var data = api.Get();
            return data;
        }

        public IList<Team> GetTeams(int competitionId)
        {
            var api = new TeamAPI(competitionId);
            var data = api.Get();
            return data;
        }
    }
}
