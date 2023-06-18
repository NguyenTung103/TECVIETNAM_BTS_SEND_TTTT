using BtsGetwayService.Core.Interfaces;
using BtsGetwayService.MongoDb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtsGetwayService.Interface
{
    public interface IDataAlarmService
    {
        List<DataAlarm> GetDataByDay(out int totalRow, int limit);
        DataAlarm FindByKey(string alarmId);
        List<DataAlarm> GetDataPaging(DateTime fromDate, DateTime toDate, int skip, int limit, int deviceId, out int total);
        List<DataAlarm> GetDataOption(int limit, int deviceid);
        bool Insert(DataAlarm entity);
    }
}
