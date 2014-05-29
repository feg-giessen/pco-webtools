namespace PcoBase
{
    public class PlanPeople
    {
        public int id { get; set; }

        public int plan_id { get; set; }

        public string position { get; set; }

        public string status { get; set; }

        public bool prepare_notification { get; set; }

        public int category_id { get; set; }

        public string category_name { get; set; }

        public int? category_sequence { get; set; }

        public string person_photo_thumbnail { get; set; }

        public int person_id { get; set; }

        public string person_name { get; set; }

        public int? responds_to_id { get; set; }

        public string excluded_times { get; set; }

        public string notes { get; set; }
    }
}