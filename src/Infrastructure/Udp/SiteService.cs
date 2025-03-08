using bts.udpgateway;
using bts.udpgateway.integration;
using BtsGetwayService;
using BtsGetwayService.Interface;
using BtsGetwayService.MongoDb.Entity;
using Core.Caching;
using Core.Constant;
using Core.Logging;
using Core.Model.TramModels;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Udp
{
    public class SiteService : ISiteService
    {
        public readonly ISiteData _siteData;
        private readonly ILoggingService _loggingService;
        private readonly IAsyncCacheService _asyncCacheService;
        private string _cacheKey;
        private readonly CacheSettings _cacheSettings;
        Helper helperUlti = new Helper();
        public SiteService(IOptions<CacheSettings> option
            ,ISiteData siteData
            , IAsyncCacheService cacheService
           )
        {
            _cacheKey = "TramService";
            _siteData = siteData;
            _asyncCacheService = cacheService;
            _cacheSettings = option.Value;
        }
        public async Task<List<SiteModel>> GetDanhSachTram(List<string> danh_sach_tram)
        {
            List<SiteModel> result = new List<SiteModel>();
            try
            {
                string key_ds_tram = string.Empty;
                if (key_ds_tram.Any())
                {
                    key_ds_tram = string.Join(",", danh_sach_tram);
                }
                string strCachedKey = Utility.BuildCachedKey(_cacheKey, "GetDanhSachTram", key_ds_tram);
                return await _asyncCacheService.GetOrCreateAsync(strCachedKey, async () =>
                {
                    var dsDeviceId = danh_sach_tram.Select(i => Int32.Parse(i.Substring(5))).ToList();
                    var data = (await _siteData.GetDsSite(dsDeviceId));
                    return data.Select(i => new SiteModel
                    {
                        station_code = (i.Code == ObservationConstant.MA_TRAM_MUA_HOA_BINH ? ObservationConstant.PREFIX_DO_MUA_HOA_BINH + i.DeviceId.ToString() : i.DeviceId.ToString()),
                        station_name = i.Name,
                        latitude = string.IsNullOrEmpty(i.Latitude)?"": i.Latitude.Trim(),
                        longtitude = string.IsNullOrEmpty(i.Longtitude) ? "" : i.Longtitude.Trim(),
                        status = (i.IsActive == true ? 1 : 0).ToString(),
                        address = i.Address,
                        obs_type = i.TypeSiteId.ToString()
                    }).ToList();
                }, _cacheSettings.CacheTime);
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex);

            }
            return result;
        }
    }
}
