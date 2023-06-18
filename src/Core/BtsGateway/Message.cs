using System;
using System.Collections.Generic;
using System.Text;

namespace bts.udpgateway.integration
{
    public class Message
    {
        public string command
        {
            get;
            set;
        }

        public string device_eqid
        {
            get;
            set;
        }

        public int id
        {
            get;
            set;
        }

        public int status
        {
            get;
            set;
        }

        //public DateTime time
        //{
        //    get;
        //    set;
        //}
    }
}
