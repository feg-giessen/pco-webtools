using Newtonsoft.Json;

namespace PcoBase
{
    public class PlanContribution
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("plan_base_id")]
        public int PlanBaseId { get; set; }

        [JsonProperty("person")]
        public PersonReference Person { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }
}