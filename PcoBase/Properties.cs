using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class Properties
    {
        [JsonProperty("class_name")]
        public string ClassName { get; set; }

        [JsonProperty("fields")]
        public List<object> Fields { get; set; }

        [JsonProperty("options")]
        public List<object> Options { get; set; }
    }
}