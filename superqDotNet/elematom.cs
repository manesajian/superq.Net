using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class elematom : LinkedListNode
    {
        public string name { get; set; }
        public string type { get; set; }
        public object value { get; set; }

        public elematom(string name, string type, object value)
        {
            this.name = name;
            this.type = type;
            this.value = value;
        }
    }
}
