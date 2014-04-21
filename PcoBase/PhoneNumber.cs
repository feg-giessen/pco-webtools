namespace PcoBase
{
    public class PhoneNumber
    {
        public int id { get; set; }
        public string number { get; set; }
        public bool text_enabled { get; set; }
        public bool text_for_plan_emails { get; set; }
        public bool text_for_notifications { get; set; }
        public bool text_for_reminders { get; set; }
        public bool text_for_people_emails { get; set; }
        public string carrier { get; set; }
        public string location { get; set; }
    }
}