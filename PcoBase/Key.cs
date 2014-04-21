using System.Collections.Generic;

namespace PcoBase
{
    public class Key
    {
        public int id { get; set; }
        public string starting { get; set; }
        public string ending { get; set; }
        public string name { get; set; }
        public List<object> alternate_keys { get; set; }
    }
}