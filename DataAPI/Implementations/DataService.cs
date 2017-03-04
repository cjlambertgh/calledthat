using DataAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Implementations
{
    public class DataService: IDataService
    {

        private readonly string competition = "http://api.football-data.org/v1/competitions/?season=2015";

        public string GetCompetition()
        {
            var comp = new CompetitionAPI();
            var l = comp.GetCompetitions();
            return "";
        }

        public async Task<string> GetCompetitionAsync()
        {
            using (var wc = new HttpClient())
            {
                var response = await wc.GetAsync(competition);
                if(response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return data;
                }
                else
                {
                    return "";
                }
            }
        }

    }
}
