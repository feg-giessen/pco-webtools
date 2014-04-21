using System.Collections.Generic;

namespace PcoBase
{
    public class Person
    {
        public int id { get; set; }
        public int account_center_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string photo_thumbnail_url { get; set; }
        public string name { get; set; }
        public string photo_url { get; set; }
        public int last_service_type_id { get; set; }
        public string permissions { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int organization_id { get; set; }
        public string notes { get; set; }
        public int? created_by_id { get; set; }
        public int? updated_by_id { get; set; }
        public string logged_in_at { get; set; }
        public string max_permissions { get; set; }
        public List<Property> properties { get; set; }
        public object facebook_id { get; set; }
        public object remote_id { get; set; }
        public object birthdate { get; set; }
        public object anniversary { get; set; }
        public ContactData contact_data { get; set; }
        public object connected_person_ids { get; set; }
        public string ical_code { get; set; }
    }
}