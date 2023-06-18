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
using Core.Model;
using static log4net.Appender.RollingFileAppender;

namespace BtsGetwayService
{
    public class BtsGetway
    {

        public readonly IGroupData _groupData;
        public readonly ISiteData _siteData;
        public readonly IReportDailyHuongGioData _reportDailyHuongGioData;
        public readonly IReportDailyTocDoGioData _reportDailyTocDoGioData;
        public readonly IDataObservationService _dataObservationMongoService;
        public readonly IReportS10Data _reportS10Data;
        public readonly AppSetting _appSetting;
        Helper helperUlti = new Helper();
        public BtsGetway(
            IOptions<AppSetting> option,
            IGroupData groupData,
            ISiteData siteData,
            IDataObservationService dataObservationMongoService,
            IReportDailyTocDoGioData reportDailyTocDoGioData,
            IReportDailyHuongGioData reportDailyHuongGioData,
             IReportS10Data reportS10Data)
        {
            _appSetting = option.Value;
            _groupData = groupData;
            _siteData = siteData;
            _reportDailyHuongGioData = reportDailyHuongGioData;
            _reportDailyTocDoGioData = reportDailyTocDoGioData;
            _dataObservationMongoService = dataObservationMongoService;
            _reportS10Data = reportS10Data;
        }
        public void SendFile(int nam, int thang)
        {            
            List<RegionalGroup> lstGroup = _groupData.GetGroupSend().Where(i => i.Code == "DB").ToList();
            foreach (DateTime date in AllDatesInMonth(nam, thang))
            {
                for (int i = 0; i < 144; i++)
                {
                    var from = date.AddMinutes(i * 10);
                    var to = date.AddMinutes((i + 1) * 10);                   
                    string appConfigPath = _appSetting.FolderLuuTruFile + @"\data_thang_" + thang.ToString() + @"\";
                    foreach (var grp in lstGroup)
                    {                        
                        string userName = grp.FtpAccount.Trim();
                        string password = grp.FtpPassword.Trim();
                        string appConfigFolderTTTT = grp.FtpDirectory.Trim();
                        string host = grp.FtpIp.Trim();
                        DateTime dateTime = from;
                        var dateTimeStr = "";
                        List<ModelFileKhiTuongS10Json> dataTramKhiTuong = new List<ModelFileKhiTuongS10Json>();
                        List<ModelFileGioS10Json> dataTramGio = new List<ModelFileGioS10Json>();
                        List<ModelFileLuongMuaS10Json> dataMua = new List<ModelFileLuongMuaS10Json>();
                        List<ModelFileMuaNhietS10Json> dataMuaNhiet = new List<ModelFileMuaNhietS10Json>();
                        List<ModelFileThuyVanS10Json> dataThuyVan = new List<ModelFileThuyVanS10Json>();
                        try
                        {
                            List<Site> lstSite = _siteData.GetListSite(grp.Id).ToList();
                            foreach (var site in lstSite)
                            {
                                if (site.TypeSiteId == Constant.KhiTuong)
                                {
                                    if (site.DeviceId.HasValue)
                                    {
                                        var item = _reportS10Data.GetByTime(from, to, site.DeviceId.Value).LastOrDefault();
                                        if (item != null)
                                        {
                                            ModelFileKhiTuongS10Json modelFileS10Json = new ModelFileKhiTuongS10Json();
                                            modelFileS10Json.StationNo = item.DeviceId.Value.ToString("D5");
                                            modelFileS10Json.Datadate = double.Parse(item.DateCreate.Value.ToString("yyyyMMddHHmmss"));
                                            modelFileS10Json.T2m = Utility.CheckNull(item.MTI);
                                            modelFileS10Json.T2mmax = Utility.CheckNull(item.MTX);
                                            modelFileS10Json.T2mmin = Utility.CheckNull(item.MTM);
                                            modelFileS10Json.Rh2m = Utility.CheckNull(item.MHU);
                                            modelFileS10Json.R2mmax = Utility.CheckNull(item.MHX);
                                            modelFileS10Json.R2mmin = Utility.CheckNull(item.MHM);
                                            modelFileS10Json.PS = Utility.CheckNull(item.MAV);
                                            modelFileS10Json.FF10m = Utility.CheckNull(item.MSM);
                                            modelFileS10Json.DD10m = Utility.CheckNull(item.MDM);
                                            modelFileS10Json.FxFx = Utility.CheckNull(item.MSS);
                                            modelFileS10Json.DxDx = Utility.CheckNull(item.MDS);
                                            modelFileS10Json.DtdateFxDx = Utility.CheckNull(item.MHR);
                                            modelFileS10Json.Rain10m = Utility.CheckNull(item.MRT);
                                            modelFileS10Json.Rain1h = Utility.CheckNull(item.MRH);
                                            modelFileS10Json.Rain19h = Utility.CheckNull(item.MRC);
                                            modelFileS10Json.Rain00h = Utility.CheckNull(item.MRB);
                                            modelFileS10Json.Battery = Utility.CheckNull(item.MVC);
                                            dataTramKhiTuong.Add(modelFileS10Json);
                                            dateTimeStr = modelFileS10Json.Datadate.ToString();
                                            dateTime = item.DateCreate.Value;
                                        }
                                    }
                                }
                                else if (site.TypeSiteId == Constant.ThuyVan)
                                {
                                    if (site.DeviceId.HasValue)
                                    {
                                        var item = _reportS10Data.GetByTime(from, to, site.DeviceId.Value).LastOrDefault();
                                        if (item != null)
                                        {
                                            ModelFileThuyVanS10Json modelFileS10Json = new ModelFileThuyVanS10Json();
                                            modelFileS10Json.StationNo = item.DeviceId.Value.ToString("D5");
                                            modelFileS10Json.Datadate = double.Parse(item.DateCreate.Value.ToString("yyyyMMddHHmmss"));
                                            modelFileS10Json.WL = 0;
                                            dataThuyVan.Add(modelFileS10Json);
                                            dateTimeStr = modelFileS10Json.Datadate.ToString();
                                            dateTime = item.DateCreate.Value;
                                        }
                                    }
                                }
                                else if (site.TypeSiteId == Constant.DoMua)
                                {
                                    if (site.DeviceId.HasValue)
                                    {
                                        var item = _reportS10Data.GetByTime(from, to, site.DeviceId.Value).LastOrDefault();
                                        if (item != null)
                                        {
                                            ModelFileLuongMuaS10Json modelFileS10Json = new ModelFileLuongMuaS10Json();
                                            modelFileS10Json.StationNo = item.DeviceId.Value.ToString("D6");
                                            modelFileS10Json.Datadate = double.Parse(item.DateCreate.Value.ToString("yyyyMMddHHmmss"));
                                            modelFileS10Json.Rain10m = Utility.CheckNull(item.MRT);
                                            modelFileS10Json.Rain1h = Utility.CheckNull(item.MRH);
                                            modelFileS10Json.Rain19h = Utility.CheckNull(item.MRC);
                                            modelFileS10Json.Rain00h = Utility.CheckNull(item.MRB);
                                            modelFileS10Json.Battery = Utility.CheckNull(item.MVC);
                                            dataMua.Add(modelFileS10Json);
                                            dateTimeStr = modelFileS10Json.Datadate.ToString();
                                            dateTime = item.DateCreate.Value;
                                        }
                                    }
                                }
                                else if (site.TypeSiteId == Constant.DoGio)
                                {
                                    if (site.DeviceId.HasValue)
                                    {
                                        var item = _reportS10Data.GetByTime(from, to, site.DeviceId.Value).LastOrDefault();
                                        if (item != null)
                                        {
                                            ModelFileGioS10Json modelFileS10Json = new ModelFileGioS10Json();
                                            modelFileS10Json.StationNo = item.DeviceId.Value.ToString("D6");
                                            modelFileS10Json.Datadate = double.Parse(item.DateCreate.Value.ToString("yyyyMMddHHmmss"));
                                            modelFileS10Json.FF10m = Utility.CheckNull(item.MSM);
                                            modelFileS10Json.DD10m = Utility.CheckNull(item.MDM);
                                            modelFileS10Json.FxFx = Utility.CheckNull(item.MSS);
                                            modelFileS10Json.DxDx = Utility.CheckNull(item.MDS);
                                            modelFileS10Json.DtdateFxDx = Utility.CheckNull(item.MHR);
                                            modelFileS10Json.Battery = Utility.CheckNull(item.MVC);
                                            dataTramGio.Add(modelFileS10Json);
                                            dateTimeStr = modelFileS10Json.Datadate.ToString();
                                            dateTime = item.DateCreate.Value;
                                        }
                                    }
                                }
                                else if (site.TypeSiteId == Constant.MuaNhiet)
                                {
                                    if (site.DeviceId.HasValue)
                                    {
                                        var item = _reportS10Data.GetByTime(from, to, site.DeviceId.Value).LastOrDefault();
                                        if (item != null)
                                        {
                                            ModelFileMuaNhietS10Json modelFileS10Json = new ModelFileMuaNhietS10Json();
                                            modelFileS10Json.StationNo = item.DeviceId.Value.ToString("D5");
                                            modelFileS10Json.Datadate = double.Parse(item.DateCreate.Value.ToString("yyyyMMddHHmmss"));
                                            modelFileS10Json.T2m = Utility.CheckNull(item.MTI);
                                            modelFileS10Json.T2mmax = Utility.CheckNull(item.MTX);
                                            modelFileS10Json.T2mmin = Utility.CheckNull(item.MTM);
                                            modelFileS10Json.Rh2m = Utility.CheckNull(item.MHU);
                                            modelFileS10Json.R2mmax = Utility.CheckNull(item.MHX);
                                            modelFileS10Json.R2mmin = Utility.CheckNull(item.MHM);
                                            modelFileS10Json.Rain10m = Utility.CheckNull(item.MRT);
                                            modelFileS10Json.Rain1h = Utility.CheckNull(item.MRH);
                                            modelFileS10Json.Rain19h = Utility.CheckNull(item.MRC);
                                            modelFileS10Json.Rain00h = Utility.CheckNull(item.MRB);
                                            modelFileS10Json.Battery = Utility.CheckNull(item.MVC);
                                            dataMuaNhiet.Add(modelFileS10Json);
                                            dateTimeStr = modelFileS10Json.Datadate.ToString();
                                            dateTime = item.DateCreate.Value;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        #region Tạo directory trên server
                        string urlFullPathFolder = appConfigPath + appConfigFolderTTTT + "\\" + helperUlti.GetDay(dateTime) + "\\";
                        bool exists = Directory.Exists(urlFullPathFolder);
                        if (!exists)
                            Directory.CreateDirectory(urlFullPathFolder);
                        #endregion
                    }
                }
            }
        }
        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }
    }
}
