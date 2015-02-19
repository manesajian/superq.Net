using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class superqelem : LinkedListNode
    {
        public string name;
        public object value;
        public superq parentSq;
        public bool buildFromStr;

        public superqelem(string name,
                          object value,
                          superq parentSq,
                          bool buildFromStr)
        {

        }

        public string ToString()
        {
            return "";
        }

        public void FromString(string sqe)
        {

        }
    }
}
