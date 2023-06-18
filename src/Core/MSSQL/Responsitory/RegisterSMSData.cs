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
    public class RegisterSMSData : DapperBaseData<RegisterSMS>, IRegisterSMSData
    {

        public RegisterSMSData(IOptions<Connections> option) : base(option.Value.ConnectSqlString)
        {

        }
        public IEnumerable<RegisterSMS> GetRegisterSMS(int deviceid)
        {
            string query = string.Format(@"select *from RegisterSMS where DeviceId={0}", deviceid);
            return Query<RegisterSMS>(query, null);
        }

    }
}
