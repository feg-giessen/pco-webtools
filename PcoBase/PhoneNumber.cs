using Newtonsoft.Json;

namespace PcoBase
{
    public class PhoneNumber
    {
        [JsonProperty("id")]
		public int Id { get; set; }

        [JsonProperty("number")]
		public string Number { get; set; }

        [JsonProperty("text_enabled")]
		public bool TextEnabled { get; set; }

        [JsonProperty("text_for_plan_emails")]
		public bool TextForPlanEmails { get; set; }

        [JsonProperty("text_for_notifications")]
		public bool TextForNotifications { get; set; }

        [JsonProperty("text_for_reminders")]
		public bool TextForReminders { get; set; }

        [JsonProperty("text_for_people_emails")]
		public bool TextForPeopleEmails { get; set; }

        [JsonProperty("carrier")]
		public string Carrier { get; set; }

        [JsonProperty("location")]
		public string Location { get; set; }
    }
}