using BtsGetwayService.MongoDb.Entity;
using ES_CapDien.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BtsGetwayService.Service
{
    public class DataAlarmMongoService
    {
        DataAlarmData data = new DataAlarmData();
        public List<DataAlarm> GetDataByDay(out int totalRow, int limit)
        {
            List<DataAlarm> list = new List<DataAlarm>();
            DateTime from = DateTime.Today;
            DateTime to = DateTime.Today.AddDays(1);
            list = data.FindAll(i => i.DateCreate < to && i.DateCreate > from).OrderByDescending(i => i.DateCreate).ToList();
            totalRow = list.Count();            
            return list;
        }
        public DataAlarm FindByKey(string alarmId)
        {
            DataAlarm entity = new DataAlarm();
            entity = data.FindByKey(alarmId);            
            return entity;
        }
        public List<DataAlarm> GetDataPaging(DateTime fromDate,DateTime toDate, int skip, int limit, int deviceId, out int total)
        {
            List<DataAlarm> list = new List<DataAlarm>();           
            list = data.FindPagingOption(i =>  i.Device_Id==deviceId, limit, skip,out long totalRow).OrderByDescending(i => i.DateCreate).ToList();
            total = Convert.ToInt32(totalRow);
            return list;
        }
        public List<DataAlarm> GetDataOption(int limit, int deviceid)
        {
            List<DataAlarm> list = new List<DataAlarm>();         
            list = data.FindPagingOption(i => i.Device_Id==deviceid, 300,0,out long total).OrderByDescending(i => i.DateCreate).ToList();           
            return list;
        }
    }
}