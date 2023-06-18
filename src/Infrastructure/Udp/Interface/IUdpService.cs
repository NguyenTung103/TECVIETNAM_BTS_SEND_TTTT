using bts.udpgateway;
using bts.udpgateway.integration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Udp
{
    public interface IUdpService
    {
        Task<ReturnInfo> Insert(string msg_content);
    }
}
