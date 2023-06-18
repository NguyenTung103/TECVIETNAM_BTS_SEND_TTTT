using bts.udpgateway;
using BtsGetwayService.Core;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{    
    public class ObservationData : DapperBaseData<Observation>, IObservationData
{

        public ObservationData(IOptions<Connections> option) : base(option.Value.ConnectSqlString)
        {

        }
        public Observation GetByCode(string code)
        {
            DynamicParameters listParameter = new DynamicParameters();
            listParameter.Add("@Code", code);
            string query = string.Format(@"select * from Observation where Code=@Code");
            return QueryFirstOrDefault<Observation>(query, listParameter);
        }
    }
}
