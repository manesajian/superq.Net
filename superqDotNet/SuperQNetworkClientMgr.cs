using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace superqDotNet
{
    public static class SuperQNetworkClientMgr
    {
        private const int DEFAULT_TCP_PORT = 9990;

        private static void send(Socket socket, byte[] buf)
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

        private static byte[] recv(Socket socket, int bytes)
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

            return buf;
        }

        private static SuperQNodeResponse get_msg(Socket socket)
        {
            // first byte will always be a marker to verify begining of Request
            byte[] data = recv(socket, 1);

            if (data.Count() != 1 || data[0] != 42)
                throw new Exception("Marker byte not read. Bad message.");

            // next 4 bytes must always be message body length
            data = recv(socket, 4);

            if (data.Count() != 4)
                throw new Exception("Msg length not read. Bad message.");

            // convert length
            int messageLength = BitConverter.ToInt32(data, 0);

            // now read the rest of the message
            data = recv(socket, messageLength);

            // decode character data
            string msg = Encoding.UTF8.GetString(data);

            // build response object from string
            SuperQNodeResponse response = new SuperQNodeResponse();
            response.from_str(msg);

            return response;
        }

        private static SuperQNodeResponse send_msg(string host, string msg)
        {
            // SSL support is not currently implemented
            bool ssl = false;

            int port = DEFAULT_TCP_PORT;

            // 'local' is shorthand for localhost:DEFAULT_PORT
            if (host == "local")
            {
                host = "localhost";
            }
            else
            {
                if (host.StartsWith("ssl:"))
                {
                    string[] elems = host.Split(':');

                    ssl = true;
                    host = elems[1];
                    port = Int32.Parse(elems[2]);
                }
                else
                {
                    try
                    {
                        string[] elems = host.Split(':');

                        host = elems[0];
                        port = Int32.Parse(elems[1]);
                    }
                    catch
                    {
                        port = DEFAULT_TCP_PORT;
                    }
                }
            }

            // allocate buffer
            byte[] buf = new byte[5 + msg.Count()];

            // set header byte
            buf[0] = 0x2A; // 42

            // add message length
            BitConverter.GetBytes(msg.Count()).CopyTo(buf, 1);

            // add message
            Encoding.UTF8.GetBytes(msg).CopyTo(buf, 5);     

            // open socket
            Socket socket = new TcpClient(host, port).Client;

            // send message
            send(socket, buf);

            // get response
            SuperQNodeResponse response = get_msg(socket);

            // close socket
            socket.Close();

            return response;
        }

        public static bool superq_exists(string name, string host)
        {
            // build request object from string
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superq_exists";
            request.args = name;

            SuperQNodeResponse response = send_msg(host, request.ToString());

            return bool.Parse(response.result);
        }

        public static void superq_create(superq sq)
        {
            // build request object from string
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superq_create";
            request.args = sq.publicName;
            request.body = sq.ToString();

            send_msg(sq.host, request.ToString());
        }

        public static superq superq_read(string name, string host)
        {
            // build request object from string
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superq_read";
            request.args = name;

            SuperQNodeResponse response = send_msg(host, request.ToString());

            if (bool.Parse(response.result) == false)
                throw new Exception(name + " does not exist.");

            // deserialize response body into a detached superq and return
            return new superq(response.body, "", "", "", false, true);
        }

        public static void superq_delete(string name, string host)
        {
            // build request object
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superq_delete";
            request.args = name;

            send_msg(host, request.ToString());
        }

        public static superq superq_query(superq sq, string query)
        {
            // build request object from string
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superq_query";
            request.args = sq.publicName;
            request.body = query;

            SuperQNodeResponse response = send_msg(sq.host, request.ToString());

            if (bool.Parse(response.result) == false)
                throw new Exception("Not sure what to raise here yet.");
            
            return new superq(response.body, "", "", "", false, true);
        }

        public static void superqelem_create(superq sq, superqelem sqe, int idx = -1)
        {
            // build request object
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superqelem_create";
            request.args = sq.publicName + "," + idx.ToString();
            request.body = sqe.ToString();

            send_msg(sq.host, request.ToString());
        }

        public static void superqelem_update(superq sq, superqelem sqe)
        {
            // build request object
            SuperQNodeRequest request = new SuperQNodeRequest();
            request.cmd = "superqelem_update";
            request.args = sq.publicName;
            request.body = sqe.ToString();

            send_msg(sq.host, request.ToString());
        }

        public static void superqelem_delete(superq sq, string name)
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
