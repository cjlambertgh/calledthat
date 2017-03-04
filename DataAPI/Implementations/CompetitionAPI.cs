
using System.Collections.Generic;
using Newtonsoft.Json;
using DataAPI.Models;

namespace DataAPI.Implementations
{
    public class CompetitionAPI
    {
        private static readonly string Uri = "http://api.football-data.org/v1/competitions/?season={0}";
        private static readonly string SeasonYear = "2016";

        public IList<Competition> GetCompetitions()
        {
            var uri = string.Format(Uri, SeasonYear);
            var api = new RestApi(uri);
            var data = api.Get();
            var list = JsonConvert.DeserializeObject<List<Competition>>(data);
            return list;
        }
    }
}
