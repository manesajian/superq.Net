using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class superqelem : LinkedListNode, IEnumerable<elematom>
    {
        // elematom list
        private LinkedList<elematom> list = new LinkedList<elematom>();

        // elematom dict, keyed by field name
        private Dictionary<string, LinkedListNode> dict = new Dictionary<string, LinkedListNode>();

        public object name;
        public dynamic value;
        public superq parentSq;

        public string valueType;

        public superqelem(string name,
                          dynamic value,
                          superq parentSq,
                          bool buildFromStr)
        {
            this.name = name;
            this.value = value;
            this.parentSq = parentSq;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<elematom> GetEnumerator()
        {
            elematom node = (elematom)list.head;
            while (node != null)
            {
                yield return node;
                node = (elematom)node.next;
            }
        }

        private string toPyType(object variable)
        {
            return "str";
        }

        public override string ToString()
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

        public void FromString(string sqeStr)
        {
            int headerSeparatorIdx = sqeStr.IndexOf(';');

            // separate out sqe header from remainder
            string sqeHeader = sqeStr.Substring(0, headerSeparatorIdx);
            string sqeBody = sqeStr.Substring(headerSeparatorIdx + 1);

            // parse out header fields
            string[] headerElems = sqeHeader.Split(',');

            // name type and name  
            string nameType = headerElems[0];
            if (nameType.StartsWith("str"))
                name = headerElems[1];
            else if (nameType.StartsWith("int"))
                name = Int32.Parse(headerElems[1]);
            else if (nameType.StartsWith("float"))
                name = float.Parse(headerElems[1]);

            // value type and value          
            string valueType = headerElems[2];
            if (valueType.StartsWith("str"))
                value = headerElems[3];
            else if (valueType.StartsWith("int"))
                value = Int32.Parse(headerElems[3]);
            else if (valueType.StartsWith("float"))
                value = float.Parse(headerElems[3]);

            // if it's not empty, i.e. it is a scalar sqe, we're done parsing
            if (!string.IsNullOrEmpty(valueType))
                return;

            // only scalar superqelems should use value
            value = null;

            // number of fields or atoms
            int numFields = Int32.Parse(headerElems[4]);

            // parse out each field
            for (int i = 0; i < numFields; ++i)
            {
                // separate field length indicator from remainder
                int separatorIdx = sqeBody.IndexOf('|');
                int fieldLen = Int32.Parse(sqeBody.Substring(0, separatorIdx));
                sqeBody = sqeBody.Substring(separatorIdx + 1);

                // slice the rest of the field out
                string field = sqeBody.Substring(0, fieldLen - 1);
                sqeBody = sqeBody.Substring(fieldLen);

                // slice field name from field
                separatorIdx = field.IndexOf('|');
                string fieldName = field.Substring(0, separatorIdx);
                field = field.Substring(separatorIdx + 1);

                // now retrieve type and value
                separatorIdx = field.IndexOf('|');
                string fieldType = field.Substring(0, separatorIdx);

                string fieldStr = field.Substring(separatorIdx + 1);
                object fieldValue = fieldStr;
                if (fieldType.StartsWith("int"))
                    fieldValue = Int32.Parse(fieldStr);
                else if (fieldType.StartsWith("float"))
                    fieldValue = float.Parse(fieldStr);

                add_atom(fieldName, fieldType, fieldValue);
            }
        }

        private void add_atom(string name, string type, object value)
        {
            elematom atom = new elematom(name, type, value);

            dict[name] = atom;
            list.push_tail(atom);
        }
    }
}
