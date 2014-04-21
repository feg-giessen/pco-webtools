using System;
using System.Linq;
using System.Web;

namespace PcoBase
{
    public class ServiceType
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parent_id { get; set; }
        public string type { get; set; }
        public string container { get; set; }
        public object container_id { get; set; }
        public int sequence { get; set; }
        public bool attachment_types_enabled { get; set; }
        public string permissions { get; set; }
    }
}