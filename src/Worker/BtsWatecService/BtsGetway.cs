using bts.udpgateway;
using BtsGetwayService.MSSQL.Entity;
using Core.Helper;
using Core.Logging;
using Core.Model;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BtsGetwayService
{
    public class BtsGetway
    {
        private readonly ILogger<BtsGetway> _logger;
        public readonly IGroupData _groupData;
        private readonly ILoggingService _loggingService;
        public readonly ISiteData _siteData;
        public readonly IReportS10Data _reportS10Data;
        public readonly AppApiWatecSetting _appSetting;
        Helper helperUlti = new Helper();
        public BtsGetway(ILogger<BtsGetway> logger,
            IOptions<AppApiWatecSetting> option,
            IGroupData groupData,
            ISiteData siteData,
            ILoggingService loggingService,
            IReportS10Data reportS10Data)
        {
            _appSetting = option.Value;
            _groupData = groupData;
            _siteData = siteData;
            _loggingService = loggingService;
            _reportS10Data = reportS10Data;
            _logger = logger;
        }
        public async void SendFile(DateTime to, DateTime from, int groupId)
        {
            List<RegionalGroup> lstGroup = new List<RegionalGroup>();
            List<RegionalGroup> lstGroupAll = _groupData.GetAll().ToList();
            if (_appSetting.IsChooseGroup == 1)
                lstGroup = lstGroupAll.Where(i => i.Id == groupId).ToList();
            else
                lstGroup = lstGroupAll;
            if (lstGroup != null && lstGroup.Count() > 0)
            {
                foreach (var grp in lstGroup)
                {
                    DateTime dateTime = from;
                    #region Lấy dữ liệu                                     
                    List<WatecS10Model> listData = new List<WatecS10Model>();
                    try
                    {
                        List<Site> lstSite = _siteData.GetListSite(grp.Id).ToList();
                        foreach (var site in lstSite)
                        {
                            _logger.LogInformation("Get " + site.Name);
                            if (site.TypeSiteId == Constant.DoMua)
                            {
                                if (site.DeviceId.HasValue)
                                {
                                    var item = _reportS10Data.GetByTime(from, to, site.DeviceId.Value).LastOrDefault();
                                    if (item != null)
                                    {
                                        _logger.LogInformation("Push Data " + item);
                                        WatecS10Model modelFileS10Json = new WatecS10Model();
                                        modelFileS10Json.sid = item.DeviceId.Value.ToString("D10");
                                        modelFileS10Json.from = item.DateCreate.Value.ToString("yyyy-MM-dd HH:mm");
                                        modelFileS10Json.to = item.DateCreate.Value.ToString("yyyy-MM-dd HH:mm");
                                        modelFileS10Json.val = Utility.CheckNull(item.MRT);
                                        listData.Add(modelFileS10Json);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _loggingService.Error(ex);
                        _logger.LogError(null, ex);
                    }
                    #endregion

                    #region Send data api                    
                    var result = await ApiSend.PostDataObject(_appSetting.ApiKey, _appSetting.UrlPost, listData);
                    _loggingService.Info(result.Code + "/" + result.Message);
                    _logger.LogInformation(result.Code + "/" + result.Message);
                    #endregion
                }
            }
        }
    }
}
