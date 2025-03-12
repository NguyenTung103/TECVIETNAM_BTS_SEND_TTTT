using bts.udpgateway;
using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.MSSQL.Responsitory.Interface
{
    public interface ISiteData : IDapperBaseData<Site>
    {
        IEnumerable<Site> GetListSite(int groupId);
        Task<IEnumerable<Site>> GetDsSite(List<int> dsDeviceId);
        Task<Site> GetSiteByDeviceId(int deviceId);
        IEnumerable<Site> GetSite(int deviceid);
        void UpdateStatusActive(int deviceid);
        void UpdateStatusActive(List<int> lstDeviceid);
        void UpdateStatusDisable(List<int> lstDeviceid);
        Task<Site> GetByDeviceId(int deviceId);
    }
}
