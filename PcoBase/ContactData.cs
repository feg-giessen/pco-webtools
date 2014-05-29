using System.Collections.Generic;

namespace PcoBase
{
    public class ContactData
    {
        public int id { get; set; }

        public int person_id { get; set; }

        public List<Address> addresses { get; set; }

        public List<EmailAddress> email_addresses { get; set; }

        public List<PhoneNumber> phone_numbers { get; set; }
    }
}