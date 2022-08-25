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

namespace BtsGetwayService
{
    public class BtsGetway
    {
        //    public readonly Timer _timer;
        //    GroupData _groupData = new GroupData();
        //    SiteData _siteData = new SiteData();
        //    ReportDailyTocDoGioData _reportDailyTocDoGioData = new ReportDailyTocDoGioData();
        //    DataObservationMongoService dataObservationMongoService = new DataObservationMongoService();
        //    Helper helperUlti = new Helper();
        //    public BtsGetway()
        //    {
        //        _timer = new Timer(SendFile, null, TimeSpan.Zero,
        //            TimeSpan.FromMinutes(10));
        //    }
        //    public void Start()
        //    {
        //        //Timer _timer;
        //        //_timer = new Timer(SendFile, null, TimeSpan.Zero,
        //        //    TimeSpan.FromMinutes(1));
        //    }
        //    public void Stop() { _timer.Dispose(); }
        //    private void SendFile(object state)
        //    {
        //        DateTime to = DateTime.Now;
        //        DateTime from = DateTime.Now.AddMinutes(-10);
        //        List<RegionalGroup> lstGroup = _groupData.GetGroupSend().ToList();
        //        string appConfigPath = ConfigurationManager.AppSettings["FolderLuuTruFile"].ToString();
        //        foreach (var grp in lstGroup)
        //        {
        //            string userName = grp.FtpAccount.Trim();
        //            string password = grp.FtpPassword.Trim();
        //            string appConfigFolderTTTT = grp.FtpDirectory.Trim();
        //            string host = grp.FtpIp.Trim();
        //            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(host))
        //            {
        //                List<ModelFileTramKhiTuongJson> dataTramKhiTuong = new List<ModelFileTramKhiTuongJson>();
        //                List<ModelFileTramThuyVanJson> dataTramThuyVan = new List<ModelFileTramThuyVanJson>();
        //                try
        //                {
        //                    List<Site> lstSite = _siteData.GetListSite(grp.Id).ToList();
        //                    foreach (var site in lstSite)
        //                    {
        //                        var tocDoGioGiatByDevice = _reportDailyTocDoGioData.GetTocDoGioLatest(site.DeviceId.Value);
        //                        string valueHuongGio2m = "0", valueTocDoGio2m = "0", valueHuongGio2s = "0", valueTocDoGio2s = "0";
        //                        if (tocDoGioGiatByDevice != null)
        //                        {
        //                            valueTocDoGio2s = tocDoGioGiatByDevice.TocDoGioLonNhat.HasValue ? tocDoGioGiatByDevice.TocDoGioLonNhat.Value.ToString() : "0";
        //                            valueHuongGio2s = tocDoGioGiatByDevice.HuongGioCuaTocDoLonNhat.HasValue ? tocDoGioGiatByDevice.HuongGioCuaTocDoLonNhat.Value.ToString() : "0";
        //                            valueTocDoGio2m = tocDoGioGiatByDevice.TocDoGioNhoNhat.HasValue ? tocDoGioGiatByDevice.TocDoGioNhoNhat.Value.ToString() : "0";
        //                            valueHuongGio2m = tocDoGioGiatByDevice.HuongGioCuarTocDoNhoNhat.HasValue ? tocDoGioGiatByDevice.HuongGioCuarTocDoNhoNhat.Value.ToString() : "0";
        //                        }
        //                        try
        //                        {
        //                            BtsGetwayService.MongoDb.Entity.Data obs = new BtsGetwayService.MongoDb.Entity.Data();
        //                            obs = dataObservationMongoService.GetDataPagingByDeviceId(from, to, site.DeviceId.Value, 0, 1, out int total).FirstOrDefault();
        //                            if (obs != null)
        //                            {
        //                                if (site.TypeSiteId == 1)
        //                                {
        //                                    ModelFileTramKhiTuongJson dataOfSite = new ModelFileTramKhiTuongJson();
        //                                    dataOfSite.StationNo = site.DeviceId.ToString();
        //                                    dataOfSite.Datadate = long.Parse(helperUlti.DatetimeOnlyNumber(to));
        //                                    dataOfSite.DD10m = float.Parse(valueHuongGio2m);
        //                                    dataOfSite.FF10m = float.Parse(valueTocDoGio2m);
        //                                    dataOfSite.T2m = float.Parse(obs.BTI.ToString());
        //                                    dataOfSite.Rh2m = float.Parse(obs.BHU.ToString());
        //                                    dataOfSite.DxDx = float.Parse(valueHuongGio2s);
        //                                    dataOfSite.FxFx = float.Parse(valueTocDoGio2s);
        //                                    dataOfSite.Rain24h = float.Parse(obs.BAC.ToString());
        //                                    dataOfSite.PS = float.Parse(obs.BAV.ToString());
        //                                    dataOfSite.FTFT = float.Parse(obs.BWS.ToString());
        //                                    dataOfSite.DTDT = float.Parse(obs.BAP.ToString());
        //                                    dataTramKhiTuong.Add(dataOfSite);
        //                                }
        //                                else
        //                                {
        //                                    ModelFileTramThuyVanJson dataOfSite = new ModelFileTramThuyVanJson();
        //                                    dataOfSite.StationNo = site.DeviceId.ToString();
        //                                    dataOfSite.Datadate = long.Parse(helperUlti.DatetimeOnlyNumber(to));
        //                                    dataOfSite.WL = float.Parse(obs.BAF.ToString());
        //                                    dataTramThuyVan.Add(dataOfSite);
        //                                }
        //                            }


        //                        }
        //                        catch (Exception ex)
        //                        {

        //                        }

        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    //Log.Info("Nhóm: " + grp.Id + ": ERROR " + ex);
        //                }
        //                string urlFullPathFolder = appConfigPath + helperUlti.GetDay(to) + "\\";
        //                bool exists = Directory.Exists(urlFullPathFolder);
        //                if (!exists)
        //                    Directory.CreateDirectory(urlFullPathFolder);
        //                if (dataTramKhiTuong != null && dataTramKhiTuong.Count() > 0)
        //                {
        //                    string nameFile = grp.Code + "_AWS_" + helperUlti.DatetimeOnlyNumber(to) + ".json";
        //                    string path = urlFullPathFolder + nameFile;
        //                    try
        //                    {
        //                        var options = new JsonSerializerOptions { WriteIndented = true };
        //                        string jsonString = System.Text.Json.JsonSerializer.Serialize(dataTramKhiTuong, options);
        //                        // Create a new file                         
        //                        if (!File.Exists(path))
        //                        {
        //                            File.WriteAllText(path, jsonString);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                    }
        //                    try
        //                    {
        //                        GetOrCreateFolder(appConfigFolderTTTT, to, nameFile, path, userName, password, host);
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                    }

        //                }
        //                if (dataTramThuyVan != null && dataTramThuyVan.Count() > 0)
        //                {
        //                    string nameFile = grp.Code + "_WL_" + helperUlti.DatetimeOnlyNumber(to) + ".json";
        //                    string path = urlFullPathFolder + nameFile;
        //                    try
        //                    {
        //                        var options = new JsonSerializerOptions { WriteIndented = true };
        //                        string jsonString = System.Text.Json.JsonSerializer.Serialize(dataTramThuyVan, options);
        //                        if (!File.Exists(path))
        //                        {
        //                            File.WriteAllText(path, jsonString);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                    }
        //                    try
        //                    {
        //                        GetOrCreateFolder(appConfigFolderTTTT, to, nameFile, path, userName, password, host);
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                    }
        //                }
        //            }                    
        //        }
        //    }
        //    private void GetOrCreateFolder(string folderTTTT, DateTime to, string fileName, string fullPathSourceFile, string userName, string password, string host)
        //    {
        //        string urlFolder = host + folderTTTT + "/a/";
        //        bool checkExisFolder = DoesFtpDirectoryExist(urlFolder, userName, password);
        //        if (checkExisFolder == false)
        //        {
        //            bool check = CreateFTPDirectory(urlFolder, userName, password);
        //        }
        //        using (WebClient client = new WebClient())
        //        {
        //            client.Credentials = new NetworkCredential(userName, password);
        //            client.UploadFile(urlFolder + fileName, WebRequestMethods.Ftp.UploadFile, fullPathSourceFile);
        //        }
        //    }
        //    private bool CreateFile(string path, string userName, string password)
        //    {
        //        bool isCreated = true;
        //        try
        //        {
        //            WebRequest request = WebRequest.Create(path);
        //            request.Method = WebRequestMethods.Ftp.MakeDirectory;
        //            request.Credentials = new NetworkCredential(userName, password);
        //            using (var resp = (FtpWebResponse)request.GetResponse())
        //            {
        //                Console.WriteLine(resp.StatusCode);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            isCreated = false;
        //        }
        //        return isCreated;
        //    }
        //    private bool DeleteFile(string Folderpath, string userName, string password)
        //    {
        //        bool result = false;
        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Folderpath);
        //        request.Method = WebRequestMethods.Ftp.RemoveDirectory;
        //        request.Credentials = new System.Net.NetworkCredential(userName, password); ;
        //        request.GetResponse().Close();
        //        return result;
        //    }
        //    private List<string> GetAllFtpFiles(string ParentFolderpath, string userName, string password)
        //    {
        //        try
        //        {
        //            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ParentFolderpath);
        //            ftpRequest.Credentials = new NetworkCredential(userName, password);
        //            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
        //            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
        //            StreamReader streamReader = new StreamReader(response.GetResponseStream());

        //            List<string> directories = new List<string>();

        //            string line = streamReader.ReadLine();
        //            while (!string.IsNullOrEmpty(line))
        //            {
        //                var lineArr = line.Split('/');
        //                line = lineArr[lineArr.Count() - 1];
        //                directories.Add(line);
        //                line = streamReader.ReadLine();
        //            }

        //            streamReader.Close();

        //            return directories;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public bool DoesFtpDirectoryExist(string dirPath, string userName, string password)
        //    {
        //        bool isexist = false;

        //        try
        //        {
        //            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(dirPath);
        //            request.Credentials = new NetworkCredential(userName, password);
        //            request.Method = WebRequestMethods.Ftp.ListDirectory;
        //            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
        //            {
        //                isexist = true;
        //            }
        //        }
        //        catch (WebException ex)
        //        {
        //            if (ex.Response != null)
        //            {
        //                FtpWebResponse response = (FtpWebResponse)ex.Response;
        //                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
        //                {
        //                    return false;
        //                }
        //            }
        //        }
        //        return isexist;
        //    }
        //    private bool CreateFTPDirectory(string directory, string userName, string password)
        //    {

        //        try
        //        {
        //            //create the directory
        //            FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri(directory));
        //            requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
        //            requestDir.Credentials = new NetworkCredential(userName, password);
        //            requestDir.UsePassive = true;
        //            requestDir.UseBinary = true;
        //            requestDir.KeepAlive = false;
        //            FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
        //            Stream ftpStream = response.GetResponseStream();

        //            ftpStream.Close();
        //            response.Close();

        //            return true;
        //        }
        //        catch (WebException ex)
        //        {

        //            FtpWebResponse response = (FtpWebResponse)ex.Response;
        //            String status = response.StatusDescription;
        //            if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
        //            {
        //                response.Close();
        //                return true;
        //            }
        //            else
        //            {
        //                response.Close();
        //                return false;
        //            }
        //        }
        //    }
    }
}
