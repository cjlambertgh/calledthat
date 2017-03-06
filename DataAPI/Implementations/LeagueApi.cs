using DataAPI.Interfaces;
using DataAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Implementations
{
    public class LeagueApi : IApi<League>
    {
        private static readonly string Uri = "http://api.football-data.org/v1/competitions/{0}/leagueTable";
        private readonly int _competitionId;

        public LeagueApi(int competitionId)
        {
            _competitionId = competitionId;
        }

        public List<League> Get()
        {
            var api = new RestApi(string.Format(Uri, _competitionId));
            var data = api.Get();
            var list = JsonConvert.DeserializeObject<League>(data);
            return new List<League>() { list };
        }

    }
}
