using bts.udpgateway;
using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.MSSQL.Responsitory.Interface
{
    public interface IReportDailyHuongGioData : IDapperBaseData<ReportDailyHuongGio>
    {
        IEnumerable<ReportDailyHuongGio> GetHuongGioByTime(DateTime from, DateTime to);
        ReportDailyTocDoGio GetHuongGioLatest(int deviceId);
    }
}
