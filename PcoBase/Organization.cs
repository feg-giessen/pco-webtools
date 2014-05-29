using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class Organization
    {
        [JsonProperty("id")]
		public int Id { get; set; }

        [JsonProperty("account_center_id")]
		public int AccountCenterId { get; set; }

        [JsonProperty("name")]
		public string Name { get; set; }

        [JsonProperty("owner_name")]
		public string OwnerName { get; set; }

        [JsonProperty("music_stand_enabled")]
		public bool MusicStandEnabled { get; set; }

        [JsonProperty("projector_enabled")]
		public object ProjectorEnabled { get; set; }

        [JsonProperty("ccli_connected")]
		public bool CcliConnected { get; set; }

        [JsonProperty("secret")]
		public int Secret { get; set; }

        [JsonProperty("date_format")]
		public int DateFormat { get; set; }

        [JsonProperty("twenty_four_hour_time")]
		public bool TwentyFourHourTime { get; set; }

        [JsonProperty("total_songs")]
		public int TotalSongs { get; set; }

        [JsonProperty("total_people")]
		public int TotalPeople { get; set; }

        [JsonProperty("service_types")]
		public List<ServiceType> ServiceTypes { get; set; }

        [JsonProperty("service_type_folders")]
		public List<ServiceTypeFolder> ServiceTypeFolders { get; set; }
    }
}