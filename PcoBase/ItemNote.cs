﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PcoBase
{
    public class ItemNote
    {
        public int id { get; set; }
        public string note { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
    }
}
