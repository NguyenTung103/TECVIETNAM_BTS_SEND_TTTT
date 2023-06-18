using BtsGetwayService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtsGetwayService.Interface
{
    public interface IDataObservationService
    {
        List<BtsGetwayService.MongoDb.Entity.Data> GetDataByDay(out int totalRow);
        List<BtsGetwayService.MongoDb.Entity.Data> GetDataPaging(DateTime fromDate, DateTime toDate, int skip, int limit, out int total);
        List<BtsGetwayService.MongoDb.Entity.Data> GetDataPagingByDeviceId(DateTime fromDate, DateTime toDate, int deviceId, int skip, int limit, out int total);
        Task<IEnumerable<BtsGetwayService.MongoDb.Entity.Data>> GetDatayDeviceId(DateTime fromDate, DateTime toDate, int deviceId);
        List<BtsGetwayService.MongoDb.Entity.Data> GetOffline(int deviceId, int skip, int limit, out int total);
        List<BtsGetwayService.MongoDb.Entity.Data> GetByDeviceId(int deviceId, int skip, int limit, out int total);
        bool Insert(BtsGetwayService.MongoDb.Entity.Data data);
    }
}
