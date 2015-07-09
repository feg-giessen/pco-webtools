using Newtonsoft.Json;

namespace PcoBase
{
    public class ItemNote
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("category_id")]
        public int CategoryId { get; set; }

        [JsonProperty("category_name")]
        public string CategoryName { get; set; }
    }
}