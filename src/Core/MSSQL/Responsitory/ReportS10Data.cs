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
    public class ReportS10Data : DapperBaseData<ReportS10>, IReportS10Data
    {

        public ReportS10Data(IOptions<Connections> option) : base(option.Value.ConnectSqlString)
        {

        }
        public IEnumerable<ReportS10> GetByTime(DateTime from, DateTime to)
        {
            string dateFrom = from.ToString("yyyy-MM-dd HH:mm:ss");
            string dateTo = from.ToString("yyyy-MM-dd HH:mm:ss");
            string query = string.Format(@"select * from ReportS10 r
                                            join Site s on r.DeviceId = s.DeviceId
                                            where DateCreate between '{0}' and '{1}' and s.IsActive=1 ", dateFrom, dateTo);
            return Query<ReportS10>(query, null);
        }
        public ReportS10 GetLatest(int deviceId)
        {
            string query = string.Format(@"select top 1 * from ReportS10 where DeviceId = {0}
                                            order by id desc", deviceId);
            return QueryFirstOrDefault<ReportS10>(query, null);
        }
    }
}
