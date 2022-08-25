using BtsGetwayService.Interface;
using BtsGetwayService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BtsGetwayService.MongoDb.Entity;
using Microsoft.Extensions.Options;
using Core.Setting;
using Core.MongoDb.Data.Interface;

namespace ES_CapDien.AppCode
{
    public class DataObservationMongoData : MogoRepository<Data>, IDataObservationMongoData
    {                      
        public DataObservationMongoData(IOptions<Connections> option) : base(option.Value.ConnectMongoString)
        {            
            
        }        
    }
}