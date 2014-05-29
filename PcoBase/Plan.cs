using System.Collections.Generic;

namespace PcoBase
{
    public class Plan
    {
        public int id { get; set; }

        public string plan_title { get; set; }

        public string series_title { get; set; }

        public int service_type_id { get; set; }

        public string service_type_name { get; set; }

        public string dates { get; set; }

        public object series { get; set; }

        public bool @public { get; set; }

        public string updated_at { get; set; }

        public string created_at { get; set; }

        public string sort_date { get; set; }

        public string type { get; set; }

        public UserReference updated_by { get; set; }

        public UserReference created_by { get; set; }

        public string permissions { get; set; }

        public ServiceType service_type { get; set; }

        public int total_length { get; set; }

        public string total_length_formatted { get; set; }

        public string comma_separated_attachment_type_ids { get; set; }

        public List<PlanNote> plan_notes { get; set; }

        public List<Position> positions { get; set; }

        public List<Item> items { get; set; }

        public List<ServiceTime> service_times { get; set; }

        public List<object> rehearsal_times { get; set; }

        public List<object> other_times { get; set; }

        public List<object> attachments { get; set; }

        public string sort_by { get; set; }

        public List<PlanPeople> plan_people { get; set; }

        public int next_plan_id { get; set; }

        public int prev_plan_id { get; set; }

        public List<PlanContribution> plan_contributions { get; set; }
    }
}