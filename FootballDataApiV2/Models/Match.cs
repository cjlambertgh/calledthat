using Newtonsoft.Json;
using System;

namespace FootballDataApiV2.Models
{
    public partial class Match : IModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("season")]
        public Season Season { get; set; }

        [JsonProperty("utcDate")]
        public DateTime UtcDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("matchday")]
        public int Matchday { get; set; }

        [JsonProperty("stage")]
        public string Stage { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("homeTeam")]
        public Team HomeTeam { get; set; }

        [JsonProperty("awayTeam")]
        public Team AwayTeam { get; set; }

        [JsonProperty("score")]
        public Score Score { get; set; }

        [JsonProperty("referees")]
        public object[] Referees { get; set; }

        public FixtureStatus FixtureStatus => (FixtureStatus)Enum.Parse(typeof(FixtureStatus), Status, true);

        public bool IsFixtureInPlay => (FixtureStatus == FixtureStatus.In_Play);

        public bool IsFixtureInFinished => FixtureStatus == FixtureStatus.Finished;
        
    }

    public partial class Score
    {
        [JsonProperty("winner")]
        public object Winner { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("fullTime")]
        public Score FullTime { get; set; }

        [JsonProperty("halfTime")]
        public Score HalfTime { get; set; }

        [JsonProperty("extraTime")]
        public Score ExtraTime { get; set; }

        [JsonProperty("penalties")]
        public Score Penalties { get; set; }
    }

    public partial class Score
    {
        [JsonProperty("homeTeam")]
        public int? HomeTeam { get; set; }

        [JsonProperty("awayTeam")]
        public int? AwayTeam { get; set; }
    }

    public partial class Season
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("startDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTimeOffset EndDate { get; set; }

        [JsonProperty("currentMatchday")]
        public int? CurrentMatchday { get; set; }
    }

    public enum FixtureStatus
    {
        Scheduled,
        Timed,
        In_Play,
        Postponed,
        Canceled,
        Finished
    }
}
