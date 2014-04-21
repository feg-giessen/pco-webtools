namespace PcoBase
{
    public class PlanNote
    {
        public int id { get; set; }
        public string note { get; set; }
        public int plan_id { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int category_sequence { get; set; }
    }
}