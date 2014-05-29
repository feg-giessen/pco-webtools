using Newtonsoft.Json;

namespace PcoBase
{
    public class PlanPeople
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("plan_id")]
        public int PlanId { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("prepare_notification")]
        public bool PrepareNotification { get; set; }

        [JsonProperty("category_id")]
        public int CategoryId { get; set; }

        [JsonProperty("category_name")]
        public string CategoryName { get; set; }

        [JsonProperty("category_sequence")]
        public int? CategorySequence { get; set; }

        [JsonProperty("person_photo_thumbnail")]
        public string PersonPhotoThumbnail { get; set; }

        [JsonProperty("person_id")]
        public int PersonId { get; set; }

        [JsonProperty("person_name")]
        public string PersonName { get; set; }

        [JsonProperty("responds_to_id")]
        public int? RespondsToId { get; set; }

        [JsonProperty("excluded_times")]
        public string ExcludedTimes { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }
    }
}