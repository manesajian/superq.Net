﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class superq
    {
        public string publicName { get; set; }
        public string host { get; set; }

        public superq(object initObj,
                      string name,
                      string host,
                      bool attach,
                      bool buildFromStr = false)
        {

        }

        public string ToString()
        {
            return "";
        }
    }
}
