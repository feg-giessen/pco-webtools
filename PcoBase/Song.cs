using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class Song
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("admin")]
        public string Admin { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("ccli_id")]
        public int? CcliId { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("created_by_id")]
        public int CreatedById { get; set; }

        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("themes")]
        public string Themes { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("updated_by_id")]
        public int UpdatedById { get; set; }

        [JsonProperty("last_plan_id")]
        public int LastPlanId { get; set; }

        [JsonProperty("attachments")]
        public List<object> Attachments { get; set; }
    }
}