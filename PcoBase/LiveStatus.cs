using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class LiveStatus
    {
        [JsonProperty("controlled_by")]
        public string ControlledBy { get; set; }

        [JsonProperty("controlled_by_id")]
        public int ControlledById { get; set; }

        [JsonProperty("additional_controllers")]
        public List<object> AdditionalControllers { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("next_id")]
        public int NextId { get; set; }

        [JsonProperty("next_item_id")]
        public int NextItemId { get; set; }

        [JsonProperty("next_title")]
        public string NextTitle { get; set; }

        [JsonProperty("service_type")]
        public string ServiceType { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("actual_start")]
        public string ActualStart { get; set; }

        [JsonProperty("scheduled_start")]
        public string ScheduledStart { get; set; }

        [JsonProperty("service_time_id")]
        public int ServiceTimeId { get; set; }
    }
}