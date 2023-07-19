using bts.udpgateway;
using BtsGetwayService;
using Core.Helper;
using Core.Model;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Udp
{
    public class ReportS10Service : IReportS10Service
    {
        public readonly IReportS10Data _reportS10Data;
        public readonly ISiteData _siteData;
        public readonly AppApiWatecSetting _appApiWatecSetting;
        public ReportS10Service(IReportS10Data reportS10Data, ISiteData siteData, IOptions<AppApiWatecSetting> appApiWatecSetting)
        {
            _reportS10Data = reportS10Data;
            _siteData = siteData;
            _appApiWatecSetting = appApiWatecSetting.Value;
        }
        public ReportS10 InitS10(string message)
        {
            ReportS10 dataMongo = new ReportS10();
            IDictionary<string, string> dictionary1 = new Dictionary<string, string>();
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>()
              {
                {
                  "EQID",
                  "ID thiet bi"
                },
                {
                  "S10",
                  "Lưu vào DB"
                },
                {
                  "DATE",
                  "Thời gian"
                },
                {
                  "MTI",
                  "Nhiet do hien tai"
                },
                {
                  "MTX",
                  "Nhiet do lon nhat"
                },
                {
                  "MTM",
                  "Nhiet do nho nhat"
                },
                {
                  "MHU",
                  "Do am hien tai"
                },
                {
                  "MHX",
                  "Do am lon nhat"
                },
                {
                  "MHM",
                  "Do am nho nhat"
                },
                {
                  "MAV",
                  "Ap suat hien tai"
                },
                {
                  "MSM",
                  "Toc do gio lon nhat trong 2p (chu ky tinh 10p)"
                },
                {
                  "MDM",
                  "Huong gio cua toc do gio lon nhat trong 2p"
                },
                {
                  "MSS",
                  "Toc do gio lon nhat trong 2s (chu ky tinh 10p)"
                },
                {
                  "MDS",
                  "Huong gio cua toc do gio lon nhat trong 2s "
                },
                {
                  "MHR",
                  "Tgian xay ra toc do gio lon nhat trong 2s"
                },
                {
                  "MRT",
                  "Luong mua trong 10p"
                },
                {
                  "MRH",
                  "Luong mua trong 1h"
                },
                {
                  "MRC",
                  "Luong mua tu 19h den 19h"
                },
                {
                  "MRB",
                  "Luong mua tu 00h den 00h"
                },
                {
                  "MVC",
                  "Nguon cung cap 12V; dien ap 12V"
               }
                ,
                {
                  "MFX",
                  "Toc do gio TB 2m cuoi"
               },
                {
                  "MFD",
                  "Huong gio TB 2m cuoi"
               },
                {
                  "MHD",
                  "Ngay"
               },
                {
                  "MFL",
                  "Water Level, tinh den mm, co offset do cao"
               },
                {
                  "MFF",
                  "Flow (m3/s)"
               },
                {
                  "MFV",
                  "Velocity (m/s)"
               }
              };
            string[] strArray = message.Split(';');
            string s1 = strArray[0].Split('=')[1];
            dataMongo.DeviceId = int.Parse(s1);
            string str1 = strArray[1];
            string str2 = strArray[2];
            try
            {
                dataMongo.DateCreate = DateTime.ParseExact(str2, "HH:mm:ss-dd/MM/yy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {

            }
            dictionary1.Add(new KeyValuePair<string, string>("EQID", s1));
            dictionary1.Add(new KeyValuePair<string, string>("S10", "1"));
            dictionary1.Add(new KeyValuePair<string, string>("DATE", str2));
            for (int index = 3; index < strArray.Length - 2; ++index)
            {
                try
                {
                    strArray[index] = strArray[index].Trim();
                    string str3 = strArray[index].Substring(0, 3);
                    string s2 = double.Parse(strArray[index].Substring(3)).ToString();
                    switch (str3)
                    {
                        case "MTI":
                            dataMongo.MTI = GetValue(s2);
                            break;
                        case "MTX":
                            dataMongo.MTX = GetValue(s2);
                            break;
                        case "MTM":
                            dataMongo.MTM = GetValue(s2);
                            break;
                        case "MHU":
                            dataMongo.MHU = GetValue(s2);
                            break;
                        case "MHX":
                            dataMongo.MHX = GetValue(s2);
                            break;
                        case "MHM":
                            dataMongo.MHM = GetValue(s2);
                            break;
                        case "MAV":
                            dataMongo.MAV = GetValue(s2);
                            break;
                        case "MSM":
                            dataMongo.MSM = GetValue(s2);
                            break;
                        case "MDM":
                            dataMongo.MDM = GetValue(s2);
                            break;
                        case "MSS":
                            dataMongo.MSS = GetValue(s2);
                            break;
                        case "MDS":
                            dataMongo.MDS = GetValue(s2);
                            break;
                        case "MHR":
                            dataMongo.MHR = GetValue(s2);
                            break;
                        case "MRT":
                            dataMongo.MRT = GetValue(s2);
                            break;
                        case "MRH":
                            dataMongo.MRH = GetValue(s2);
                            break;
                        case "MRC":
                            dataMongo.MRC = GetValue(s2);
                            break;
                        case "MRB":
                            dataMongo.MRB = GetValue(s2);
                            break;
                        case "MVC":
                            dataMongo.MVC = GetValue(s2);
                            break;
                        case "MFX":
                            dataMongo.MFX = GetValue(s2);
                            break;
                        case "MFD":
                            dataMongo.MFD = GetValue(s2);
                            break;
                        case "MHD":
                            dataMongo.MHD = GetValue(s2);
                            break;
                        case "MFL":
                            dataMongo.MFL = GetValue(s2);
                            break;
                        case "MFF":
                            dataMongo.MFF = GetValue(s2);
                            break;
                        case "MFV":
                            dataMongo.MFV = GetValue(s2);
                            break;
                            // Hải văn
                        case "MTS":
                            dataMongo.MFL = GetValue(s2);
                            break;
                        case "MEC":
                            dataMongo.MFF = GetValue(s2);
                            break;
                        case "MEV":
                            dataMongo.MFV = GetValue(s2);
                            break;
                    }
                }
                catch (Exception)
                {                   
                    return dataMongo;
                }
            }
            return dataMongo;
        }
        private double? GetValue(string input)
        {
            double? result = null;
            if (!string.IsNullOrEmpty(input))
            {
                result = double.Parse(input, (IFormatProvider)CultureInfo.InvariantCulture);
            }
            return result;
        }
        public bool InsertS10(ReportS10 reportS10)
        {
            WatecS10Model modelFileS10Json = new WatecS10Model();          
            var siteByĐeviceId = _siteData.GetSite(reportS10.DeviceId.Value).FirstOrDefault();
            if (siteByĐeviceId != null)
            {
                try
                {
                    siteByĐeviceId.IsActive = true;
                    var tasks = new List<Task>();
                    tasks.Add(Task.Run(() => _siteData.UpdateStatusActive(reportS10.DeviceId.Value)));
                    tasks.Add(Task.Run(() => _reportS10Data.Insert(reportS10)));
                    if (_appApiWatecSetting.IsSendWatec == 1)
                    {
                        modelFileS10Json.sid = reportS10.DeviceId.Value.ToString("D10");
                        modelFileS10Json.from = reportS10.DateCreate.Value.ToString("yyyy-MM-dd HH:mm");
                        modelFileS10Json.to = reportS10.DateCreate.Value.ToString("yyyy-MM-dd HH:mm");
                        modelFileS10Json.val = Utility.CheckNull(reportS10.MRT);
                        tasks.Add(Task.Run(() => ApiSend.PostDataObject(_appApiWatecSetting.ApiWatecKey, _appApiWatecSetting.ApiWatecUrl, modelFileS10Json)));
                    }
                    Task t = Task.WhenAll(tasks);
                    t.Wait();
                }
                catch (Exception)
                {

                }
                finally
                {
                }

            }
            return true;
        }
    }
}
