
using System.Collections.Generic;
using Newtonsoft.Json;
using DataAPI.Models;
using DataAPI.Interfaces;
using System;

namespace DataAPI.Implementations
{
    public class CompetitionAPI : IApi<Competition>
    {
        private static readonly string Uri = "http://api.football-data.org/v1/competitions/?season={0}";
        private static readonly string SeasonYear = "2016";

        public List<Competition> Get()
        {
            var uri = string.Format(Uri, SeasonYear);
            var api = new RestApi(uri);
            var data = api.Get();
            var list = JsonConvert.DeserializeObject<List<Competition>>(data);
            return list;
        }

        public List<Competition> Get(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Competition> Get(string name)
        {
            throw new NotImplementedException();
        }
    }
}
