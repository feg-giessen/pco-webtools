using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class ServiceTypeFolder
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

        [JsonProperty("service_types")]
        public List<ServiceType> ServiceTypes { get; set; }

        [JsonProperty("service_type_folders")]
        public List<ServiceTypeFolder> ServiceTypeFolders { get; set; }
    }
}