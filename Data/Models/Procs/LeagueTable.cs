using System;

namespace Data.Models.Procs
{
    public class LeagueTable
    {
        public string PlayerName { get; set; }
        public Guid PlayerId { get; set; }
        public string LeagueName { get; set; }
        public int GameweekPoints { get; set; }
        public int TotalPoints { get; set; }
        public Guid GameweekId { get; set; }
        public Guid LeagueId { get; set; }
    }
}
