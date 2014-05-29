using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class Plan
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

        [JsonProperty("service_type")]
		public ServiceType ServiceType { get; set; }

        [JsonProperty("total_length")]
		public int TotalLength { get; set; }

        [JsonProperty("total_length_formatted")]
		public string TotalLengthFormatted { get; set; }

        [JsonProperty("comma_separated_attachment_type_ids")]
		public string CommaSeparatedAttachmentTypeIds { get; set; }

        [JsonProperty("plan_notes")]
		public List<PlanNote> PlanNotes { get; set; }

        [JsonProperty("positions")]
		public List<Position> Positions { get; set; }

        [JsonProperty("items")]
		public List<Item> Items { get; set; }

        [JsonProperty("service_times")]
		public List<ServiceTime> ServiceTimes { get; set; }

        [JsonProperty("rehearsal_times")]
		public List<object> RehearsalTimes { get; set; }

        [JsonProperty("other_times")]
		public List<object> OtherTimes { get; set; }

        [JsonProperty("attachments")]
		public List<object> Attachments { get; set; }

        [JsonProperty("sort_by")]
		public string SortBy { get; set; }

        [JsonProperty("plan_people")]
		public List<PlanPeople> PlanPeople { get; set; }

        [JsonProperty("next_plan_id")]
		public int NextPlanId { get; set; }

        [JsonProperty("prev_plan_id")]
		public int PrevPlanId { get; set; }

        [JsonProperty("plan_contributions")]
		public List<PlanContribution> PlanContributions { get; set; }
    }
}