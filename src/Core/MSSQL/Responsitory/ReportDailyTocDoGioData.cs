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
    public class ReportDailyTocDoGioData : DapperBaseData<ReportDailyTocDoGio>, IReportDailyTocDoGioData
    {

        public ReportDailyTocDoGioData(IOptions<Connections> option) : base(option.Value.ConnectSqlString)
        {

        }
        public IEnumerable<ReportDailyTocDoGio> GetTocDoGioByTime(DateTime from, DateTime to)
        {
            string dateFrom = from.ToString("yyyy-MM-dd HH:mm:ss");
            string dateTo = from.ToString("yyyy-MM-dd HH:mm:ss");
            string query = string.Format(@"select * from ReportDailyTocDoGio 
                                            where DateRequestReport between '{0}' and '{1}' ", dateFrom, dateTo);
            return Query<ReportDailyTocDoGio>(query, null);
        }
        public ReportDailyTocDoGio GetTocDoGioLatest(int deviceId)
        {            
            string query = string.Format(@"select top 1 * from ReportDailyTocDoGio where DeviceId = {0}
                                            order by id desc", deviceId);
            return QueryFirstOrDefault<ReportDailyTocDoGio>(query, null);
        }
    }
}
