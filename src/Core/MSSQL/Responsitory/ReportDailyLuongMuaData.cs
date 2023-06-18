using bts.udpgateway;
using BtsGetwayService.Core;
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
    public class ReportDailyNhietDoData : DapperBaseData<ReportDailyNhietDo>, IReportDailyNhietDoData
{

        public ReportDailyNhietDoData(IOptions<Connections> option) : base(option.Value.ConnectSqlString)
        {

        }
    }
}
