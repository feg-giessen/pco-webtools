using System.Collections.Generic;

namespace PcoBase
{
    public class Song
    {
        public int id { get; set; }
        public string title { get; set; }
        public string admin { get; set; }
        public string author { get; set; }
        public int? ccli_id { get; set; }
        public string copyright { get; set; }
        public string created_at { get; set; }
        public int created_by_id { get; set; }
        public bool hidden { get; set; }
        public string notes { get; set; }
        public string themes { get; set; }
        public string updated_at { get; set; }
        public int updated_by_id { get; set; }
        public int last_plan_id { get; set; }
        public List<object> attachments { get; set; }
    }
}