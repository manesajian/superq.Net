using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class superqelem : LinkedListNode
    {
        // list of elematoms
        private LinkedList<superqelem> list = new LinkedList<superqelem>();

        // dictionary of elematoms, keyed by 'field' name
        private Dictionary<string, LinkedListNode> dict = new Dictionary<string, LinkedListNode>();

        public string name;
        public object value;
        public superq parentSq;

        public string valueType;

        public bool buildFromStr;

        public superqelem(string name,
                          object value,
                          superq parentSq,
                          bool buildFromStr)
        {

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<LinkedListNode> GetEnumerator()
        {
            LinkedListNode node = list.head;
            while (node != null)
            {
                yield return node;
                node = node.next;
            }
        }

        private string toPyType(object variable)
        {
            return "str";
        }

        public string ToString()
        {
            string sqeStr = string.Format("{0},{1},{2},{3},{4};", toPyType(name),
                                                                  name,
                                                                  valueType,
                                                                  value,
                                                                  list.count);
            foreach (elematom atom in this)
            {
                string elemStr = string.Format("{0}|{1}|{2};", atom.name, atom.type, atom.value);
                sqeStr += string.Format("{0}|{1}", elemStr.Length, elemStr);
            }

            return sqeStr;
        }

        public void FromString(string sqe)
        {

        }
    }
}
