using System;
using System.Collections.Generic;
using System.Linq;
using PcoBase;

namespace PcoWeb.Models
{
    public class PersonNameService
    {
        private readonly IList<Person> persons;

        private readonly IDictionary<int, string> names = new Dictionary<int, string>();

        public PersonNameService(IList<Person> persons)
        {
            if (persons == null)
                throw new ArgumentNullException("persons");

            this.persons = persons;
        }

        protected IList<Person> Persons
        {
            get
            {
                if (this.names.Count == 0)
                {
                    this.ProcessPersons();
                }

                return this.persons;
            }
        }

        public string GetName(int personId)
        {
            Person person = this.Persons.FirstOrDefault(p => p.Id == personId);

            if (person == null)
                return null;

            string firstName;
            if (!this.names.TryGetValue(personId, out firstName))
            {
                firstName = person.FirstName.Length > 0 ? person.FirstName.Substring(0, 1) + ". " : string.Empty;
            }
            else
            {
                firstName += " ";
            }

            return firstName + person.LastName;
        }

        private void ProcessPersons()
        {
            foreach (var personGroup in this.persons.GroupBy(p => p.LastName))
            {
                var list = personGroup.ToList();

                int length = 1;
                int success = this.names.Keys.Count(k => list.Any(p => p.Id == k));
                while (success < list.Count)
                {
                    foreach (Person p in list)
                    {
                        if (this.names.ContainsKey(p.Id))
                            continue;

                        if (p.FirstName.Length == length || length > 2)
                        {
                            this.names.Add(p.Id, p.FirstName);
                            success++;
                        }
                        else if (!list.Any(l => l.Id != p.Id && l.FirstName.Length >= length && p.FirstName.Substring(0, length) == l.FirstName.Substring(0, length)))
                        {
                            this.names.Add(p.Id, p.FirstName.Substring(0, length) + ".");
                            success++;
                        }
                    }

                    length++;
                }
            }
        } 
    }
}