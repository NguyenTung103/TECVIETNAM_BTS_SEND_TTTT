using ES_CapDien.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BtsGetwayService.MongoDb.Entity;

namespace BtsGetwayService.Service
{
    public class DataObservationMongoService
    {
        DataObservationMongoData data = new DataObservationMongoData();
        public List<BtsGetwayService.MongoDb.Entity.Data> GetDataByDay(out int totalRow)
        {
            List<BtsGetwayService.MongoDb.Entity.Data> list = new List<BtsGetwayService.MongoDb.Entity.Data>();
            DateTime from = DateTime.Today;
            DateTime to = DateTime.Today.AddDays(1);
            list = data.FindAll(i => i.DateCreate < to && i.DateCreate > from).ToList();
            totalRow = list.Count();            
            return list;
        }
        public List<BtsGetwayService.MongoDb.Entity.Data> GetDataPaging(DateTime fromDate,DateTime toDate, int skip, int limit, out int total)
        {
            List<BtsGetwayService.MongoDb.Entity.Data> list = new List<BtsGetwayService.MongoDb.Entity.Data>();           
            list = data.FindPagingOption(i => i.DateCreate < toDate && i.DateCreate > fromDate, limit, skip,out long totalRow).OrderByDescending(i => i.DateCreate).ToList();
            total = Convert.ToInt32(totalRow);
            return list;
        }
        public List<BtsGetwayService.MongoDb.Entity.Data> GetDataPagingByDeviceId(DateTime fromDate, DateTime toDate, int deviceId, int skip, int limit, out int total)
        {
            List<BtsGetwayService.MongoDb.Entity.Data> list = new List<BtsGetwayService.MongoDb.Entity.Data>();
            list = data.FindPagingOption(i => i.Device_Id == deviceId && i.DateCreate < toDate && i.DateCreate > fromDate , limit, skip, out long totalRow).OrderByDescending(i => i.DateCreate).ToList();
            total = Convert.ToInt32(totalRow);
            return list;
        }
        public async Task<IEnumerable<BtsGetwayService.MongoDb.Entity.Data>> GetDatayDeviceId(DateTime fromDate, DateTime toDate, int deviceId)
        {
            IEnumerable<BtsGetwayService.MongoDb.Entity.Data> list = null;
            list = await data.FindOption(i => i.Device_Id == deviceId && i.DateCreate < toDate && i.DateCreate > fromDate);            
            return list;
        }
        public List<BtsGetwayService.MongoDb.Entity.Data> GetOffline(int deviceId, int skip, int limit, out int total)
        {
            List<BtsGetwayService.MongoDb.Entity.Data> list = new List<BtsGetwayService.MongoDb.Entity.Data>();
            list = data.FindPagingOption(i => i.Device_Id == deviceId, 5,0, out long totalRow1).OrderByDescending(i => i.DateCreate).ToList();
            total = Convert.ToInt32(totalRow1);
            return list;
        }
        public List<BtsGetwayService.MongoDb.Entity.Data> GetByDeviceId(int deviceId, int skip, int limit, out int total)
        {
            List<BtsGetwayService.MongoDb.Entity.Data> list = new List<BtsGetwayService.MongoDb.Entity.Data>();
            list = data.FindPagingOption(i => i.Device_Id == deviceId, 1, 0, out long totalRow).OrderByDescending(i => i.DateCreate).ToList();
            total = Convert.ToInt32(totalRow);
            return list;
        }
    }
}