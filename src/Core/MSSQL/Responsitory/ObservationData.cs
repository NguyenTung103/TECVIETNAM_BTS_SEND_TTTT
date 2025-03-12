using bts.udpgateway;
using BtsGetwayService.Core;
using Core.Model.ObservationModel;
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
        public async Task<List<Observation>> GetAll(int? type = null)
        {
            string query = string.Format(@"select * from Observation");
            DynamicParameters listParameter = new DynamicParameters();            
            if (type.HasValue)
            {
                query += " where CategoryTypeSite=@CategoryTypeSite";
                listParameter.Add("@CategoryTypeSite", type);
            }
            return (await QueryAsync<Observation>(query, listParameter)).ToList();
        }
        public async Task<List<ObservationReponseModel>> GetObservationByDeviceId(List<int>? deviceId = null)
        {
            string query = string.Format(@"SELECT 
    s.DeviceId AS station_code,
    COALESCE(
        NULLIF(s.Code_Group, 'MUA_HOA_BINH'), 
        N'AQRLG' + CAST(s.DeviceId AS NVARCHAR)
    ) AS sensor_target,
    t.Name AS sensor_name,
    '1' AS status 
FROM Site s
JOIN CategoryTypeSite t ON s.TypeSiteId = t.Id
");
            DynamicParameters listParameter = new DynamicParameters();
            if (deviceId.Any())
            {
                query += " where s.DeviceId IN (SELECT CAST(value AS INT) FROM STRING_SPLIT(@DS_DeviceID, ','))";
                listParameter.Add("@DS_DeviceID", deviceId);
            }
            return (await QueryAsync<ObservationReponseModel>(query, listParameter)).ToList();
        }
    }
}
