
using System.Collections.Generic;
using Newtonsoft.Json;
using DataAPI.Models;
using DataAPI.Interfaces;
using System;

namespace DataAPI.Implementations
{
    public class CompetitionAPI : IApi<Competition>
    {
        private static readonly string Uri = "http://api.football-data.org/v1/competitions";

        public List<Competition> Get()
        {
            var api = new RestApi(Uri);
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
