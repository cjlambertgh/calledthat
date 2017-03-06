using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Models
{
    public class League : IModel
    {
        public string LeagueCaption { get; set; }
        public int MatchDay { get; set; }
        [JsonProperty("standing")]
        public List<Standing> Standings { get; set; }
    }

    public class Standing
    {
        public int Rank { get; set; }
        public string Team { get; set; }
        public int TeamId { get; set; }
        public int PlayedGames { get; set; }
        public string BadgeUrl { get; set; }
        public int Points { get; set; }
        public int Goals { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
    }
}
