using bts.udpgateway;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Udp
{
    public interface IReportDailyService
    {
        void ReportDaily(string message);        
    }
}
