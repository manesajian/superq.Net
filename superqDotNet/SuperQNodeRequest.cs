using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class SuperQNodeRequest
    {
        public string msg_id { get; set; }
        public string cmd { get; set; }
        public string args { get; set; }
        public string body { get; set; }

        private static int last_msg_id = 0;

        public SuperQNodeRequest()
        {
            msg_id = get_msg_id().ToString();
        }

        public int get_msg_id()
        {
            return Interlocked.Increment(ref last_msg_id);
        }
    }
}
