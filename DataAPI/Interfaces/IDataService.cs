using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Interfaces
{
    public interface IDataService
    {
        Task<string> GetCompetitionAsync();
        string GetCompetition();
    }
}
