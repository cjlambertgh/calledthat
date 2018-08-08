using Newtonsoft.Json;
using System;

namespace FootballDataApiV2.Models
{
    public partial class Team : IModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("area")]
        public Area Area { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("tla")]
        public string Tla { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("founded")]
        public long Founded { get; set; }

        [JsonProperty("clubColors")]
        public string ClubColors { get; set; }

        [JsonProperty("venue")]
        public object Venue { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTimeOffset LastUpdated { get; set; }
    }
}
