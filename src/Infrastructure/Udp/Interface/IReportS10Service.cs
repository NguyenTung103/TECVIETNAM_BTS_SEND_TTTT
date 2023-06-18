using bts.udpgateway;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Udp
{
    public interface IReportS10Service
    {
        ReportS10 InitS10(string message);
        bool InsertS10(ReportS10 reportS10);
    }
}
