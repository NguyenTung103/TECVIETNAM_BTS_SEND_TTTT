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

namespace BtsGetwayService
{
    public class BtsGetway
    {

        public readonly IGroupData _groupData;
        public readonly ISiteData _siteData;
        public readonly IReportDailyHuongGioData _reportDailyHuongGioData;
        public readonly IReportDailyTocDoGioData _reportDailyTocDoGioData;
        public readonly IDataObservationService _dataObservationMongoService;
        public readonly AppSetting _appSetting;
        Helper helperUlti = new Helper();
        public BtsGetway(
            IOptions<AppSetting> option,
            IGroupData groupData,
            ISiteData siteData,
            IDataObservationService dataObservationMongoService,
            IReportDailyTocDoGioData reportDailyTocDoGioData,
            IReportDailyHuongGioData reportDailyHuongGioData)
        {
            _appSetting = option.Value;
            _groupData = groupData;
            _siteData = siteData;
            _reportDailyHuongGioData = reportDailyHuongGioData;
            _reportDailyTocDoGioData = reportDailyTocDoGioData;
            _dataObservationMongoService = dataObservationMongoService;
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
                        List<ModelFileTramKhiTuongJson> dataTramKhiTuong = new List<ModelFileTramKhiTuongJson>();
                        List<ModelFileTramThuyVanJson> dataTramThuyVan = new List<ModelFileTramThuyVanJson>();
                        try
                        {
                            List<Site> lstSite = _siteData.GetListSite(grp.Id).ToList();
                            foreach (var site in lstSite)
                            {
                                var tocDoGioGiatByDevice = _reportDailyTocDoGioData.GetTocDoGioLatest(site.DeviceId.Value);
                                string valueHuongGio2m = "0", valueTocDoGio2m = "0", valueHuongGio2s = "0", valueTocDoGio2s = "0";
                                if (tocDoGioGiatByDevice != null)
                                {
                                    valueTocDoGio2s = tocDoGioGiatByDevice.TocDoGioLonNhat.HasValue ? tocDoGioGiatByDevice.TocDoGioLonNhat.Value.ToString() : "0";
                                    valueHuongGio2s = tocDoGioGiatByDevice.HuongGioCuaTocDoLonNhat.HasValue ? tocDoGioGiatByDevice.HuongGioCuaTocDoLonNhat.Value.ToString() : "0";
                                    valueTocDoGio2m = tocDoGioGiatByDevice.TocDoGioNhoNhat.HasValue ? tocDoGioGiatByDevice.TocDoGioNhoNhat.Value.ToString() : "0";
                                    valueHuongGio2m = tocDoGioGiatByDevice.HuongGioCuarTocDoNhoNhat.HasValue ? tocDoGioGiatByDevice.HuongGioCuarTocDoNhoNhat.Value.ToString() : "0";
                                }
                                try
                                {
                                    BtsGetwayService.MongoDb.Entity.Data obs = new BtsGetwayService.MongoDb.Entity.Data();
                                    obs = _dataObservationMongoService.GetDataPagingByDeviceId(from, to, site.DeviceId.Value, 0, 1, out int total).FirstOrDefault();
                                    if (obs != null)
                                    {
                                        if (site.TypeSiteId == 1)
                                        {
                                            ModelFileTramKhiTuongJson dataOfSite = new ModelFileTramKhiTuongJson();
                                            dataOfSite.StationNo = site.DeviceId.ToString();
                                            dataOfSite.Datadate = long.Parse(helperUlti.DatetimeOnlyNumber(to));
                                            dataOfSite.DD10m = float.Parse(valueHuongGio2m);
                                            dataOfSite.FF10m = float.Parse(valueTocDoGio2m);
                                            dataOfSite.T2m = float.Parse(obs.BTI.ToString());
                                            dataOfSite.Rh2m = float.Parse(obs.BHU.ToString());
                                            dataOfSite.DxDx = float.Parse(valueHuongGio2s);
                                            dataOfSite.FxFx = float.Parse(valueTocDoGio2s);
                                            dataOfSite.Rain24h = float.Parse(obs.BAC.ToString());
                                            dataOfSite.PS = float.Parse(obs.BAV.ToString());
                                            dataOfSite.FTFT = float.Parse(obs.BWS.ToString());
                                            dataOfSite.DTDT = float.Parse(obs.BAP.ToString());
                                            dataTramKhiTuong.Add(dataOfSite);
                                        }
                                        else
                                        {
                                            ModelFileTramThuyVanJson dataOfSite = new ModelFileTramThuyVanJson();
                                            dataOfSite.StationNo = site.DeviceId.ToString();
                                            dataOfSite.Datadate = long.Parse(helperUlti.DatetimeOnlyNumber(to));
                                            dataOfSite.WL = float.Parse(obs.BAF.ToString());
                                            dataTramThuyVan.Add(dataOfSite);
                                        }
                                    }


                                }
                                catch (Exception ex)
                                {

                                }

                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        string urlFullPathFolder = appConfigPath + helperUlti.GetDay(to) + "\\";
                        bool exists = Directory.Exists(urlFullPathFolder);
                        if (!exists)
                            Directory.CreateDirectory(urlFullPathFolder);
                        if (dataTramKhiTuong != null && dataTramKhiTuong.Count() > 0)
                        {
                            string nameFile = grp.Code + "_AWS_" + helperUlti.DatetimeOnlyNumber(to) + ".json";
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

                            }
                        }
                        if (dataTramThuyVan != null && dataTramThuyVan.Count() > 0)
                        {
                            string nameFile = grp.Code + "_WL_" + helperUlti.DatetimeOnlyNumber(to) + ".json";
                            string path = urlFullPathFolder + nameFile;
                            try
                            {
                                var options = new JsonSerializerOptions { WriteIndented = true };
                                string jsonString = System.Text.Json.JsonSerializer.Serialize(dataTramThuyVan, options);
                                if (!File.Exists(path))
                                {
                                    File.WriteAllText(path, jsonString);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
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
