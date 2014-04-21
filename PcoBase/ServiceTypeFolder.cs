using System.Collections.Generic;

namespace PcoBase
{
    public class ServiceTypeFolder
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parent_id { get; set; }
        public string type { get; set; }
        public string container { get; set; }
        public object container_id { get; set; }
        public List<ServiceType> service_types { get; set; }
        public List<ServiceTypeFolder> service_type_folders { get; set; }
    }
}