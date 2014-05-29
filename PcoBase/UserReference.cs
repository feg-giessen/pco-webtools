using Newtonsoft.Json;

namespace PcoBase
{
    public class UserReference
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}