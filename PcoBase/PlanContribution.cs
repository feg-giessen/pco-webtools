namespace PcoBase
{
    public class PlanContribution
    {
        public int id { get; set; }
        public int plan_base_id { get; set; }
        public PersonReference person { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}