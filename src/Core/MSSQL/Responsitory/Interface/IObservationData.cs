using bts.udpgateway;
using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using Core.Model.ObservationModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.MSSQL.Responsitory.Interface
{
    public interface IObservationData : IDapperBaseData<Observation>
    {
        Observation GetByCode(string code);
        Task<List<Observation>> GetAll(int? type = null);
        Task<List<ObservationReponseModel>> GetObservationByDeviceId(List<int>? deviceId = null);
    }
}
