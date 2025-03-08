using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    public class SiteData : DapperBaseData<Site>, ISiteData
    {

        public SiteData(IOptions<Connections> option) : base(option.Value.ConnectSqlString)
        {

        }
        public IEnumerable<Site> GetSite(int deviceid)
        {
            string query = string.Format(@"select Name from Site where DeviceId={0}", deviceid);
            return Query<Site>(query, null);
        }
        public void UpdateStatusActive(List<int> lstDeviceid)
        {           
            if (lstDeviceid != null && lstDeviceid.Count() > 0)
            {
                var strDevice = string.Join(",", lstDeviceid);
                var strDeviceQuery = strDevice.Remove(strDevice.Length - 1, 1);
                string query = string.Format(@"update site set IsActive=1 where DeviceId in({0})", strDevice);              
                Execute(query, null);
            }

        }
        public void UpdateStatusActive(int deviceid)
        {
            string query = string.Format(@"update site set IsActive=0 where DeviceId ={0}", deviceid);
            Execute(query, null);
        }
        public void UpdateStatusDisable(List<int> lstDeviceid)
        {     
            if (lstDeviceid != null && lstDeviceid.Count() > 0)
            {
                var strDevice = string.Join(",", lstDeviceid);
                var strDeviceQuery = strDevice.Remove(strDevice.Length - 1, 1);
                string query = string.Format(@"update site set IsActive=0 where DeviceId in({0})", strDevice);               
                Execute(query, null);
            }

        }
        public IEnumerable<Site> GetListSite(int groupId)
        {
            DynamicParameters listParameter = new DynamicParameters();
            listParameter.Add("@Group_ID", groupId);
            string query = string.Format(@"select * from Site where Group_Id=@Group_ID and IsActive=1", groupId);
            return Query<Site>(query, listParameter);
        }
        public async Task<IEnumerable<Site>> GetDsSite(List<int> dsDeviceId)
        {
            DynamicParameters listParameter = new DynamicParameters();
            string query = string.Format(@"select * from Site s left join RegionalGroup rg on s.Group_Id = rg.Id");
            if (dsDeviceId.Any())
            {
                query += "where s.DeviceId IN(@DS_DeviceId)";
                listParameter.Add("@DS_DeviceId", dsDeviceId);
            }
            
            return await QueryAsync<Site>(query, listParameter);
        }
        public async Task<Site> GetSiteByDeviceId(int deviceId)
        {
            DynamicParameters listParameter = new DynamicParameters();
            listParameter.Add("@DeviceID", deviceId);
            string query = string.Format(@"select * from Site where DeviceId=@DeviceID and IsActive=1");
            return await QueryFirstOrDefaultAsync<Site>(query, listParameter);
        }
    }
}
