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

            foreach (PlanPeople item in plan.PlanPeople)
            {
                PlanCategory category = list.FirstOrDefault(x => x.id == item.CategoryId);

                if (category == null)
                {
                    category = new PlanCategory { id = item.CategoryId, name = item.CategoryName, sequence = item.CategorySequence ?? int.MaxValue, positions = new List<PlanPosition>(), };

                    list.Add(category);
                }

                PlanPosition position = category.positions.FirstOrDefault(x => x.name == item.Position);

                if (position == null)
                {
                    position = new PlanPosition { name = item.Position };

                    category.positions.Add(position);
                }
            }

            foreach (Position item in plan.Positions)
            {
                PlanCategory category = list.FirstOrDefault(x => x.id == item.CategoryId);

                if (category == null)
                {
                    category = new PlanCategory { id = item.CategoryId, name = item.CategoryName, sequence = item.CategorySequence ?? int.MaxValue, positions = new List<PlanPosition>(), };

                    list.Add(category);
                }

                PlanPosition position = category.positions.FirstOrDefault(x => x.name == item.Name);

                if (position == null)
                {
                    position = new PlanPosition { name = item.Name };

                    category.positions.Add(position);
                }
            }

            return list;
        }
    }
}