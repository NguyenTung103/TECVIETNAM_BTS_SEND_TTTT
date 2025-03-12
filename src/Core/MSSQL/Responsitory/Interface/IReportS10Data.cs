﻿using bts.udpgateway;
using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using Core.Model.Report.ReportDay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.MSSQL.Responsitory.Interface
{
    public interface IReportS10Data : IDapperBaseData<ReportS10>
    {
        IEnumerable<ReportS10> GetByTime(DateTime from, DateTime to, int deviceId);
        ReportS10 GetLatest(int deviceId);
        List<DataReportDayEntity> GetReportByDay(DateTime fromDate, DateTime toDate, string sensortarget, string dsIdThietBi, int type);
        List<DataReportDayEntity> GetReportSumByTime(DateTime fromDate, DateTime toDate, string sensortarget, string dsIdThietBi, int type);
    }
}
