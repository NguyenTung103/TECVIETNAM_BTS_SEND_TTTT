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
        private readonly ISiteData _siteData;
        private string _cacheKey;
        public ObservationService(IReportS10Data reportS10Data
            , IObservationData observationData
            , ILoggingService loggingService
            , IAsyncCacheService cacheService
            , ISiteData siteData
            , IOptions<CacheSettings> option
            )
        {
            _cacheKey = "ObservationService";
            _observationData = observationData;
            _asyncCacheService = cacheService;
            _loggingService = loggingService;
            _siteData = siteData;
            _cacheSettings = option.Value;
        }
        public async Task<List<ObservationReponseModel>> GetAllObservation(ObservationRequestModel model)
        {
            List<ObservationReponseModel> result = new List<ObservationReponseModel>();
            var thietBi = await _siteData.GetByDeviceId(Int32.Parse(model.station_code));
            var dataSql = await _observationData.GetAll(thietBi.TypeSiteId);
            return dataSql.Select(i => new ObservationReponseModel
            {
                station_code = model == null ? "" : string.IsNullOrEmpty(model.station_code) ? "" : model.station_code,
                sensor_target = i.Code,
                sensor_name = i.Name,
                status = "1",
            }).ToList();
        }
        public async Task<List<DsObservationReponseModel>> GetDsObservation(DsObservationRequestModel model)
        {
            List<int> lstIdThietBi = new List<int>();
            if (model.station_codes.Any())
            {
                var dsThietBi = model.station_codes.Split(",").ToList();
                foreach (var item in dsThietBi)
                {
                    if (item.Contains(Constant.Prefix_Device_HoaBinh))
                        lstIdThietBi.Add(Int32.Parse(item.Replace(Constant.Prefix_Device_HoaBinh, "")));
                    else
                        lstIdThietBi.Add(Int32.Parse(item));
                }
            }
            var thietBi = await _siteData.GetDsSite(lstIdThietBi);
            var dsSensorAll = await _observationData.GetAll();
            List<DsObservationReponseModel> result = new List<DsObservationReponseModel>();
            foreach (var item in thietBi)
            {
                DsObservationReponseModel dataSensorThietBi = new DsObservationReponseModel();
                string codePrefix = item.Code_Group == Constant.MuaHoaBinh ? Constant.Prefix_Device_HoaBinh : "";
                dataSensorThietBi.station_code = codePrefix + item.Code;
                List<ObservationDetailReponseModel> dsSensor = dsSensorAll.Where(i => i.CategoryTypeSite == item.TypeSiteId).Select(i => new ObservationDetailReponseModel
                {
                    sensor_ch = i.CategoryTypeSite.ToString(),
                    sensor_name = i.Name,
                    sensor_target = codePrefix + i.Code,
                }).ToList();
                dataSensorThietBi.sensor = dsSensor;
                result.Add(dataSensorThietBi);
            }            
            return result;
        }
    }
}
