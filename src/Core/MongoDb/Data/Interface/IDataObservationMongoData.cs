using BtsGetwayService.Core.Interfaces;
using BtsGetwayService.Data;
using BtsGetwayService.MongoDb.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.MongoDb.Data.Interface
{
    public interface IDataObservationMongoData : IRepository<BtsGetwayService.MongoDb.Entity.Data>
    {
    }
}
