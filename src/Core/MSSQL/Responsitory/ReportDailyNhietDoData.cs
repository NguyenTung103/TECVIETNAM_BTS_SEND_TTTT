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
    public class ReportDailyLuongMuaData : DapperBaseData<ReportDailyLuongMua>, IReportDailyLuongMuaData
    {

        public ReportDailyLuongMuaData(IOptions<Connections> option) : base(option.Value.ConnectSqlString)
        {

        }
    }
}
