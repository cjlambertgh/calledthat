using System;

namespace FootballDataApiV2.Models
{
    public class Competition : IModel
    {
        public int Id { get; set; }
        public Area Area { get; set; }
        public string Name { get; set; }
        public object Code { get; set; }
        public string Plan { get; set; }
        public CurrentSeason CurrentSeason { get; set; }
        public int NumberOfAvailableSeasons { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class CurrentSeason
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? CurrentMatchday { get; set; }
    }
}
