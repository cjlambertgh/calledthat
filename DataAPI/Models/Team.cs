using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Models
{
    public class Team : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string MarketValue { get; set; }
        public string BadgeUrl { get; set; }
    }

    public class TeamWrapper
    {
        public List<Team> Teams { get; set; }
    }
}
