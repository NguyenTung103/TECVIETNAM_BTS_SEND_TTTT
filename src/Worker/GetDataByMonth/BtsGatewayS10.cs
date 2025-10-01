using bts.udpgateway;
using BtsGetwayService;
using BtsGetwayService.Model;
using BtsGetwayService.MSSQL.Entity;
using Core.Logging;
using Core.Model;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using static log4net.Appender.RollingFileAppender;

namespace GetDataByMonth
{
    public class BtsGatewayS10
    {
        public readonly IGroupData _groupData;
        private readonly ILoggingService _loggingService;
        public readonly ISiteData _siteData;
        public readonly IReportS10Data _reportS10Data;
        public readonly AppSetting _appSetting;
        Helper helperUlti = new Helper();
        public BtsGatewayS10(
            IOptions<AppSetting> option,
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
        public void SendFile(int nam, int thang, int ngay)
        {
            string appConfigPath = _appSetting.FolderLuuTruFile + @"\";
            int isSendDataTTTT = _appSetting.IsSendTTTT;
            List<RegionalGroup> lstGroup = _groupData.GetGroupSend().ToList();
            if (ngay > 0)
            {
                var ngayLayDuLieu = new DateTime(nam, thang, ngay, 00, 00, 00, 00);               
                List<DateTime> lstDate = new List<DateTime>();
                lstDate.Add(ngayLayDuLieu);
                foreach (DateTime date in lstDate)
                {
                    for (int i = 0; i < 144; i++)
                    {
                        var from = date.AddMinutes(i * 10);
                        var to = date.AddMinutes((i + 1) * 10);
                        foreach (var grp in lstGroup)
                        {
                            DateTime dateTime = from;
                            string userName = isSendDataTTTT == 1 ? grp.FtpAccount.Trim() : "";
                            string password = isSendDataTTTT == 1 ? grp.FtpPassword.Trim() : "";
                            string appConfigFolderTTTT = isSendDataTTTT == 1 ? grp.FtpDirectory.Trim() : "";
                            string host = isSendDataTTTT == 1 ? grp.FtpIp.Trim() : "";
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
                                                modelFileS10Json.WL = float.Parse(item.MFL == null ? "0" : Math.Round(item.MFL.Value * 100, 2).ToString());
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
                                _loggingService.Error(ex);
                            }
                            #region Tạo directory trên server
                            string urlFullPathFolder = appConfigPath + appConfigFolderTTTT + "\\" + helperUlti.GetDay(dateTime) + "\\";
                            bool exists = Directory.Exists(urlFullPathFolder);
                            if (!exists)
                                Directory.CreateDirectory(urlFullPathFolder);
                            #endregion

                            #region Tạo file trên server và gửi dữ liệu sang TTDL                    
                            if (dataTramKhiTuong != null && dataTramKhiTuong.Count() > 0)
                            {
                                string nameFile = grp.Code + "_" + Constant.MaKhiTuong + "_" + dateTimeStr + ".json";
                                string path = urlFullPathFolder + nameFile;
                                try
                                {
                                    var options = new JsonSerializerOptions { WriteIndented = true };
                                    string jsonString = System.Text.Json.JsonSerializer.Serialize(dataTramKhiTuong, options);
                                    //Create a new file
                                    if (!File.Exists(path))
                                    {
                                        File.WriteAllText(path, jsonString);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }
                                try
                                {
                                    if (isSendDataTTTT == 1)
                                    {
                                        GetOrCreateFolder(appConfigFolderTTTT, dateTime, nameFile, path, userName, password, host);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }
                            }
                            if (dataTramGio != null && dataTramGio.Count() > 0)
                            {
                                string nameFile = grp.Code + "_" + Constant.MaDoGio + "_" + dateTimeStr + ".json";
                                string path = urlFullPathFolder + nameFile;
                                try
                                {
                                    var options = new JsonSerializerOptions { WriteIndented = true };
                                    string jsonString = System.Text.Json.JsonSerializer.Serialize(dataTramGio, options);
                                    //Create a new file
                                    if (!File.Exists(path))
                                    {
                                        File.WriteAllText(path, jsonString);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }
                                try
                                {
                                    if (isSendDataTTTT == 1)
                                    {
                                        GetOrCreateFolder(appConfigFolderTTTT, dateTime, nameFile, path, userName, password, host);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }
                            }
                            if (dataMua != null && dataMua.Count() > 0)
                            {
                                string nameFile = grp.Code + "_" + Constant.MaDoMua + "_" + dateTimeStr + ".json";
                                string path = urlFullPathFolder + nameFile;
                                try
                                {
                                    var options = new JsonSerializerOptions { WriteIndented = true };
                                    string jsonString = System.Text.Json.JsonSerializer.Serialize(dataMua, options);
                                    //Create a new file
                                    if (!File.Exists(path))
                                    {
                                        File.WriteAllText(path, jsonString);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }
                                try
                                {
                                    if (isSendDataTTTT == 1)
                                    {
                                        GetOrCreateFolder(appConfigFolderTTTT, dateTime, nameFile, path, userName, password, host);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }
                            }
                            if (dataMuaNhiet != null && dataMuaNhiet.Count() > 0)
                            {
                                string nameFile = grp.Code + "_" + Constant.MaMuaNhiet + "_" + dateTimeStr + ".json";
                                string path = urlFullPathFolder + nameFile;
                                try
                                {
                                    var options = new JsonSerializerOptions { WriteIndented = true };
                                    string jsonString = System.Text.Json.JsonSerializer.Serialize(dataMuaNhiet, options);
                                    //Create a new file
                                    if (!File.Exists(path))
                                    {
                                        File.WriteAllText(path, jsonString);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }
                                try
                                {
                                    if (isSendDataTTTT == 1)
                                    {
                                        GetOrCreateFolder(appConfigFolderTTTT, dateTime, nameFile, path, userName, password, host);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }
                            }
                            if (dataThuyVan != null && dataThuyVan.Count() > 0)
                            {
                                string nameFile = grp.Code + "_" + Constant.MaThuyVan + "_" + dateTimeStr + ".json";
                                string path = urlFullPathFolder + nameFile;
                                try
                                {
                                    var options = new JsonSerializerOptions { WriteIndented = true };
                                    string jsonString = System.Text.Json.JsonSerializer.Serialize(dataThuyVan, options);
                                    //Create a new file
                                    if (!File.Exists(path))
                                    {
                                        File.WriteAllText(path, jsonString);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }
                                try
                                {
                                    if (isSendDataTTTT == 1)
                                    {
                                        GetOrCreateFolder(appConfigFolderTTTT, dateTime, nameFile, path, userName, password, host);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            else
            {
                foreach (DateTime date in AllDatesInMonth(nam, thang))
                {
                    for (int i = 0; i < 144; i++)
                    {
                        var from = helperUlti.RoundDown(date.AddMinutes(i * 10), TimeSpan.FromMinutes(5));
                        var to = helperUlti.RoundDown(date.AddMinutes((i + 1) * 10), TimeSpan.FromMinutes(5));                        
                        _loggingService.Info("item.DateCreate is null");
                        foreach (var grp in lstGroup)
                        {
                            DateTime dateTime = from;
                            string userName = isSendDataTTTT == 1 ? grp.FtpAccount.Trim() : "";
                            string password = isSendDataTTTT == 1 ? grp.FtpPassword.Trim() : "";
                            string appConfigFolderTTTT = isSendDataTTTT == 1 ? grp.FtpDirectory.Trim() : "";
                            string host = isSendDataTTTT == 1 ? grp.FtpIp.Trim() : "";                            
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
                                                modelFileS10Json.WL = float.Parse(item.MFL == null ? "0" : Math.Round(item.MFL.Value, 2).ToString());
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
                                _loggingService.Error(ex);
                            }
                            string urlFullPathFolder = appConfigPath + appConfigFolderTTTT + "\\" + helperUlti.GetDay(dateTime) + "\\";
                            bool exists = Directory.Exists(urlFullPathFolder);
                            if (!exists)
                                Directory.CreateDirectory(urlFullPathFolder);
                            if (dataTramKhiTuong != null && dataTramKhiTuong.Count() > 0)
                            {
                                string nameFile = grp.Code + "_" + Constant.MaKhiTuong + "_" + dateTimeStr + ".json";
                                string path = urlFullPathFolder + nameFile;
                                try
                                {
                                    var options = new JsonSerializerOptions { WriteIndented = true };
                                    string jsonString = System.Text.Json.JsonSerializer.Serialize(dataTramKhiTuong, options);
                                    //Create a new file
                                    if (!File.Exists(path))
                                    {
                                        File.WriteAllText(path, jsonString);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }                                
                            }
                            if (dataTramGio != null && dataTramGio.Count() > 0)
                            {
                                string nameFile = grp.Code + "_" + Constant.MaDoGio + "_" + dateTimeStr + ".json";
                                string path = urlFullPathFolder + nameFile;
                                try
                                {
                                    var options = new JsonSerializerOptions { WriteIndented = true };
                                    string jsonString = System.Text.Json.JsonSerializer.Serialize(dataTramGio, options);
                                    //Create a new file
                                    if (!File.Exists(path))
                                    {
                                        File.WriteAllText(path, jsonString);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }                                
                            }
                            if (dataMua != null && dataMua.Count() > 0)
                            {
                                string nameFile = grp.Code + "_" + Constant.MaDoMua + "_" + dateTimeStr + ".json";
                                string path = urlFullPathFolder + nameFile;
                                try
                                {
                                    var options = new JsonSerializerOptions { WriteIndented = true };
                                    string jsonString = System.Text.Json.JsonSerializer.Serialize(dataMua, options);
                                    //Create a new file
                                    if (!File.Exists(path))
                                    {
                                        File.WriteAllText(path, jsonString);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }                                
                            }
                            if (dataMuaNhiet != null && dataMuaNhiet.Count() > 0)
                            {
                                string nameFile = grp.Code + "_" + Constant.MaMuaNhiet + "_" + dateTimeStr + ".json";
                                string path = urlFullPathFolder + nameFile;
                                try
                                {
                                    var options = new JsonSerializerOptions { WriteIndented = true };
                                    string jsonString = System.Text.Json.JsonSerializer.Serialize(dataMuaNhiet, options);
                                    //Create a new file
                                    if (!File.Exists(path))
                                    {
                                        File.WriteAllText(path, jsonString);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }                                
                            }
                            if (dataThuyVan != null && dataThuyVan.Count() > 0)
                            {
                                string nameFile = grp.Code + "_" + Constant.MaThuyVan + "_" + dateTimeStr + ".json";
                                string path = urlFullPathFolder + nameFile;
                                try
                                {
                                    var options = new JsonSerializerOptions { WriteIndented = true };
                                    string jsonString = System.Text.Json.JsonSerializer.Serialize(dataThuyVan, options);
                                    //Create a new file
                                    if (!File.Exists(path))
                                    {
                                        File.WriteAllText(path, jsonString);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _loggingService.Error(ex);
                                }                                
                            }
                            //}
                        }
                    }
                }
            }

        }
        private void GetOrCreateFolder(string folderTTTT, DateTime to, string fileName, string fullPathSourceFile, string userName, string password, string host)
        {
            string urlFolder = host + folderTTTT + "/" + helperUlti.GetDay(to) + "/";
            bool checkExisFolder = DoesFtpDirectoryExist(urlFolder, userName, password);
            if (checkExisFolder == false)
            {
                bool check = CreateFTPDirectory(urlFolder, userName, password);
            }
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(userName, password);
                client.UploadFile(urlFolder + fileName, WebRequestMethods.Ftp.UploadFile, fullPathSourceFile);
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
        public bool DoesFtpDirectoryExist(string dirPath, string userName, string password)
        {
            bool isexist = false;

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(dirPath);
                request.Credentials = new NetworkCredential(userName, password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    isexist = true;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return false;
                    }
                }
            }
            return isexist;
        }
        private bool CreateFTPDirectory(string directory, string userName, string password)
        {

            try
            {
                //create the directory
                FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri(directory));
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential(userName, password);
                requestDir.UsePassive = true;
                requestDir.UseBinary = true;
                requestDir.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();

                return true;
            }
            catch (WebException ex)
            {

                FtpWebResponse response = (FtpWebResponse)ex.Response;
                String status = response.StatusDescription;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }
            }
        }
    }
}
