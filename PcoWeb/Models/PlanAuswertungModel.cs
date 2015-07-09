using System;

namespace PcoWeb.Models
{
    public class PlanAuswertungModel
    {
        public DateTime Date { get; set; }

        public decimal NichtBand { get; set; }

        public decimal Band { get; set; }

        public decimal? BandProzent
        {
            get
            {
                if ((this.NichtBand + this.Band) == 0)
                    return default(decimal?);

                return this.Band / (this.NichtBand + this.Band);
            }
        }
    }
}
