﻿using System;
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

        public MatrixPlan(Plan plan)
        {
            if (plan == null)
                throw new ArgumentNullException("plan");

            this.plan = plan;
            this.Date = DateTime.Parse(plan.service_times.First(t => t.time_type == "Service").starts_at_unformatted.Replace(" +0000", string.Empty));
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
                return this.plan.plan_title;
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
                string moderation = this.GetPlanPeople("Moderation", "Haupt");

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
                return this.GetPlanNote("Deko");
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
                this.plan.plan_people
                    .Where(pp => pp.category_name == category && pp.position.IndexOf(position, StringComparison.InvariantCultureIgnoreCase) > -1)
                    .Where(pp => pp.status != "D")  // exclude declined
                    .Select(p => this.FormatName(p.person_name)));

            if (string.IsNullOrWhiteSpace(people))
            {
                if (!this.plan.positions.Any(p => p.category_name == category && p.position.IndexOf(position, StringComparison.InvariantCultureIgnoreCase) > -1))
                {
                    return "#"; // No one required.
                }
            }

            return people;
        }

        private string GetPlanNote(string name)
        {
            return this.plan.plan_notes.FirstOrDefault(n => n.category_name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) > -1).Try(n => n.note);
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