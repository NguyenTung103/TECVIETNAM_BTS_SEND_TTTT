using BtsGetwayService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtsGetwayService.Interface
{
    public interface IDataObservationService : IRepository<BtsGetwayService.MongoDb.Entity.Data>
    {
    }
}
