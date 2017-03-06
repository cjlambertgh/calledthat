﻿using DataAPI.Interfaces;
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
        private int _competitionId;

        public LeagueApi(int competitionId)
        {
            _competitionId = competitionId;
        }

        public List<League> Get()
        {
            var api = new RestApi(string.Format(Uri, _competitionId));
            var data = api.Get();
            var list = JsonConvert.DeserializeObject<List<League>>(data);
            return list;
        }

        public List<League> Get(string name)
        {
            throw new NotImplementedException();
        }

        public List<League> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
