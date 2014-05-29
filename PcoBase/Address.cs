using Newtonsoft.Json;

namespace PcoBase
{
    public class Address
    {
        [JsonProperty("id")]
		public int Id { get; set; }

        [JsonProperty("street")]
		public string Street { get; set; }

        [JsonProperty("city")]
		public string City { get; set; }

        [JsonProperty("state")]
		public string State { get; set; }

        [JsonProperty("zip")]
		public string Zip { get; set; }

        [JsonProperty("location")]
		public string Location { get; set; }
    }
}