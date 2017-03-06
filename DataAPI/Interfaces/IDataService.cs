using DataAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Interfaces
{
    public interface IDataService
    {
        IList<Competition> GetCompetition();
        IList<Team> GetTeams(int LeagueId);
        IList<Team> GetTeams(string LeagueName);
        IList<League> GetLeagues(int competitionId);
    }
}
