using System.Collections.Generic;

namespace PcoWeb.Models
{
    public class AuswertungModel
    {
        public List<PlanAuswertungModel> Morgens { get; set; }

        public List<PlanAuswertungModel> Abends { get; set; }
    }
}
