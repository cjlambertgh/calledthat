using Data.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Procs
{
    class LeagueStats
    {
        public Guid PlayerID { get; set; }
        public int Points { get; set; }
        public int GameweekNumber { get; set; }
        public string Stat
        {
            get { return LeagueStatType.ToString(); }
            set { LeagueStatType = value.ToEnum<LeagueStatType>(); }
        }

        [NotMapped]
        public LeagueStatType LeagueStatType { get; set; }

    }

    public enum LeagueStatType
    {
        LowestScore,
        HighestScore
    }
}
