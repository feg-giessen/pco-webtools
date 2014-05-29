using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class ContactData
    {
        [JsonProperty("id")]
		public int Id { get; set; }

        [JsonProperty("person_id")]
		public int PersonId { get; set; }

        [JsonProperty("addresses")]
		public List<Address> Addresses { get; set; }

        [JsonProperty("email_addresses")]
		public List<EmailAddress> EmailAddresses { get; set; }

        [JsonProperty("phone_numbers")]
		public List<PhoneNumber> PhoneNumbers { get; set; }
    }
}