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
    public class MatchdayFixtureApi : IApi<Fixture>
    {
        private static readonly string Uri = "http://api.football-data.org/v1/competitions/426/fixtures";
        private static readonly string FilterMatchday = "matchday={0}";

        private readonly int _competitionId;
        private readonly int _matchDay;

        public MatchdayFixtureApi(int competitionId, int matchDay)
        {
            _competitionId = competitionId;
            _matchDay = matchDay;
        }

        public List<Fixture> Get()
        {
            var uri = $"{Uri}?{string.Format(FilterMatchday, _matchDay) }";
            var api = new RestApi(string.Format(Uri, _competitionId));
            var data = api.Get();
            var wrapper = JsonConvert.DeserializeObject<FixtureWrapper>(data);
            return wrapper.Fixtures;
        }

    }
}
