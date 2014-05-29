using Newtonsoft.Json;

namespace PcoBase
{
    public class Property
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("field_id")]
        public int FieldId { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("field_allows_multiple_selections")]
        public bool FieldAllowsMultipleSelections { get; set; }

        [JsonProperty("option")]
        public string Option { get; set; }

        [JsonProperty("option_id")]
        public int OptionId { get; set; }
    }
}