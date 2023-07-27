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

namespace BtsGetwayService
{
    public class BtsGetway
    {

        public readonly IGroupData _groupData;
        private readonly ILoggingService _loggingService;
        public readonly ISiteData _siteData;
        public readonly IReportS10Data _reportS10Data;
        public readonly AppSetting _appSetting;
        Helper helperUlti = new Helper();
        public BtsGetway(
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
        public void SendFile(DateTime to, DateTime from)
        {
            List<RegionalGroup> lstGroup = _groupData.GetGroupSend().ToList();
            string appConfigPath = _appSetting.FolderLuuTruFile;
            int isSendDataTTTT = _appSetting.IsSendTTTT;
            foreach (var grp in lstGroup)
            {
                DateTime dateTime = from;
                string userName = grp.FtpAccount.Trim();
                string password = grp.FtpPassword.Trim();
                string appConfigFolderTTTT = grp.FtpDirectory.Trim();
                string host = grp.FtpIp.Trim();
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(host))
                {
                    #region Lấy dữ liệu
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
                    #endregion

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
        private bool CreateFile(string path, string userName, string password)
        {
            bool isCreated = true;
            try
            {
                WebRequest request = WebRequest.Create(path);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(userName, password);
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine(resp.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex);
                isCreated = false;
            }
            return isCreated;
        }
        private bool DeleteFile(string Folderpath, string userName, string password)
        {
            bool result = false;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Folderpath);
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            request.Credentials = new System.Net.NetworkCredential(userName, password); ;
            request.GetResponse().Close();
            return result;
        }
        private List<string> GetAllFtpFiles(string ParentFolderpath, string userName, string password)
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ParentFolderpath);
                ftpRequest.Credentials = new NetworkCredential(userName, password);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                List<string> directories = new List<string>();

                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var lineArr = line.Split('/');
                    line = lineArr[lineArr.Count() - 1];
                    directories.Add(line);
                    line = streamReader.ReadLine();
                }

                streamReader.Close();

                return directories;
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex);
                throw ex;
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
