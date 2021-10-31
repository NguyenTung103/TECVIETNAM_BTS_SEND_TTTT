using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    public class SiteData : DapperBaseData<Site>
    {

        public SiteData()
        {

        }
        public IEnumerable<Site> GetListSite(int groupId)
        {
            DynamicParameters listParameter = new DynamicParameters();
            listParameter.Add("@Group_ID", groupId);
            string query = string.Format(@"select * from Site where Group_Id=@Group_ID and IsActive=1", groupId);
            return Query<Site>(query, listParameter);
        }

    }
}
