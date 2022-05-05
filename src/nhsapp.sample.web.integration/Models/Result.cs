using Newtonsoft.Json;

namespace nhsapp.sample.web.integration.Models
{
    public sealed class Result
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("html_url")]
        public string Url { get; set; }
    }
}
