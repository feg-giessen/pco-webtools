using System.Collections.Generic;

namespace PcoBase
{
    public class Organization
    {
        public int id { get; set; }

        public int account_center_id { get; set; }

        public string name { get; set; }

        public string owner_name { get; set; }

        public bool music_stand_enabled { get; set; }

        public object projector_enabled { get; set; }

        public bool ccli_connected { get; set; }

        public int secret { get; set; }

        public int date_format { get; set; }

        public bool twenty_four_hour_time { get; set; }

        public int total_songs { get; set; }

        public int total_people { get; set; }

        public List<ServiceType> service_types { get; set; }

        public List<ServiceTypeFolder> service_type_folders { get; set; }
    }
}