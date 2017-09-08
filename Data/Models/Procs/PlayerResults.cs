using System;

namespace Data.Models.Procs
{
    public class PlayerResults
    {
        public int GameweekNumber { get; set; }
        public DateTime KickOffDateTime { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public int HomeScorePick { get; set; }
        public int AwayScorePick { get; set; }
        public int Points { get; set; }
        public bool Banker { get; set; }
        public bool Double { get; set; }
    }
}
