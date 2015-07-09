using Newtonsoft.Json;

namespace PcoBase
{
    public class LivePlan
    {
        [JsonProperty("service_type_id")]
        public int ServiceTypeId { get; set; }

        [JsonProperty("service_type_name")]
        public string ServiceTypeName { get; set; }

        [JsonProperty("plan_id")]
        public int PlanId { get; set; }

        [JsonProperty("plan_title")]
        public string PlanTitle { get; set; }

        [JsonProperty("plan_series_title")]
        public string PlanSeriesTitle { get; set; }

        [JsonProperty("plan_dates")]
        public string PlanDates { get; set; }

        [JsonProperty("chat_room")]
        public string ChatRoom { get; set; }

        [JsonProperty("live")]
        public string Live { get; set; }

        [JsonProperty("status")]
        public LiveStatus LiveStatus { get; set; }
    }
}
