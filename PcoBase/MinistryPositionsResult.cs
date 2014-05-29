using System.Collections.Generic;

namespace PcoBase
{
    public class MinistryPositionsResult
    {
        public int id { get; set; }

        public string name { get; set; }

        public int parent_id { get; set; }

        public string type { get; set; }

        public string container { get; set; }

        public object container_id { get; set; }

        public int? sequence { get; set; }

        public bool attachment_types_enabled { get; set; }

        public string permissions { get; set; }

        public int? ministry_id { get; set; }

        public int? service_type_id { get; set; }

        public string service_type { get; set; }

        public string name_for_data_attr { get; set; }

        public int? scheduled_viewers_see { get; set; }

        public int? category_id { get; set; }

        public Properties properties { get; set; }

        public List<string> attachment_type_ids { get; set; }

        public string category_name { get; set; }

        public int? category_sequence { get; set; }

        public string position { get; set; }

        public int? quantity { get; set; }

        public bool? can_be_deleted { get; set; }
    }
}