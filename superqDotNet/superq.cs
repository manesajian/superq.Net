using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class superq
    {
        public string name { get; set; }
        public string host { get; set; }
        public string publicName { get; set; }

        public string keyCol { get; set; }
        public int maxlen { get; set; }
        public bool autoKey { get; set; }

        private LinkedList<superqelem> list = new LinkedList<superqelem>();
        private Dictionary<string, LinkedListNode> dict = new Dictionary<string, LinkedListNode>();

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

        public void FromString()
        {
            //# initialize internal storage
            //self.__internalList = LinkedList()
            //self.__internalDict = {}

            //# separate out sq header from remainder
            //headerSeparatorIdx = sqStr.index(';')
            //sqHeader = sqStr[ : headerSeparatorIdx]
            //sqStr = sqStr[headerSeparatorIdx + 1 : ]

            //# get name and number of fields from sq header
            //headerElems = sqHeader.split(',')
            //self.name = headerElems[0]
            //numSqes = int(headerElems[1])

            //# separate out attributes from remainder
            //headerSeparatorIdx = sqStr.index(';')
            //sqAttrs = sqStr[ : headerSeparatorIdx]
            //sqStr = sqStr[headerSeparatorIdx + 1 : ]

            //# set attributes
            //attrElems = sqAttrs.split(',')
            //for attr in attrElems:
            //    name, value = attr.split('|')

            //    if value.startswith('None'):
            //        value = None

            //    setattr(self, name, value)

            //if attach:
            //    self.attach()

            //# parse out each superqelem
            //for i in range(0, numSqes):
            //    # separate field length indicator from remainder
            //    separatorIdx = sqStr.index(',')
            //    elemLen = int(sqStr[ : separatorIdx])
            //    sqStr = sqStr[separatorIdx + 1 : ]

            //    # slice the rest of the sqe out
            //    sqeStr = sqStr[ : elemLen]
            //    sqStr = sqStr[elemLen : ]

            //    # deserialize sqe from string fragment
            //    sqe = superqelem(sqeStr, parentSq = self, buildFromStr = True)

            //    # add element to internal dictionary and tail of internal list
            //    self.__internalDict[sqe.name] = sqe
            //    self.__internalList.push_tail(sqe)
        }
    }
}
