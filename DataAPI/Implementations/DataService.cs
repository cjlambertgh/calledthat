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
            var comp = new CompetitionAPI();
            var l = comp.Get();
            return l;
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
