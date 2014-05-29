using Newtonsoft.Json;

namespace PcoBase
{
    public class PlanNote
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("plan_id")]
        public int PlanId { get; set; }

        [JsonProperty("category_id")]
        public int CategoryId { get; set; }

        [JsonProperty("category_name")]
        public string CategoryName { get; set; }

        [JsonProperty("category_sequence")]
        public int CategorySequence { get; set; }
    }
}