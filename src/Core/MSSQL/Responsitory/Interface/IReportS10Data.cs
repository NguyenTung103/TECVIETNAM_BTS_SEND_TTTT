using bts.udpgateway;
using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.MSSQL.Responsitory.Interface
{
    public interface IReportS10Data : IDapperBaseData<ReportS10>
    {
        IEnumerable<ReportS10> GetByTime(DateTime from, DateTime to, int deviceId);
        ReportS10 GetLatest(int deviceId);
    }
}
