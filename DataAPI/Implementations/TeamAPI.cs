using DataAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAPI.Models;
using Newtonsoft.Json;

namespace DataAPI.Implementations
{
    public class TeamAPI : IApi<Team>
    {

        private static readonly string Uri = "http://api.football-data.org/v1/competitions/{0}/teams";
        public List<Team> Get()
        {
            throw new NotImplementedException();
        }

        public List<Team> Get(string name)
        {
            throw new NotImplementedException();
        }

        public List<Team> Get(int Id)
        {
            var uri = string.Format(Uri, Id);
            var api = new RestApi(uri);
            var data = api.Get();
            var list = JsonConvert.DeserializeObject<List<Team>>(data);
            return list;
        }
    }
}
