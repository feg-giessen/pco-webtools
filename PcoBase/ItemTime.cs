using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PcoBase
{
    public class ItemTime
    {
        public int id { get; set; }
        public int plan_item_id { get; set; }
        public int time_id { get; set; }
        public object live_start { get; set; }
        public object live_end { get; set; }
        public int plan_id { get; set; }
        public bool exclude { get; set; }
    }
}
