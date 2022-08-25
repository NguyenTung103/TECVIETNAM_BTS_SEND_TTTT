using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    public class ReportDailyHuongGioData : DapperBaseData<ReportDailyHuongGio>, IReportDailyHuongGioData
    {

        public ReportDailyHuongGioData(IOptions<Connections> option) : base(option.Value.ConnectSqlString)
        {

        }
        public IEnumerable<ReportDailyHuongGio> GetHuongGioByTime(DateTime from, DateTime to)
        {
            string dateFrom = from.ToString("yyyy-MM-dd HH:mm:ss");
            string dateTo = from.ToString("yyyy-MM-dd HH:mm:ss");
            string query = string.Format(@"select * from ReportDailyHuongGio 
                                            where DateRequestReport between '{0}' and '{1}' ", dateFrom, dateTo);
            return Query<ReportDailyHuongGio>(query, null);
        }
        public ReportDailyTocDoGio GetHuongGioLatest(int deviceId)
        {
            string query = string.Format(@"select top 1 * from ReportDailyHuongGio where DeviceId = {0}
                                            order by id desc", deviceId);
            return QueryFirstOrDefault<ReportDailyTocDoGio>(query, null);
        }
    }
}
