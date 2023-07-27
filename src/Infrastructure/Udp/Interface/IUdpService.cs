using bts.udpgateway;
using bts.udpgateway.integration;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Udp
{
    public interface IUdpService
    {
        Task<ReturnInfo> Insert(string msg_content);
        ReturnInfo SendCommand(string message, IPEndPoint endPoint, UdpClient udpClient);
        void CheckDeviceS10();
    }
}
