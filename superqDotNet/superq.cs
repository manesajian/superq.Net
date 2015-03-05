using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class superq : IEnumerable<superqelem>
    {
        public LinkedList<superqelem> list = new LinkedList<superqelem>();
        public Dictionary<dynamic, superqelem> dict = new Dictionary<dynamic, superqelem>();

        public string name;
        public string host;
        public string publicName;

        public string keyCol;
        public int maxlen;
        public bool autoKey;

        public bool attached = false;

        public superq(dynamic obj,
                      string name,
                      string host,
                      string keyCol,
                      bool attach,
                      bool buildFromStr = false)
        {
            this.name = name;
            this.host = host;
            this.keyCol = keyCol;

            publicName = "";
            maxlen = 0;
            autoKey = false;

            if (obj.GetType().IsArray)
                foreach (dynamic elem in (Array)obj)
                    CreateElem(elem);

            attached = false;
        }

        static public superq Create(dynamic obj, string keyCol = "")
        {
            string name = Guid.NewGuid().ToString();

            return new superq(obj, name, string.Empty, keyCol, false, false);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<superqelem> GetEnumerator()
        {
            superqelem node = (superqelem)list.head;
            while (node != null)
            {
                yield return node;
                node = (superqelem)node.next;
            }
        }

        private dynamic getitem(dynamic val)
        {
            if (dict.ContainsKey(val))
                return dict[val].value;
            else if (val is Int32)
                return list[val].value;

            throw new Exception("Couldn't getitem().");
        }

        public object this[dynamic val]
        {
            get { return getitem(val); }
            set { this.GetType().GetProperty(val).SetValue(this, value, null); }
        }

        private void attach()
        {
            //if self.attached:
            //    raise Exception('Already attached!')

            //if self.dataStore.superq_exists(self.name):
            //    raise NotImplemented('Not yet allowed to attach existing superqs.')

            attached = true;

            //self.dataStore.superq_create(self)

            //# create each elem. The 1st one triggers backing table creation
            //if len(self) > 0:
            //    # add each superqelem now
            //    for name, sqe in self.__internalDict.items():
            //        self.create_elem_datastore_only(sqe);
        }

        public override string ToString()
        {
            string sqHdr = name + "," + list.count + ";";

            // serialize necessary attributes as name-value pairs
            string sqAttrs = "";
            sqAttrs += string.Format("name|{0},", name);
            sqAttrs += string.Format("host|{0},", host);
            sqAttrs += string.Format("keyCol|{0},", keyCol);
            sqAttrs += string.Format("maxlen|{0},", maxlen);
            sqAttrs += string.Format("autoKey|{0}", autoKey);
            sqAttrs += ";";

            string sqElems = "";
            foreach (LinkedListNode sqe in list)
            {
                string sqeStr = sqe.ToString();
                sqElems += string.Format("{0},{1}", sqeStr.Length, sqeStr);
            }

            string sqStr = string.Format("{0}{1}{2}", sqHdr, sqAttrs, sqElems);

            return sqStr;
        }

        public void FromString(string sqStr, bool attach = false)
        {
            // initialize internal storage
            list = new LinkedList<superqelem>();
            dict = new Dictionary<dynamic, superqelem>();

            // separate out sq header from remainder
            int headerSeparatorIdx = sqStr.IndexOf(';');
            string sqHeader = sqStr.Substring(0, headerSeparatorIdx);
            sqStr = sqStr.Substring(headerSeparatorIdx + 1);

            // get name and number of fields from sq header
            string[] headerElems = sqHeader.Split(',');
            name = headerElems[0];
            int numSqes = Int32.Parse(headerElems[1]);

            // separate out attributes from remainder
            headerSeparatorIdx = sqStr.IndexOf(';');
            string sqAttrs = sqStr.Substring(0, headerSeparatorIdx);
            sqStr = sqStr.Substring(headerSeparatorIdx + 1);

            // set attributes
            string[] attrElems = sqAttrs.Split(',');
            foreach (string attr in attrElems)
            {
                string[] elems = attr.Split('|');
                string attrName = elems[0];
                string attrValue = elems[1];

                if (attrValue.StartsWith("None"))
                    attrValue = null;

                this[attrName] = attrValue;
            }

            if (attach)
                this.attach();

            // parse out each superqelem
            for (int i = 0; i < numSqes; ++i)
            {
                // separate field length indicator from remainder
                int separatorIdx = sqStr.IndexOf(',');
                int elemLen = Int32.Parse(sqStr.Substring(0, separatorIdx));
                sqStr = sqStr.Substring(separatorIdx + 1);

                // slice the rest of the sqe out
                string sqeStr = sqStr.Substring(0, elemLen);
                sqStr = sqStr.Substring(elemLen);

                // deserialize sqe from string fragment
                superqelem sqe = new superqelem(sqeStr, null, this, true);

                // add element to internal dictionary and tail of internal list
                dict[sqe.name] = sqe;
                list.push_tail(sqe);
            }
        }

        private superqelem wrap_elem(object obj)
        {
            return null;
        }

        private object unwrap_elem(superqelem sqe)
        {
            return null;
        }

        public void CreateElem(dynamic obj)
        {
            superqelem sqe = obj as superqelem;
            if (sqe == null)
            {
                sqe = new superqelem("", obj, this, false);
            }

            list.push_tail(sqe);
            dict[sqe.name] = sqe;
        }

        public void UpdateElem(dynamic obj)
        {

        }

        private void DeleteElemDatastore(superqelem sqe)
        {

        }

        public void DeleteElem(dynamic obj)
        {

        }

        public void push(dynamic val, int idx = -1, bool block = false, int timeout = -1)
        {

        }

        public void push_head(dynamic val, bool block = true, int timeout = -1)
        {

        }

        public void push_tail(dynamic val, bool block = true, int timeout = -1)
        {

        }

        public dynamic pop(int idx = -1, bool block = true, int timeout = -1)
        {
            // TODO: still need to add synchronization
            if (!block)
            {
                if (list.count == 0)
                    throw new Exception("No elements in superq.");
            }
            else if (timeout == -1)
            {
                while (list.count == 0)
                {
                    timeout = -1; // TODO: fix this bogus statement with wait
                }
            }
            else if (timeout < 0)
                throw new Exception("Invalid timeout value.");
            else
            {
                DateTime endTime = DateTime.Now.AddSeconds(timeout);
                while (list.count == 0)
                {
                    if (DateTime.Now > endTime)
                        throw new Exception("No elements in superq.");
                    timeout = timeout; // TODO: fix this bogus statement with wait
                }
            }

            // Default to stack/LIFO behavior
            if (idx == -1)
                idx = list.count - 1;

            // Remove element from internal collections
            superqelem sqe = list.pop(idx);
            dict.Remove(sqe.name);

            // For now pops on hosted superqs are slow due to blocking here
            if (attached)
                DeleteElemDatastore(sqe);

            // TODO: Look at python version for condition wakeup here

            return unwrap_elem(sqe);
        }

        public dynamic pop_head(bool block = true, int timeout = -1)
        {
            return pop(0, block, timeout);
        }

        public dynamic pop_tail(bool block = true, int timeout = -1)
        {
            return pop(list.count - 1, block, timeout);
        }

        public void rotate(int n)
        {
            // Iterate to the indicated index
            if (n >= 0)
                for (int i = 0; i < n; ++i)
                    push_head(pop_tail());
            else
                for (int i = 1; i < Math.Abs(n))
                    push_tail(pop_head());
        }
    }
}
