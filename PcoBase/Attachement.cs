using System.Collections.Generic;

namespace PcoBase
{
    public class Attachment
    {
        public string id { get; set; }

        public string url { get; set; }

        public bool allow_mp3_download { get; set; }

        public string content_type { get; set; }

        public string filename { get; set; }

        public bool downloadable { get; set; }

        public bool streamable { get; set; }

        public object comma_separated_page_order { get; set; }

        public string comma_separated_attachment_type_ids { get; set; }

        public string linked_object_type { get; set; }

        public int linked_object_id { get; set; }

        public double? offset_x { get; set; }

        public double? offset_y { get; set; }

        public double? zoom { get; set; }

        public List<object> custom_zooms { get; set; }

        public string secure_link { get; set; }

        public string updated_at { get; set; }
    }
}