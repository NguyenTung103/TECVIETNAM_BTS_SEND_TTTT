using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using Core.Model.Report.ReportDay;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    public class ReportS10Data : DapperBaseData<ReportS10>, IReportS10Data
    {

        public ReportS10Data(IOptions<Connections> option) : base(option.Value.ConnectSqlString)
        {

        }
        public IEnumerable<ReportS10> GetByTime(DateTime from, DateTime to, int deviceId)
        {
            string dateFrom = from.ToString("yyyy-MM-dd HH:mm:ss");
            string dateTo = to.ToString("yyyy-MM-dd HH:mm:ss");
            string query = string.Format(@"select * from ReportS10 r
                                            join Site s on r.DeviceId = s.DeviceId
                                            where DateCreate between '{0}' and '{1}'  and r.DeviceId = {2} order by r.id desc", dateFrom, dateTo, deviceId);
            return Query<ReportS10>(query, null);
        }
        public ReportS10 GetLatest(int deviceId)
        {
            string query = string.Format(@"select top 1 * from ReportS10 where DeviceId = {0}
                                            order by id desc", deviceId);
            return QueryFirstOrDefault<ReportS10>(query, null);
        }
        public List<DataReportDayEntity> GetReportByDay(ReportDayRequestModel model)
        {
            List<DataReportDayEntity> result = new List<DataReportDayEntity>();
            string format = "dd-MM-yyyy", query = "";
            DateTime fromDate = DateTime.Today;
            DateTime toDate = DateTime.Now;            
            DynamicParameters listParameter = new DynamicParameters();
            if (!string.IsNullOrEmpty(model.fromDate))
            {
                fromDate = DateTime.ParseExact(model.fromDate, format, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(model.toDate))
            {
                toDate = DateTime.ParseExact(model.toDate, format, CultureInfo.InvariantCulture);
            }
            listParameter.Add("@FROM_DATE", fromDate);
            listParameter.Add("@TO_DATE", toDate);            
            query = string.Format(@"SELECT 
    main.DeviceId,
    tram.TypeSiteId,
    tram.Code_Group,
    CAST(DateCreate AS DATE) AS Date,
    CAST(AVG({0}) AS FLOAT) AS average, 
    CAST(MIN({0}) AS FLOAT) AS min, 
    CAST(MAX({0}) AS FLOAT) AS max, 
    CAST(SUM({0}) AS FLOAT) AS sum,
    (SELECT TOP 1 {0} 
     FROM ReportS10 AS sub
     WHERE sub.DeviceId = main.DeviceId  
     AND CAST(sub.DateCreate AS DATE) = CAST(main.DateCreate AS DATE)
     AND sub.DateCreate BETWEEN @FROM_DATE AND @TO_DATE
     ORDER BY sub.DateCreate DESC) AS last
FROM ReportS10 AS main
Join Site as tram on main.DeviceId = tram.DeviceId
WHERE DateCreate BETWEEN @FROM_DATE AND @TO_DATE ", model.sensorTarget);
            if (!string.IsNullOrEmpty(model.stationCodes))
            {                
                listParameter.Add("@DS_DeviceID", model.stationCodes.Split(","));
                query += "AND main.DeviceId IN (SELECT CAST(value AS INT) FROM STRING_SPLIT(@DS_DeviceID, ','))";
            }
            query += "GROUP BY main.DeviceId,Code_Group,TypeSiteId, CAST(DateCreate AS DATE) ORDER BY main.DeviceId, Date;";
            result = Query<DataReportDayEntity>(query, listParameter).ToList();
            return result;
        }
    }
}
