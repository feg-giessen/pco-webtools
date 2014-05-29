using Newtonsoft.Json;

namespace PcoBase
{
    public class PlanIndex
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("plan_title")]
        public string PlanTitle { get; set; }

        [JsonProperty("series_title")]
        public string SeriesTitle { get; set; }

        [JsonProperty("service_type_id")]
        public int ServiceTypeId { get; set; }

        [JsonProperty("service_type_name")]
        public string ServiceTypeName { get; set; }

        [JsonProperty("dates")]
        public string Dates { get; set; }

        [JsonProperty("series")]
        public object Series { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("sort_date")]
        public string SortDate { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("updated_by")]
        public UserReference UpdatedBy { get; set; }

        [JsonProperty("created_by")]
        public UserReference CreatedBy { get; set; }

        [JsonProperty("permissions")]
        public string Permissions { get; set; }

        [JsonProperty("scheduled")]
        public bool Scheduled { get; set; }
    }
}