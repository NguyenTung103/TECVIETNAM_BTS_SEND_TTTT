using System;
using System.Collections.Generic;
using System.Text;

namespace Core.BtsGateway
{
    public class BtsGatewayConfig
    {
        public int UdpPort { get; set; }        
        public bool ToFilesystem { get; set; }
        public bool ToDatabase { get; set; }
        public int DelayGetPending { get; set; }
    }
}
