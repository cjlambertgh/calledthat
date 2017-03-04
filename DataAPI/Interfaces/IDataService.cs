using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Interfaces
{
    public interface IDataService
    {
        IList<Models.Competition> GetCompetition();
        IList<Models.Team> GetTeams(int LeagueId);
        IList<Models.Team> GetTeams(string LeagueName);
    }
}
