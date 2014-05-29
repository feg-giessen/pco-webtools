using Newtonsoft.Json;

namespace PcoBase
{
    public class ServiceType
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("container")]
        public string Container { get; set; }

        [JsonProperty("container_id")]
        public object ContainerId { get; set; }

        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        [JsonProperty("attachment_types_enabled")]
        public bool AttachmentTypesEnabled { get; set; }

        [JsonProperty("permissions")]
        public string Permissions { get; set; }
    }
}