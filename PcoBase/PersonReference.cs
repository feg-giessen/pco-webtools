using Newtonsoft.Json;

namespace PcoBase
{
    public class PersonReference
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("photo_icon_url")]
        public string PhotoIconUrl { get; set; }

        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }
    }
}