using BtsGetwayService.Interface;
using BtsGetwayService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BtsGetwayService.MongoDb.Entity;

namespace ES_CapDien.AppCode
{
    public class DataObservationMongoData : MogoRepository<Data>
    {                      
        public DataObservationMongoData()
        {            
            
        }        
    }
}