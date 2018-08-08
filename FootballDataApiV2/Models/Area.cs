using Newtonsoft.Json;

namespace FootballDataApiV2.Models
{
    public partial class Area
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
