using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class Attachment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("allow_mp3_download")]
        public bool AllowMp3Download { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("downloadable")]
        public bool Downloadable { get; set; }

        [JsonProperty("streamable")]
        public bool Streamable { get; set; }

        [JsonProperty("comma_separated_page_order")]
        public object CommaSeparatedPageOrder { get; set; }

        [JsonProperty("comma_separated_attachment_type_ids")]
        public string CommaSeparatedAttachmentTypeIds { get; set; }

        [JsonProperty("linked_object_type")]
        public string LinkedObjectType { get; set; }

        [JsonProperty("linked_object_id")]
        public int LinkedObjectId { get; set; }

        [JsonProperty("offset_x")]
        public double? OffsetX { get; set; }

        [JsonProperty("offset_y")]
        public double? OffsetY { get; set; }

        [JsonProperty("zoom")]
        public double? Zoom { get; set; }

        [JsonProperty("custom_zooms")]
        public List<object> CustomZooms { get; set; }

        [JsonProperty("secure_link")]
        public string SecureLink { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }
}