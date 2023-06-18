using bts.udpgateway;
using BtsGetwayService.Model;
using BtsGetwayService.MSSQL.Entity;
using BtsGetwayService.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Text.Json;
using Core.MSSQL.Responsitory.Interface;
using Microsoft.Extensions.Options;
using Core.Setting;
using BtsGetwayService.Interface;
using Core.Logging;
using Core.Model;
using Core.Helper;

namespace BtsGetwayService
{
    public class BtsGetway
    {

        public readonly IGroupData _groupData;
        private readonly ILoggingService _loggingService;
        public readonly ISiteData _siteData;
        public readonly IReportS10Data _reportS10Data;
        public readonly AppApiWatecSetting _appSetting;
        Helper helperUlti = new Helper();
        public BtsGetway(
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
        }
        public async void SendFile(DateTime to, DateTime from, int groupId)
        {
            List<RegionalGroup> lstGroup = new List<RegionalGroup>();
            List<RegionalGroup> lstGroupAll = _groupData.GetGroupSend().ToList();
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
                            if (site.TypeSiteId == Constant.DoMua)
                            {
                                if (site.DeviceId.HasValue)
                                {
                                    var item = _reportS10Data.GetByTime(from, to, site.DeviceId.Value).LastOrDefault();
                                    if (item != null)
                                    {
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
                    }
                    #endregion

                    #region Send data api                    
                    var result = await ApiSend.PostDataObject(_appSetting.ApiKey, _appSetting.UrlPost, listData);
                    _loggingService.Info(result.Code + "/" + result.Message);
                    #endregion
                }
            }

        }
    }
}
