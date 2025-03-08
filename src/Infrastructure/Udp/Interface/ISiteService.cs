using bts.udpgateway;
using bts.udpgateway.integration;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Core.Model.TramModels;

namespace Infrastructure.Udp
{
    public interface ISiteService
    {
        Task<List<SiteModel>> GetDanhSachTram(List<string> danh_sach_tram);
    }
}
