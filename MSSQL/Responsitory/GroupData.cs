using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    public class GroupData : DapperBaseData<RegionalGroup>
    {

        public GroupData()
        {

        }
        public IEnumerable<RegionalGroup> GetGroupSend()
        {
            string query = string.Format(@"select * from RegionalGroup where IsSendTTTT=1");
            return Query<RegionalGroup>(query, null);
        }

    }
}
