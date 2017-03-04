using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Models
{
    public class Competition : IModel
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string League { get; set; }
        public string Year { get; set; }
        public int NumberOfTeams { get; set; }
        public int NumberOfGames { get; set; }
        public string LastUpdated { get; set; }

        public override string ToString()
        {
            return League;
        }
    }
}
