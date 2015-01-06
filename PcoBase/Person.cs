using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class Person
    {
        [JsonProperty("id")]
		public int Id { get; set; }

        [JsonProperty("account_center_id")]
		public int AccountCenterId { get; set; }

        [JsonProperty("first_name")]
		public string FirstName { get; set; }

        [JsonProperty("last_name")]
		public string LastName { get; set; }

        [JsonProperty("photo_thumbnail_url")]
		public string PhotoThumbnailUrl { get; set; }

        [JsonProperty("name")]
		public string Name { get; set; }

        [JsonProperty("photo_url")]
		public string PhotoUrl { get; set; }

        [JsonProperty("last_service_type_id")]
		public int LastServiceTypeId { get; set; }

        [JsonProperty("permissions")]
		public string Permissions { get; set; }

        [JsonProperty("created_at")]
		public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
		public string UpdatedAt { get; set; }

        [JsonProperty("organization_id")]
		public int OrganizationId { get; set; }

        [JsonProperty("notes")]
		public string Notes { get; set; }

        [JsonProperty("created_by_id")]
		public int? CreatedById { get; set; }

        [JsonProperty("updated_by_id")]
		public int? UpdatedById { get; set; }

        [JsonProperty("logged_in_at")]
		public string LoggedInAt { get; set; }

        [JsonProperty("max_permissions")]
		public string MaxPermissions { get; set; }

        [JsonProperty("properties")]
		public List<Property> Properties { get; set; }

        [JsonProperty("facebook_id")]
		public object FacebookId { get; set; }

        [JsonProperty("remote_id")]
		public object RemoteId { get; set; }

        [JsonProperty("birthdate")]
		public object Birthdate { get; set; }

        [JsonProperty("anniversary")]
		public object Anniversary { get; set; }

        [JsonProperty("contact_data")]
		public ContactData ContactData { get; set; }

        [JsonProperty("connected_person_ids")]
		public object ConnectedPersonIds { get; set; }

        [JsonProperty("ical_code")]
		public string IcalCode { get; set; }
    }

    public class PersonsResponse
    {
        [JsonProperty("people")]
        public List<Person> People { get; set; } 
    }
}