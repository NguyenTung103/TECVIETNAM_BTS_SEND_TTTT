using bts.udpgateway;
using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.MSSQL.Responsitory.Interface
{
    public interface IReportDailyTocDoGioData : IDapperBaseData<ReportDailyTocDoGio>
    {
        IEnumerable<ReportDailyTocDoGio> GetTocDoGioByTime(DateTime from, DateTime to);
        ReportDailyTocDoGio GetTocDoGioLatest(int deviceId);
    }
}
