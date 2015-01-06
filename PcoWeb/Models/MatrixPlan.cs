using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

using PcoBase;

namespace PcoWeb.Models
{
    public class MatrixPlan
    {
        private readonly Plan plan;

        private readonly PersonNameService personNames;

        public MatrixPlan(Plan plan, PersonNameService personNames)
        {
            if (plan == null)
                throw new ArgumentNullException("plan");
            if (personNames == null)
                throw new ArgumentNullException("personNames");

            this.plan = plan;
            this.personNames = personNames;
            this.Date = DateTime.Parse(plan.ServiceTimes.First(t => t.TimeType == "Service").StartsAtUnformatted.Replace(" +0000", string.Empty));
        }

        public Plan Item
        {
            get { return this.plan; }
        }

        public DateTime Date { get; private set; }

        public string Anlass
        {
            get
            {
                return this.plan.PlanTitle;
            }
        }

        public string Gottesdienstplanung
        {
            get
            {
                return this.GetPlanPeople("Planung", "Gottesdienstplanung");
            }
        }

        public string Hauptmoderation
        {
            get
            {
                string moderation = this.GetPlanPeople("Moderation", "01 Leitung (Moderation)");

                if (string.IsNullOrWhiteSpace(moderation) || moderation == "#")
                {
                    string alt = this.GetPlanPeople("Moderation", "Haupt");

                    if (alt != "#")
                        return alt;
                }

                if (string.IsNullOrWhiteSpace(moderation) || moderation == "#")
                {
                    string alt = this.GetPlanPeople("Planung", "Haupt");

                    if (alt != "#")
                        return alt;
                }

                return moderation;
            }
        }

        public string Abendmahl
        {
            get
            {
                return this.GetPlanPeople("Abendmahl", "Leitung");
            }
        }

        public string Verkuendigung
        {
            get
            {
                return this.GetPlanNote("Verkündigung");
            }
        }

        public string Thema
        {
            get
            {
                return this.GetPlanNote("Thema");
            }
        }

        public string Musik
        {
            get
            {
                string musik = this.GetPlanPeople("Musik", "verantwortlicher");

                if (string.IsNullOrWhiteSpace(musik) || musik == "#") 
                {
                    string alt = this.GetPlanPeople("Planung", "musik");

                    if (alt != "#")
                        return alt;
                }

                return musik;
            }
        }

        public string Ton
        {
            get
            {
                return this.GetPlanPeople("Technik", "Ton");
            }
        }

        public string Praesentation
        {
            get
            {
                return this.GetPlanPeople("Technik", "Präsentation");
            }
        }

        public string Licht
        {
            get
            {
                return this.GetPlanPeople("Technik", "Licht");
            }
        }

        public string BesElemente
        {
            get
            {
                return this.GetPlanNote("Element");
            }
        }

        public string Bemerkung
        {
            get
            {
                return this.GetPlanNote("Bemerkungen");
            }
        }

        public string Kollekte
        {
            get
            {
                return this.GetPlanNote("Kollekte (Zweck)");
            }
        }

        public string Bistro
        {
            get
            {
                return this.GetPlanNote("Bistro") + this.GetPlanNote("Stammtisch");
            }
        }

        public string Deko
        {
            get
            {
                return this.GetPlanPeople("Deko", string.Empty);
            }
        }

        public string Aufnahme
        {
            get
            {
                return this.GetPlanNote("Aufnahme/Vervielfältigung");
            }
        }

        private string FormatName(string name)
        {
            var regex = new Regex(@"(?<first>\w)\w* (?<last>\w+)");

            if (string.IsNullOrWhiteSpace(name)) 
                return string.Empty;

            if (regex.IsMatch(name)) 
                return regex.Replace(name, "${first}. ${last}");

            return name;
        }

        private string GetPlanPeople(string category, string position)
        {
            string people = string.Join(
                ", ", 
                this.plan.PlanPeople
                    .Where(pp => pp.CategoryName == category && pp.Position.IndexOf(position, StringComparison.InvariantCultureIgnoreCase) > -1)
                    .Where(pp => pp.Status != "D")  // exclude declined
                    .Select(p => this.personNames.GetName(p.PersonId) ?? this.FormatName(p.PersonName)));

            if (string.IsNullOrWhiteSpace(people))
            {
                if (!this.plan.Positions.Any(p => p.CategoryName == category && p.Name.IndexOf(position, StringComparison.InvariantCultureIgnoreCase) > -1))
                {
                    return "#"; // No one required.
                }
            }

            return people;
        }

        private string GetPlanNote(string name)
        {
            return this.plan.PlanNotes.FirstOrDefault(n => n.CategoryName.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) > -1).Try(n => n.Note);
        }

        /*
         * Plan Notes: Anlass	
         * Planung: Gottesdienstplanung	
         * Moderation: 01 Hauptmoderation	
         * Abendmahl: 02 Leitung	
         * Plan Notes: Verkündigung	
         * Plan Notes: Thema/Text	
         * Musik: 01 Musikverantwortlicher	
         * ???	
         * Technik: Ton	
         * Technik: Präsentation	
         * Technik: Licht	
         * "Plan Notes: Bes. Elemente" + "Plan Notes: Bemerkungen"	
         * Plan Notes: Kollekte (Zweck)	
         * "Plan Notes: Bistro" + "Plan Notes: Stammtisch" 	
         * Plan Notes: Deko (irgendwann auch über PCO organisiert)
        */
    }
}