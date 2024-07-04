using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace bts.udpgateway
{
    public class GroupData : DapperBaseData<RegionalGroup>, IGroupData
    {
        public GroupData(IOptions<Connections> option) : base(option.Value.ConnectSqlString)
        {

        }
        public IEnumerable<RegionalGroup> GetGroupSend()
        {
            string query = string.Format(@"select * from RegionalGroup where IsSendTTTT=1");
            return Query<RegionalGroup>(query, null);
        }
        public IEnumerable<RegionalGroup> GetAll()
        {
            string query = string.Format(@"select * from RegionalGroup");
            return Query<RegionalGroup>(query, null);
        }

    }
}
