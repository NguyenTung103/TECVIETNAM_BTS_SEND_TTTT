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

namespace BtsGetwayService
{
    public class BtsGetway
    {
        public readonly Timer _timer;
        GroupData _groupData = new GroupData();
        SiteData _siteData = new SiteData();
        ReportDailyHuongGioData _reportDailyHuongGioData = new ReportDailyHuongGioData();
        ReportDailyTocDoGioData _reportDailyTocDoGioData = new ReportDailyTocDoGioData();
        DataObservationMongoService dataObservationMongoService = new DataObservationMongoService();
        Helper helperUlti = new Helper();
        string host = ConfigurationManager.AppSettings["Host"].ToString();
        string userName = ConfigurationManager.AppSettings["UserName"].ToString();
        string password = ConfigurationManager.AppSettings["Pass"].ToString();
        public BtsGetway()
        {
            _timer = new Timer(SendFile, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(10));
        }
        public void Start()
        {
            //Timer _timer;
            //_timer = new Timer(SendFile, null, TimeSpan.Zero,
            //    TimeSpan.FromMinutes(1));
        }
        public void Stop() { _timer.Dispose(); }
        private void SendFile(object state)
        {
            DateTime to = DateTime.Now;
            DateTime from = DateTime.Now.AddMinutes(-10);
            List<RegionalGroup> lstGroup = _groupData.GetGroupSend().ToList();
            string appConfigPath = ConfigurationManager.AppSettings["FolderLuuTruFile"].ToString();
            string appConfigFolderTTTT = ConfigurationManager.AppSettings["FolderTrungTamThongTin"].ToString();
            foreach (var grp in lstGroup)
            {
                List<ModelFileJson> dataTramKhiTuong = new List<ModelFileJson>();
                List<ModelFileJson> dataTramThuyVan = new List<ModelFileJson>();
                try
                {
                    List<Site> lstSite = _siteData.GetListSite(grp.Id).ToList();
                    foreach (var site in lstSite)
                    {
                        string content = "";
                        var tocDoGioGiatByDevice = _reportDailyTocDoGioData.GetTocDoGioLatest(site.DeviceId.Value);
                        string valueHuongGio = "0", valueTocDoGio = "0";
                        if (tocDoGioGiatByDevice != null)
                        {
                            valueTocDoGio = tocDoGioGiatByDevice.TocDoGioLonNhat.HasValue ? tocDoGioGiatByDevice.TocDoGioLonNhat.Value.ToString() : "0";
                            valueHuongGio = tocDoGioGiatByDevice.HuongGioCuaTocDoLonNhat.HasValue ? tocDoGioGiatByDevice.HuongGioCuaTocDoLonNhat.Value.ToString() : "0";
                        }
                        try
                        {
                            BtsGetwayService.MongoDb.Entity.Data obs = new BtsGetwayService.MongoDb.Entity.Data();
                            obs = dataObservationMongoService.GetDataPagingByDeviceId(from, to, site.DeviceId.Value, 0, 1, out int total).FirstOrDefault();
                            if (obs != null)
                            {
                                ModelFileJson dataOfSite = new ModelFileJson();
                                dataOfSite.StationNo = site.DeviceId.ToString();
                                dataOfSite.Datadate = long.Parse(helperUlti.DatetimeOnlyNumber(obs.DateCreate));
                                if (site.TypeSiteId == 1)
                                {
                                    dataOfSite.DD10m = float.Parse(valueHuongGio);
                                    dataOfSite.FF10m = float.Parse(valueTocDoGio);
                                    dataOfSite.T2m = float.Parse(obs.BTI.ToString());
                                    dataOfSite.Rh2m = float.Parse(obs.BHU.ToString());
                                    dataOfSite.DxDx = float.Parse(obs.BAP.ToString());
                                    dataOfSite.FxFx = float.Parse(obs.BWS.ToString());
                                    dataTramKhiTuong.Add(dataOfSite);
                                    content = helperUlti.DatetimeOnlyNumber(obs.DateCreate)
                                       + "#" + obs.Device_Id
                                       + "#" + obs.BTI
                                       + "#" + obs.BAP
                                       + "#" + obs.BWS
                                       + "#" + valueHuongGio
                                       + "#" + valueTocDoGio
                                       + "#" + obs.BAC
                                       + "#" + obs.BAV
                                       + "#0" + "#0" + "#" + obs.BHU + "#0" + "#" + obs.BAF + "#################################0#0#00#0#0#0#0#0#0#0#0#1#0#0#0#1#0#0#0#0#0#0#0####";
                                }
                                else
                                {
                                    dataOfSite.WL = float.Parse(obs.BAF.ToString());
                                    dataTramThuyVan.Add(dataOfSite);
                                    content = helperUlti.DatetimeOnlyNumber(obs.DateCreate)
                                       + "#" + obs.Device_Id
                                       + "#0#0#0#0#0#0#0#0#0#0#0#" + obs.BAF + "#################################0#0#00#0#0#0#0#0#0#0#0#1#0#0#0#1#0#0#0#0#0#0#0####";
                                }
                            }


                        }
                        catch (Exception ex)
                        {
                            //Log.Info("Nhóm: " + grp.Id + "Trạm: " + site.Id + ": ERROR " + ex);                           
                        }

                    }
                }
                catch (Exception ex)
                {
                    //Log.Info("Nhóm: " + grp.Id + ": ERROR " + ex);
                }
                string urlFullPathFolder = appConfigPath + helperUlti.GetDay(to) + "\\";
                bool exists = Directory.Exists(urlFullPathFolder);
                if (!exists)
                    Directory.CreateDirectory(urlFullPathFolder);
                if (dataTramKhiTuong != null && dataTramKhiTuong.Count() > 0)
                {
                    string nameFile = grp.Code + "_AWS_" + helperUlti.DatetimeOnlyNumber(to) + ".json";

                    string path = urlFullPathFolder + nameFile;
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {

                        JsonSerializer serializer = new JsonSerializer();
                        //serialize object directly into file stream
                        serializer.Serialize(sw, dataTramKhiTuong);
                        string From = path;
                        //string pathCreateFolder1 = "/ODA-KOREA" + "/" + to.ToString("dd-MM-yyyy");
                        //string pathCreateFolder2 = "/ODA-KOREA" + "/" + to.ToString("dd-MM-yyyy") + "/" + helperUlti.CreateTitle(site.Name);
                        //GetOrCreateFolder(host + pathCreateFolder1);                       
                        string To = host + appConfigFolderTTTT + "/" + helperUlti.GetDay(to) + "/";
                        GetOrCreateFolder(To);
                        using (WebClient client = new WebClient())
                        {
                            client.Credentials = new NetworkCredential(userName, password);
                            client.UploadFile(To + nameFile, WebRequestMethods.Ftp.UploadFile, From);
                        }
                    }
                }
                if (dataTramThuyVan != null && dataTramThuyVan.Count() > 0)
                {
                    string nameFile = grp.Code + "_WL_" + helperUlti.DatetimeOnlyNumber(to) + ".json";                   
                    string path = urlFullPathFolder + nameFile;
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {                        
                        if (!File.Exists(path))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            //serialize object directly into file stream
                            serializer.Serialize(sw, dataTramThuyVan);
                            string From = path;
                            //string pathCreateFolder1 = "/ODA-KOREA" + "/" + to.ToString("dd-MM-yyyy");
                            //string pathCreateFolder2 = "/ODA-KOREA" + "/" + to.ToString("dd-MM-yyyy") + "/" + helperUlti.CreateTitle(site.Name);
                            //GetOrCreateFolder(host + pathCreateFolder1);                       
                            string To = host + appConfigFolderTTTT + "/" + helperUlti.GetDay(to) + "/";
                            GetOrCreateFolder(To);
                            using (WebClient client = new WebClient())
                            {
                                client.Credentials = new NetworkCredential(userName, password);
                                client.UploadFile(To + nameFile, WebRequestMethods.Ftp.UploadFile, From);
                            }
                        }
                    }
                }
            }            
        }
        private void GetOrCreateFolder(string url)
        {
            bool checkExisFolder = DoesFtpDirectoryExist(url);
            if (checkExisFolder == false)
            {
                bool check = CreateFTPDirectory(url);
            }
        }
        private bool CreateFile(string path)
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
                isCreated = false;
            }
            return isCreated;
        }
        private bool DeleteFile(string Folderpath)
        {
            bool result = false;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Folderpath);
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            request.Credentials = new System.Net.NetworkCredential(userName, password); ;
            request.GetResponse().Close();
            return result;
        }
        private List<string> GetAllFtpFiles(string ParentFolderpath)
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
                throw ex;
            }
        }
        public bool DoesFtpDirectoryExist(string dirPath)
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
        private bool CreateFTPDirectory(string directory)
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
