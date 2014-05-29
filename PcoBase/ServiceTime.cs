using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class ServiceTime
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ministry_id")]
        public string MinistryId { get; set; }

        [JsonProperty("starts_at")]
        public string StartsAt { get; set; }

        [JsonProperty("ends_at")]
        public string EndsAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("plan_id")]
        public int PlanId { get; set; }

        [JsonProperty("plan_visible")]
        public bool PlanVisible { get; set; }

        [JsonProperty("created_by_id")]
        public int CreatedById { get; set; }

        [JsonProperty("updated_by_id")]
        public int UpdatedById { get; set; }

        [JsonProperty("print")]
        public bool Print { get; set; }

        [JsonProperty("recorded")]
        public bool Recorded { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("time_type")]
        public string TimeType { get; set; }

        [JsonProperty("actual_start")]
        public object ActualStart { get; set; }

        [JsonProperty("actual_end")]
        public object ActualEnd { get; set; }

        [JsonProperty("excluded_categories")]
        public List<object> ExcludedCategories { get; set; }

        [JsonProperty("category_reminders")]
        public CategoryReminders CategoryReminders { get; set; }

        [JsonProperty("type_to_s")]
        public string TypeToS { get; set; }

        [JsonProperty("starts_at_unformatted")]
        public string StartsAtUnformatted { get; set; }

        [JsonProperty("ends_at_unformatted")]
        public string EndsAtUnformatted { get; set; }
    }
}