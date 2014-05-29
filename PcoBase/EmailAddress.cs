using Newtonsoft.Json;

namespace PcoBase
{
    public class EmailAddress
    {
        [JsonProperty("id")]
		public int Id { get; set; }

        [JsonProperty("address")]
		public string Address { get; set; }

        [JsonProperty("location")]
		public string Location { get; set; }
    }
}