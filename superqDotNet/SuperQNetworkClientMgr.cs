using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class SuperQNetworkClientMgr
    {
        public SuperQNetworkClientMgr()
        {

        }

        public void send(Socket socket, byte[] buf)
        {
            int sent = 0;
            do
            {
                try
                {
                    sent += socket.Send(buf, sent, buf.Count() - sent, SocketFlags.None);
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.WouldBlock ||
                        e.SocketErrorCode == SocketError.IOPending ||
                        e.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // buffer may be full. Wait and try again
                        Thread.Sleep(10);
                    }
                    else
                        throw e;
                }

            } while (sent < buf.Count());
        }

        public void recv(Socket socket, int bytes)
        {
            byte[] buf = new byte[bytes];

            int received = 0;
            do
            {
                try
                {
                    received += socket.Receive(buf, received, bytes - received, SocketFlags.None);
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.WouldBlock ||
                        e.SocketErrorCode == SocketError.IOPending ||
                        e.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // buffer may be empty. Wait and try again
                        Thread.Sleep(10);
                    }
                    else
                        throw e;
                }
            } while (received < bytes);
        }

        private SuperQNodeResponse get_msg(Socket socket)
        {
            return null;
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
            // build request object from string
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superq_create";
            request.args = sq.publicName;
            request.body = sq.ToString();

            send_msg(sq.host, request.ToString());
        }

        public superq superq_read(string name, string host)
        {
            // build request object from string
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superq_read";
            request.args = name;

            SuperQNodeResponse response = send_msg(host, request.ToString());

            if (bool.Parse(response.result) == false)
                throw new Exception(name + " does not exist.");

            // deserialize response body into a detached superq and return
            return new superq(response.body, "", "", false, true);
        }

        public void superq_delete(superq sq)
        {
            // build request object
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superq_delete";
            request.args = sq.publicName;

            send_msg(sq.host, request.ToString());
        }

        public superq superq_query(superq sq, string query)
        {
            // build request object from string
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superq_query";
            request.args = sq.publicName;
            request.body = query;

            SuperQNodeResponse response = send_msg(sq.host, request.ToString());

            if (bool.Parse(response.result) == false)
                throw new Exception("Not sure what to raise here yet.");
            
            return new superq(response.body, "", "", false, true);
        }

        public void superqelem_create(superq sq, superqelem sqe, int idx = -1)
        {
            // build request object
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superqelem_create";
            request.args = sq.publicName + "," + idx.ToString();
            request.body = sqe.ToString();

            send_msg(sq.host, request.ToString());
        }

        public void superqelem_update(superq sq, superqelem sqe)
        {
            // build request object
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superqelem_update";
            request.args = sq.publicName;
            request.body = sqe.ToString();

            send_msg(sq.host, request.ToString());
        }

        public void superqelem_delete(superq sq, string name)
        {
            // build request object
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superqelem_delete";
            request.args = sq.publicName;
            request.body = name;

            send_msg(sq.host, request.ToString());
        }
    }
}
