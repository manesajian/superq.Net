using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class superq
    {
        private LinkedList<superqelem> list = new LinkedList<superqelem>();
        private Dictionary<object, superqelem> dict = new Dictionary<object, superqelem>();

        public string name;
        public string host;
        public string publicName;

        public string keyCol;
        public int maxlen;
        public bool autoKey;

        public bool attached = false;

        public superq(object initObj,
                      string name,
                      string host,
                      bool attach,
                      bool buildFromStr = false)
        {
            this.name = name;
            this.host = host;

            publicName = "";
            keyCol = "";
            maxlen = 0;
            autoKey = false;

            attached = false;
        }

        private object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
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

        public string ToString()
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
            dict = new Dictionary<object, superqelem>();

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
    }
}
