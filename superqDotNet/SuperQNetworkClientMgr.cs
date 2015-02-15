using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class SuperQNetworkClientMgr
    {
        public SuperQNetworkClientMgr()
        {

        }

        private SuperQNodeResponse send_msg(string host, string request)
        {
            return null;
        }

        public bool superq_exists(string name, string host)
        {
            // build request object from string
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superq_exists";
            request.args = name;

            SuperQNodeResponse response = send_msg(host, request.ToString());

            return bool.Parse(response.result);
        }

        public void superq_create(superq sq)
        {

        }

        public superq superq_read(string name, string host)
        {
            return null;
        }

        public void superq_delete(superq sq)
        {

        }

        public superq superq_query(superq sq, string query)
        {
            return null;
        }

        public void superqelem_create(superq sq, superqelem sqe, int idx = -1)
        {

        }

        public void superqelem_update(superq sq, superqelem sqe)
        {

        }

        public void superqelem_delete(superq sq, string name)
        {

        }
    }
}
