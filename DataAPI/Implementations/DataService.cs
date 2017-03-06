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
            var l = api.Get();
            return l;
        }

        public IList<League> GetLeagues(int competitionId)
        {
            var api = new LeagueApi(competitionId);
            return api.Get();
        }

        public IList<Team> GetTeams(string LeagueName)
        {
            var teamApi = new TeamAPI();
            var teams = teamApi.Get(LeagueName);
            return teams;
        }

        public IList<Team> GetTeams(int LeagueId)
        {
            var teamApi = new TeamAPI();
            var teams = teamApi.Get(LeagueId);
            return teams;
        }
    }
}
