using bts.udpgateway;
using Core.Model.Report.ReportDay;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Udp
{
    public interface IReportS10Service
    {
        ReportS10 InitS10(string message);
        bool InsertS10(ReportS10 reportS10);
        IEnumerable<ReportS10> GetByTime(DateTime from, DateTime to, int deviceId, int? kieuTram = null, int? areaId = null, int? groupId = null);
        Task<List<ReportDayReponseModel>> GetReportByDay(DateTime fromDate, DateTime toDate, string sensortarget, string dsIdThietBi, int type);
        Task<List<ReportSumByTimeReponseModel>> GetReportSumByTime(DateTime fromDate, DateTime toDate, string sensortarget, string dsIdThietBi, int type);
    }
}
