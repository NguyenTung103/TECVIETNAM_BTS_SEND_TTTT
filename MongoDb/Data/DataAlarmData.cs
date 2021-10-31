using BtsGetwayService.Interface;
using BtsGetwayService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BtsGetwayService.MongoDb.Entity;

namespace BtsGetwayService
{
    public class DataAlarmData : MogoRepository<DataAlarm>
    {              
        public DataAlarmData()
        {            
            
        }
    }
}