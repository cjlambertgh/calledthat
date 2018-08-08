using FootballDataApiV2.Interfaces;
using FootballDataApiV2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace FootballDataApiV2.Implementations
{
    public class CompetitionApi : BaseApi, ICompetitionApi
    {
        public IEnumerable<Competition> Get()
        {
            var request = new RestRequest("v2/competitions");
            request.AddHeader("X-Auth-Token", "a969a2868cd54854bdbce65c40eb95f0");

            var response = FootballDataApiClient.Execute(request);
            var content = response.Content;
            var responseData = JsonConvert.DeserializeObject<JObject>(content);
            var comps = responseData["competitions"].ToString();
            var data = JsonConvert.DeserializeObject<List<Competition>>(comps);
            return data;
        }
    }
}
