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
        public List<DataReportDayEntity> GetReportByDay(DateTime fromDate, DateTime toDate, string sensortarget, string dsIdThietBi, int type)
        {
            List<DataReportDayEntity> result = new List<DataReportDayEntity>();
            DynamicParameters listParameter = new DynamicParameters();
            listParameter.Add("@FROM_DATE", fromDate);
            listParameter.Add("@TO_DATE", toDate);
            string condition = string.Empty;
            if (!string.IsNullOrEmpty(dsIdThietBi))
            {
                listParameter.Add("@DS_DeviceID", dsIdThietBi.Split(","));
                condition += "AND main.DeviceId IN (SELECT CAST(value AS INT) FROM STRING_SPLIT(@DS_DeviceID, ','))";
            }
            string query = string.Format(@"WITH GroupedData AS (
    SELECT  
        main.DeviceId,
        tram.TypeSiteId,
        tram.Code_Group,
        DATEADD(DAY, (DATEDIFF(DAY, '2000-01-01', DateCreate) / {1}) * {1}, '2000-01-01') AS TimeSlot,
        main.DateCreate,
        main.{0},
        LAST_VALUE(main.{0}) OVER (
            PARTITION BY main.DeviceId, 
                         DATEADD(DAY, (DATEDIFF(DAY, '2000-01-01', main.DateCreate) / {1}) * {1}, '2000-01-01')
            ORDER BY main.DateCreate 
            ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING
        ) AS last
    FROM ReportS10 AS main
    JOIN Site AS tram ON main.DeviceId = tram.DeviceId
    WHERE main.DateCreate BETWEEN @FROM_DATE AND @TO_DATE {2}
)
SELECT  
    DeviceId,
    TypeSiteId,
    Code_Group,
    TimeSlot,
    DATEADD(DAY, {1}, TimeSlot) AS EndDate,
    CAST(AVG({0}) AS FLOAT) AS average, 
    CAST(MIN({0}) AS FLOAT) AS min, 
    CAST(MAX({0}) AS FLOAT) AS max, 
    CAST(SUM({0}) AS FLOAT) AS sum,
    MAX(last) AS last -- Vì LAST_VALUE có thể trùng giá trị nhiều dòng nên dùng MAX để lấy giá trị duy nhất
FROM GroupedData
GROUP BY DeviceId, TypeSiteId, Code_Group, TimeSlot
ORDER BY DeviceId, TimeSlot;

 ", sensortarget, type, condition);           
            result = Query<DataReportDayEntity>(query, listParameter).ToList();
            return result;
        }
        public List<DataReportDayEntity> GetReportSumByTime(DateTime fromDate, DateTime toDate, string sensortarget, string dsIdThietBi, int type)
        {
            List<DataReportDayEntity> result = new List<DataReportDayEntity>();
            DynamicParameters listParameter = new DynamicParameters();
            listParameter.Add("@FROM_DATE", fromDate);
            listParameter.Add("@TO_DATE", toDate);
            string query = string.Format(@"SELECT 
    main.DeviceId,
    tram.TypeSiteId,
    tram.Code_Group,
    DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DateCreate) / 5) * {1}, 0) AS TimeSlot,  
    CAST(SUM({0}) AS FLOAT) AS sum   
FROM ReportS10 AS main
JOIN Site AS tram ON main.DeviceId = tram.DeviceId
WHERE DateCreate BETWEEN @FROM_DATE AND @TO_DATE
 ", sensortarget, type);
            if (!string.IsNullOrEmpty(dsIdThietBi))
            {
                listParameter.Add("@DS_DeviceID", dsIdThietBi.Split(","));
                query += "AND main.DeviceId IN (SELECT CAST(value AS INT) FROM STRING_SPLIT(@DS_DeviceID, ','))";
            }
            query += string.Format(@" GROUP BY main.DeviceId, tram.Code_Group, tram.TypeSiteId, 
         DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DateCreate) / 5) * {0}, 0) ORDER BY main.DeviceId, TimeSlot;", type);
            result = Query<DataReportDayEntity>(query, listParameter).ToList();
            return result;
        }
    }
}
