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
        IEnumerable<ReportS10> GetByTime(DateTime from, DateTime to, int deviceId, int? kieuTram = null, int? areaId = null, int? groupId = null);
    }
}
