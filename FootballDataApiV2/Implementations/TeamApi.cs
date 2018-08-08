using FootballDataApiV2.Interfaces;
using FootballDataApiV2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System;

namespace FootballDataApiV2.Implementations
{
    public class TeamApi : BaseApi, ITeamApi
    {
        public int CompetitionId { get; set; }
        public IEnumerable<Team> Get()
        {
            if (CompetitionId == 0) throw new ArgumentNullException(nameof(CompetitionId));

            var request = new RestRequest("v2/competitions/{id}/teams");
            request.AddHeader("X-Auth-Token", "a969a2868cd54854bdbce65c40eb95f0");
            request.AddUrlSegment("id", CompetitionId.ToString());

            var response = FootballDataApiClient.Execute(request);
            var content = response.Content;
            var responseData = JsonConvert.DeserializeObject<JObject>(content);
            var comps = responseData["teams"].ToString();
            var data = JsonConvert.DeserializeObject<List<Team>>(comps);
            return data;
        }
    }
}
