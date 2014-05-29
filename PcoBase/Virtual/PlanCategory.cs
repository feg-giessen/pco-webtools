using System.Collections.Generic;
using System.Linq;

namespace PcoBase.Virtual
{
    public class PlanCategory
    {
        public int id { get; set; }

        public int sequence { get; set; }

        public string name { get; set; }

        public List<PlanPosition> positions { get; set; }

        public static IList<PlanCategory> FromPlan(Plan plan)
        {
            var list = new List<PlanCategory>();

            foreach (PlanPeople item in plan.plan_people)
            {
                PlanCategory category = list.FirstOrDefault(x => x.id == item.category_id);

                if (category == null)
                {
                    category = new PlanCategory { id = item.category_id, name = item.category_name, sequence = item.category_sequence ?? int.MaxValue, positions = new List<PlanPosition>(), };

                    list.Add(category);
                }

                PlanPosition position = category.positions.FirstOrDefault(x => x.name == item.position);

                if (position == null)
                {
                    position = new PlanPosition { name = item.position };

                    category.positions.Add(position);
                }
            }

            foreach (Position item in plan.positions)
            {
                PlanCategory category = list.FirstOrDefault(x => x.id == item.category_id);

                if (category == null)
                {
                    category = new PlanCategory { id = item.category_id, name = item.category_name, sequence = item.category_sequence ?? int.MaxValue, positions = new List<PlanPosition>(), };

                    list.Add(category);
                }

                PlanPosition position = category.positions.FirstOrDefault(x => x.name == item.position);

                if (position == null)
                {
                    position = new PlanPosition { name = item.position };

                    category.positions.Add(position);
                }
            }

            return list;
        }
    }
}