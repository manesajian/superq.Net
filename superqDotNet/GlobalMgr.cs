using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superqDotNet
{
    public static class GlobalMgr
    {
        private SuperQNetworkClientMgr _client = null;

        public SuperQNetworkClientMgr client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
            }
        }
    }
}
