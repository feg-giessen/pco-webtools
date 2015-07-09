using System.Collections.Generic;
using System.Diagnostics;

using Newtonsoft.Json;

namespace PcoBase
{
    [DebuggerDisplay("{Type}: {CategoryName}, {Name}")]
    public class MinistryPositionsResult
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
        public int? Sequence { get; set; }

        [JsonProperty("attachment_types_enabled")]
        public bool AttachmentTypesEnabled { get; set; }

        [JsonProperty("permissions")]
        public string Permissions { get; set; }

        [JsonProperty("ministry_id")]
        public int? MinistryId { get; set; }

        [JsonProperty("service_type_id")]
        public int? ServiceTypeId { get; set; }

        [JsonProperty("service_type")]
        public string ServiceType { get; set; }

        [JsonProperty("name_for_data_attr")]
        public string NameForDataAttr { get; set; }

        [JsonProperty("scheduled_viewers_see")]
        public int? ScheduledViewersSee { get; set; }

        [JsonProperty("category_id")]
        public int? CategoryId { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("attachment_type_ids")]
        public List<string> AttachmentTypeIds { get; set; }

        [JsonProperty("category_name")]
        public string CategoryName { get; set; }

        [JsonProperty("category_sequence")]
        public int? CategorySequence { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("can_be_deleted")]
        public bool? CanBeDeleted { get; set; }
    }
}