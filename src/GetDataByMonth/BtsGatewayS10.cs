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
using BtsGetwayService;

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
            List<RegionalGroup> lstGroup = _groupData.GetGroupSend().ToList();
            foreach (DateTime date in AllDatesInMonth(nam, thang))
            {
                for (int i = 0; i < 144; i++)
                {
                    var from = date.AddMinutes(i * 10);
                    var to = date.AddMinutes((i + 1) * 10);
                    string appConfigPath = _appSetting.FolderLuuTruFile + @"\" + thang.ToString() + @"\";
                    foreach (var grp in lstGroup)
                    {
                        //string userName = grp.FtpAccount.Trim();
                        //string password = grp.FtpPassword.Trim();
                        //string appConfigFolderTTTT = grp.FtpDirectory.Trim();
                        //string host = grp.FtpIp.Trim();
                        //if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(host))
                        //{
                        var dateTimeStr = "";
                        List<ModelFileS10Json> dataTramKhiTuong = new List<ModelFileS10Json>();
                        try
                        {
                            List<Site> lstSite = _siteData.GetListSite(grp.Id).ToList();
                            foreach (var site in lstSite)
                            {
                                if (site.DeviceId.HasValue)
                                {
                                    var reportS10ByDevice = _reportS10Data.GetByTime(from, to, site.DeviceId.Value);
                                    if (reportS10ByDevice != null && reportS10ByDevice.Count() > 0)
                                        foreach (var item in reportS10ByDevice)
                                        {
                                            ModelFileS10Json modelFileS10Json = new ModelFileS10Json();
                                            modelFileS10Json.StationNo = item.DeviceId.Value;
                                            modelFileS10Json.Datadate = double.Parse(item.DateCreate.Value.ToString("yyyyMMddHHmmss"));
                                            modelFileS10Json.T2m = item.MTI.Value;
                                            modelFileS10Json.T2mmax = item.MTX.Value;
                                            modelFileS10Json.T2mmin = item.MTM.Value;
                                            modelFileS10Json.Rh2m = item.MHU.Value;
                                            modelFileS10Json.R2mmax = item.MHX.Value;
                                            modelFileS10Json.R2mmin = item.MHM.Value;
                                            modelFileS10Json.PS = item.MAV.Value;
                                            modelFileS10Json.FF10m = item.MSM.Value;
                                            modelFileS10Json.DD10m = item.MDM.Value;
                                            modelFileS10Json.FxFx = item.MSS.Value;
                                            modelFileS10Json.DxDx = item.MDS.Value;
                                            modelFileS10Json.DtdateFxDx = item.MHR.Value;
                                            modelFileS10Json.Rain10m = item.MRT.Value;
                                            modelFileS10Json.Rain1h = item.MRH.Value;
                                            modelFileS10Json.Rain19h = item.MRC.Value;
                                            modelFileS10Json.Rain00h = item.MRB.Value;
                                            modelFileS10Json.Battery = item.MVC.Value;
                                            dataTramKhiTuong.Add(modelFileS10Json);
                                            dateTimeStr = modelFileS10Json.Datadate.ToString();
                                        }
                                }
                            }
                           
                        }
                        catch (Exception ex)
                        {
                            _loggingService.Error(ex);
                        }
                        string urlFullPathFolder = appConfigPath + helperUlti.GetDay(to) + "\\";
                        bool exists = Directory.Exists(urlFullPathFolder);
                        if (!exists)
                            Directory.CreateDirectory(urlFullPathFolder);
                        if (dataTramKhiTuong != null && dataTramKhiTuong.Count() > 0)
                        {
                            string nameFile = grp.Code + "_AWS_" + dateTimeStr + ".json";
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
                                // GetOrCreateFolder(appConfigFolderTTTT, to, nameFile, path, userName, password, host);
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
