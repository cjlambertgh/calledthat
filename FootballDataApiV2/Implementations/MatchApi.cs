using FootballDataApiV2.Interfaces;
using FootballDataApiV2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;

namespace FootballDataApiV2.Implementations
{
    public class MatchApi : BaseApi, IMatchApi
    {
        public int MatchDay { get; set; }
        public int CompetitionId { get; set; }
        private const string endpoint = "/v2/competitions/{competitionid}/matches?matchday={matchday}";
        //private const string endpoint = "/v2/competitions/2021/matches?matchday=1";
        public IEnumerable<Match> Get()
        {
            if (MatchDay == 0) throw new ArgumentOutOfRangeException(nameof(MatchDay));
            if (CompetitionId == 0) throw new ArgumentOutOfRangeException(nameof(CompetitionId));

            var request = new RestRequest(endpoint);
            request.AddHeader("X-Auth-Token", "a969a2868cd54854bdbce65c40eb95f0");
            request.AddUrlSegment("competitionid", CompetitionId.ToString());
            request.AddUrlSegment("matchday", MatchDay.ToString());

            var response = FootballDataApiClient.Execute(request);
            var content = response.Content;
            var responseData = JsonConvert.DeserializeObject<JObject>(content);
            var comps = responseData["matches"].ToString();
            var data = JsonConvert.DeserializeObject<List<Match>>(comps);
            return data;
        }
    }
}
