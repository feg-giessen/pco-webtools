using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class Key
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("starting")]
        public string Starting { get; set; }

        [JsonProperty("ending")]
        public string Ending { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alternate_keys")]
        public List<object> AlternateKeys { get; set; }
    }
}