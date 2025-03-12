using bts.udpgateway;
using Core.Model.ObservationModel;
using Core.Model;
using Core.Model.Report.ReportDay;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Udp
{
    public interface IObservationService
    {
        Task<List<ObservationReponseModel>> GetAllObservation(ObservationRequestModel model);
        Task<List<DsObservationReponseModel>> GetDsObservation(DsObservationRequestModel model);        
    }
}
