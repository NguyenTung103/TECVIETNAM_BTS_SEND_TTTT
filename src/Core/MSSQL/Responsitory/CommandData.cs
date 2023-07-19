using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    public class CommandData : DapperBaseData<Commands>, ICommandData
    {
        public CommandData(IOptions<Connections> option) : base(option.Value.ConnectSqlString)
        {

        }
        public IEnumerable<Commands> GetAllCommandByDeviceId(int deviceid)
        {
            string query = string.Format(@"select * from Command where Device_Id={0} and Status IS NULL ", deviceid);
            return Query<Commands>(query, null);
        }

    }
}
