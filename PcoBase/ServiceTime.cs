using System.Collections.Generic;

namespace PcoBase
{
    public class ServiceTime
    {
        public int id { get; set; }

        public string ministry_id { get; set; }

        public string starts_at { get; set; }

        public string ends_at { get; set; }

        public string updated_at { get; set; }

        public string created_at { get; set; }

        public int plan_id { get; set; }

        public bool plan_visible { get; set; }

        public int created_by_id { get; set; }

        public int updated_by_id { get; set; }

        public bool print { get; set; }

        public bool recorded { get; set; }

        public string name { get; set; }

        public string time_type { get; set; }

        public object actual_start { get; set; }

        public object actual_end { get; set; }

        public List<object> excluded_categories { get; set; }

        public CategoryReminders category_reminders { get; set; }

        public string type_to_s { get; set; }

        public string starts_at_unformatted { get; set; }

        public string ends_at_unformatted { get; set; }
    }
}