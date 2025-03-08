using bts.udpgateway;
using BtsGetwayService;
using Core.Caching;
using Core.Helper;
using Core.Logging;
using Core.Model;
using Core.Model.ObservationModel;
using Core.Model.Report.ReportDay;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Udp
{
    public class ObservationService : IObservationService
    {
        private readonly CacheSettings _cacheSettings;
        private readonly ILoggingService _loggingService;
        private readonly IAsyncCacheService _asyncCacheService;
        private readonly IObservationData _observationData;
        private string _cacheKey;
        public ObservationService(IReportS10Data reportS10Data
            , IObservationData observationData
            , ILoggingService loggingService
            , IAsyncCacheService cacheService
            , IOptions<CacheSettings> option
            )
        {
            _cacheKey = "ObservationService";
            _observationData = observationData;
            _asyncCacheService = cacheService;
            _loggingService = loggingService;
            _cacheSettings = option.Value;
        }
        public async Task<List<ObservationReponseModel>> GetAllObservation(ObservationRequestModel model)
        {
            List<ObservationReponseModel> result = new List<ObservationReponseModel>();
            var dataSql = await _observationData.GetAll();
            return dataSql.Select(i => new ObservationReponseModel
            {
                station_code = model == null ? "" : string.IsNullOrEmpty(model.station_code) ? "" : model.station_code,
                sensor_target = i.Code,
                sensor_name = i.Name,
                status = "1",
            }).ToList();
        }
    }
}
