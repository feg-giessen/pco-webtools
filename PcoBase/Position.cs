using Newtonsoft.Json;

namespace PcoBase
{
    public class Position
    {
        [JsonProperty("category_name")]
        public string CategoryName { get; set; }

        [JsonProperty("category_sequence")]
        public int? CategorySequence { get; set; }

        [JsonProperty("category_id")]
        public int CategoryId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("plan_id")]
        public int PlanId { get; set; }

        [JsonProperty("position")]
        public string Name { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}