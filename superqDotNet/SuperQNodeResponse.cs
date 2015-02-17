using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class SuperQNodeResponse
    {
        public string msg_id { get; set; }
        public string result { get; set; }
        public string body { get; set; }

        public SuperQNodeResponse()
        {

        }

        public void from_str(string responseStr)
        {
            try
            {
                int headerSeparatorIdx = responseStr.IndexOf(";;");
            
                // separate out cmd header and body
                string responseHeader = responseStr.Substring(0, headerSeparatorIdx);
                string responseBody = responseStr.Substring(headerSeparatorIdx + 2);

                string[] elems = responseHeader.Split('|');

                if (elems.Count() != 2)
                    throw new Exception("Malformed network response (" + responseStr + ")");

                msg_id = elems[0];
                result = elems[1];
                body = responseBody;
            }
            catch (Exception e)
            {
                throw new Exception("Malformed network response (" + responseStr + "). Exception: " + e.Message);
            }
        }
    }
}
