namespace PcoBase
{
    public class PlanIndex
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
        public bool scheduled { get; set; }
    }
}