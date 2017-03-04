using DataCollector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace DataCollector.Implementations
{
    public class DataService : IDataService
    {
        private readonly string competition = "http://api.football-data.org/v1/competitions/?season=2015";
        public object GetCompetition()
        {
            using (var wc = new HttpClient())
            {
                var response = wc.GetStringAsync(competition);
                return response;
            }
        }
    }
}
