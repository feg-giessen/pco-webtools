using Newtonsoft.Json;

namespace PcoBase
{
    public class ItemTime
    {
        [JsonProperty("id")]
		public int Id { get; set; }

        [JsonProperty("plan_item_id")]
		public int PlanItemId { get; set; }

        [JsonProperty("time_id")]
		public int TimeId { get; set; }

        [JsonProperty("live_start")]
		public object LiveStart { get; set; }

        [JsonProperty("live_end")]
		public object LiveEnd { get; set; }

        [JsonProperty("plan_id")]
		public int PlanId { get; set; }

        [JsonProperty("exclude")]
		public bool Exclude { get; set; }
    }
}