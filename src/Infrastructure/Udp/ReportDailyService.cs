using bts.udpgateway;
using Core.MSSQL.Responsitory.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Udp
{
    public class ReportDailyService : IReportDailyService
    {
        public readonly IReportDailyNhietDoData _reportDailyNhietDoData;
        public readonly IReportDailyDoAmData _reportDailyDoAmData;
        public readonly IReportDailyLuongMuaData _reportDailyLuongMuaData;
        public readonly IReportDailyApSuatData _reportDailyApSuatData;
        public readonly IReportDailyTocDoDongChayData _reportDailyTocDoDongChayData;
        public readonly IReportDailyMucNuocData _reportDailyMucNuocData;
        public readonly IReportDailyLuuLuongDongChayData _reportDailyLuuLuongDongChayData;
        public readonly IReportDailyBucXaMatTroiData _reportDailyBucXaMatTroiData;
        public readonly IReportDailyHuongGioData _reportDailyHuongGioData;
        public readonly IReportDailyTocDoGioData _reportDailyTocDoGioData;
        public ReportDailyService(IReportDailyApSuatData reportDailyApSuatData,
            IReportDailyLuongMuaData reportDailyLuongMuaData,
            IReportDailyMucNuocData reportDailyMucNuocData,
            IReportDailyTocDoDongChayData reportDailyTocDoDongChayData,
            IReportDailyLuuLuongDongChayData reportDailyLuuLuongDongChayData,
            IReportDailyBucXaMatTroiData reportDailyBucXaMatTroiData,
            IReportDailyTocDoGioData reportDailyTocDoGioData,
            IReportDailyNhietDoData reportDailyNhietDoData,
            IReportDailyDoAmData reportDailyDoAmData,
            IReportDailyHuongGioData reportDailyHuongGioData)
        {
            _reportDailyDoAmData = reportDailyDoAmData;
            _reportDailyLuongMuaData = reportDailyLuongMuaData;
            _reportDailyApSuatData = reportDailyApSuatData;
            _reportDailyNhietDoData = reportDailyNhietDoData;
            _reportDailyMucNuocData = reportDailyMucNuocData;
            _reportDailyTocDoDongChayData = reportDailyTocDoDongChayData;
            _reportDailyLuuLuongDongChayData = reportDailyLuuLuongDongChayData;
            _reportDailyBucXaMatTroiData = reportDailyBucXaMatTroiData;
            _reportDailyHuongGioData = reportDailyHuongGioData;
            _reportDailyTocDoGioData = reportDailyTocDoGioData;
        }

        #region report
        public void ReportDaily(string message)
        {
            var obj = message.Split(';');
            var data = obj[3].Split(',');
            // Lấy deviceId
            int deviceId = Convert.ToInt32(obj[0].Split('=')[1]);
            // Lấy kiểu report
            string typeReport = obj[1];
            //Chuyen doi ngay request
            string date = obj[2];
            string[] gioRq = obj[2].Split('-')[0].Split(':');
            string[] ngayRq = obj[2].Split('-')[1].Split('/');
            //Ngay du lieu
            DateTime dateReport = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]));
            DateTime dateRequestReport = new DateTime(Convert.ToInt32("20" + ngayRq[2]), Convert.ToInt32(ngayRq[1]), Convert.ToInt32(ngayRq[0]), Convert.ToInt32(gioRq[0]), Convert.ToInt32(gioRq[1]), Convert.ToInt32(gioRq[2]));

            //Data
            if (obj[1].Contains("RP0"))
            {
                InsertDailyNhietDo(data, obj, deviceId, typeReport, dateReport, dateRequestReport, message);
            }
            else if (obj[1].Contains("RP1"))
            {
                InsertDailyDoAm(data, obj, deviceId, typeReport, dateReport, dateRequestReport, message);
            }
            else if (obj[1].Contains("RP2"))
            {
                InsertDailyApSuat(data, obj, deviceId, typeReport, dateReport, dateRequestReport, message);
            }
            else if (obj[1].Contains("RP3"))
            {
                InsertDailyLuongMua(data, obj, deviceId, typeReport, dateReport, dateRequestReport, message);
            }
            else if (obj[1].Contains("RP4"))
            {
                InsertDailyHuongGio(data, obj, deviceId, typeReport, dateReport, dateRequestReport, message);
            }
            else if (obj[1].Contains("RP5"))
            {
                InsertDailyMucNuoc(data, obj, deviceId, typeReport, dateReport, dateRequestReport, message);
            }
            else if (obj[1].Contains("RP6"))
            {
                InsertDailyTocGio(data, obj, deviceId, typeReport, dateReport, dateRequestReport, message);
            }
            else if (obj[1].Contains("RP7"))
            {
                InsertDailyTocDoDongChay(data, obj, deviceId, typeReport, dateReport, dateRequestReport, message);
            }
            else if (obj[1].Contains("RP8"))
            {
                InsertDailyLuuLuongDongChay(data, obj, deviceId, typeReport, dateReport, dateRequestReport, message);
            }
            else if (obj[1].Contains("RP9"))
            {
                InsertDailyBucXaMatTroi(data, obj, deviceId, typeReport, dateReport, dateRequestReport, message);
            }
        }
        /// <summary>
        /// Trả về giá trị double cho dữ liệu đầu vào là chuỗi
        /// </summary>
        /// <param name="input">giá trị string của chuỗi muốn chuyển đổi sang double</param>
        /// <param name="code">mã biểu đồ</param>
        /// <returns></returns>
        private static double CheckValueReport(string input, string code)
        {
            double result = 0;
            if (code.Trim() == "RP0" || code.Trim() == "RP1" || code.Trim() == "RP2" || code.Trim() == "RP6")
            {
                if (input == "*")
                {
                    return result;

                }
                else
                {

                    result = Convert.ToDouble(input.Substring(0, 5) + "." + input.Substring(5, 1));
                    return result;
                }
            }
            else if (code.Trim() == "RP3" || code.Trim() == "RP4")
            {
                if (input == "*")
                {
                    return result;
                }
                else
                {
                    result = float.Parse(input);
                }
                return result;
            }
            else if (code.Trim() == "RP5")
            {
                if (input == "*")
                {
                    return result;
                }
                else
                {
                    result = Convert.ToDouble(input.Substring(0, 3) + "." + input.Substring(3, 3));
                }

                return result;
            }
            else if (code.Trim() == "RP7" || code.Trim() == "RP9")
            {
                if (input == "*")
                {
                    return result;
                }
                else
                {
                    result = Convert.ToDouble(input);
                }

                return result;
            }
            return result;

        }
        /// <summary>
        /// Chèn dữ liệu vào bảng nhiệt độ
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obj"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeCode"></param>
        /// <param name="dateReport"></param>
        /// <param name="dateRequest"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool InsertDailyNhietDo(string[] data, string[] obj, int deviceId, string typeCode, DateTime dateReport, DateTime dateRequest, string message)
        {
            ReportDailyNhietDo report = new ReportDailyNhietDo();
            report.DeviceId = deviceId;
            report.ReportTypeCode = typeCode;
            report.DateReport = dateReport;
            report.DateRequestReport = dateRequest;
            report.ContentReport = message;
            try
            {
                if (data[27] != "*")
                {
                    report.TimeMaxValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[27]), Convert.ToInt32(data[28]), 00);
                }
                if (data[30] != "*")
                {
                    report.TimeMinValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[30]), Convert.ToInt32(data[31]), 00);
                }
            }
            catch (Exception)
            {

            }
            report.MaxValue = CheckValueReport(data[26], obj[1]);
            report.MinValue = CheckValueReport(data[29], obj[1]);
            report.Distance1 = CheckValueReport(data[2], obj[1]);
            report.Distance2 = CheckValueReport(data[3], obj[1]);
            report.Distance3 = CheckValueReport(data[4], obj[1]);
            report.Distance4 = CheckValueReport(data[5], obj[1]);
            report.Distance5 = CheckValueReport(data[6], obj[1]);
            report.Distance6 = CheckValueReport(data[7], obj[1]);
            report.Distance7 = CheckValueReport(data[8], obj[1]);
            report.Distance8 = CheckValueReport(data[9], obj[1]);
            report.Distance9 = CheckValueReport(data[10], obj[1]);
            report.Distance10 = CheckValueReport(data[11], obj[1]);
            report.Distance11 = CheckValueReport(data[12], obj[1]);
            report.Distance12 = CheckValueReport(data[13], obj[1]);
            report.Distance13 = CheckValueReport(data[14], obj[1]);
            report.Distance14 = CheckValueReport(data[15], obj[1]);
            report.Distance15 = CheckValueReport(data[16], obj[1]);
            report.Distance16 = CheckValueReport(data[17], obj[1]);
            report.Distance17 = CheckValueReport(data[18], obj[1]);
            report.Distance18 = CheckValueReport(data[19], obj[1]);
            report.Distance19 = CheckValueReport(data[20], obj[1]);
            report.Distance20 = CheckValueReport(data[21], obj[1]);
            report.Distance21 = CheckValueReport(data[22], obj[1]);
            report.Distance22 = CheckValueReport(data[23], obj[1]);
            report.Distance23 = CheckValueReport(data[24], obj[1]);
            report.Distance24 = CheckValueReport(data[25], obj[1]);
            return _reportDailyNhietDoData.Insert(report);
        }
        /// <summary>
        /// Chèn dữ liệu vào bảng Độ ẩm
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obj"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeCode"></param>
        /// <param name="dateReport"></param>
        /// <param name="dateRequest"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool InsertDailyDoAm(string[] data, string[] obj, int deviceId, string typeCode, DateTime dateReport, DateTime dateRequest, string message)
        {
            ReportDailyDoAm report = new ReportDailyDoAm();
            report.DeviceId = deviceId;
            report.ReportTypeCode = typeCode;
            report.DateReport = dateReport;
            report.DateRequestReport = dateRequest;
            report.ContentReport = message;
            try
            {
                if (data[27] != "*")
                {
                    report.TimeMaxValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[27]), Convert.ToInt32(data[28]), 00);
                }
                if (data[30] != "*")
                {
                    report.TimeMinValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[30]), Convert.ToInt32(data[31]), 00);
                }
            }
            catch (Exception)
            {

            }
            report.MaxValue = CheckValueReport(data[26], obj[1]);
            report.MinValue = CheckValueReport(data[29], obj[1]);
            report.Distance1 = CheckValueReport(data[2], obj[1]);
            report.Distance2 = CheckValueReport(data[3], obj[1]);
            report.Distance3 = CheckValueReport(data[4], obj[1]);
            report.Distance4 = CheckValueReport(data[5], obj[1]);
            report.Distance5 = CheckValueReport(data[6], obj[1]);
            report.Distance6 = CheckValueReport(data[7], obj[1]);
            report.Distance7 = CheckValueReport(data[8], obj[1]);
            report.Distance8 = CheckValueReport(data[9], obj[1]);
            report.Distance9 = CheckValueReport(data[10], obj[1]);
            report.Distance10 = CheckValueReport(data[11], obj[1]);
            report.Distance11 = CheckValueReport(data[12], obj[1]);
            report.Distance12 = CheckValueReport(data[13], obj[1]);
            report.Distance13 = CheckValueReport(data[14], obj[1]);
            report.Distance14 = CheckValueReport(data[15], obj[1]);
            report.Distance15 = CheckValueReport(data[16], obj[1]);
            report.Distance16 = CheckValueReport(data[17], obj[1]);
            report.Distance17 = CheckValueReport(data[18], obj[1]);
            report.Distance18 = CheckValueReport(data[19], obj[1]);
            report.Distance19 = CheckValueReport(data[20], obj[1]);
            report.Distance20 = CheckValueReport(data[21], obj[1]);
            report.Distance21 = CheckValueReport(data[22], obj[1]);
            report.Distance22 = CheckValueReport(data[23], obj[1]);
            report.Distance23 = CheckValueReport(data[24], obj[1]);
            report.Distance24 = CheckValueReport(data[25], obj[1]);
            return _reportDailyDoAmData.Insert(report);
        }
        /// <summary>
        /// Chèn dữ liệu vào bảng Áp suất
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obj"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeCode"></param>
        /// <param name="dateReport"></param>
        /// <param name="dateRequest"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool InsertDailyApSuat(string[] data, string[] obj, int deviceId, string typeCode, DateTime dateReport, DateTime dateRequest, string message)
        {
            ReportDailyApSuat report = new ReportDailyApSuat();
            report.DeviceId = deviceId;
            report.ReportTypeCode = typeCode;
            report.DateReport = dateReport;
            report.DateRequestReport = dateRequest;
            report.ContentReport = message;
            try
            {
                if (data[27] != "*")
                {
                    report.TimeMaxValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[27]), Convert.ToInt32(data[28]), 00);
                }
                if (data[30] != "*")
                {
                    report.TimeMinValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[30]), Convert.ToInt32(data[31]), 00);
                }
            }
            catch (Exception)
            {

            }
            report.MaxValue = CheckValueReport(data[26], obj[1]);
            report.MinValue = CheckValueReport(data[29], obj[1]);
            report.Distance1 = CheckValueReport(data[2], obj[1]);
            report.Distance2 = CheckValueReport(data[3], obj[1]);
            report.Distance3 = CheckValueReport(data[4], obj[1]);
            report.Distance4 = CheckValueReport(data[5], obj[1]);
            report.Distance5 = CheckValueReport(data[6], obj[1]);
            report.Distance6 = CheckValueReport(data[7], obj[1]);
            report.Distance7 = CheckValueReport(data[8], obj[1]);
            report.Distance8 = CheckValueReport(data[9], obj[1]);
            report.Distance9 = CheckValueReport(data[10], obj[1]);
            report.Distance10 = CheckValueReport(data[11], obj[1]);
            report.Distance11 = CheckValueReport(data[12], obj[1]);
            report.Distance12 = CheckValueReport(data[13], obj[1]);
            report.Distance13 = CheckValueReport(data[14], obj[1]);
            report.Distance14 = CheckValueReport(data[15], obj[1]);
            report.Distance15 = CheckValueReport(data[16], obj[1]);
            report.Distance16 = CheckValueReport(data[17], obj[1]);
            report.Distance17 = CheckValueReport(data[18], obj[1]);
            report.Distance18 = CheckValueReport(data[19], obj[1]);
            report.Distance19 = CheckValueReport(data[20], obj[1]);
            report.Distance20 = CheckValueReport(data[21], obj[1]);
            report.Distance21 = CheckValueReport(data[22], obj[1]);
            report.Distance22 = CheckValueReport(data[23], obj[1]);
            report.Distance23 = CheckValueReport(data[24], obj[1]);
            report.Distance24 = CheckValueReport(data[25], obj[1]);
            return _reportDailyApSuatData.Insert(report);
        }
        /// <summary>
        /// Chèn dữ liệu vào bảng Lượng Mưa
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obj"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeCode"></param>
        /// <param name="dateReport"></param>
        /// <param name="dateRequest"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool InsertDailyLuongMua(string[] data, string[] obj, int deviceId, string typeCode, DateTime dateReport, DateTime dateRequest, string message)
        {
            ReportDailyLuongMua report = new ReportDailyLuongMua();
            report.DeviceId = deviceId;
            report.ReportTypeCode = typeCode;
            report.DateReport = dateReport;
            report.DateRequestReport = dateRequest;
            report.ContentReport = message;
            try
            {
                if (data[27] != "*")
                {
                    report.TimeMaxValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[27]), Convert.ToInt32(data[28]), 00);
                }
                if (data[30] != "*")
                {
                    report.TimeMinValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[30]), Convert.ToInt32(data[31]), 00);
                }
            }
            catch (Exception)
            {

            }
            report.TongLuongMua = CheckValueReport(data[26], obj[1]);
            report.Distance1 = CheckValueReport(data[2], obj[1]);
            report.Distance2 = CheckValueReport(data[3], obj[1]);
            report.Distance3 = CheckValueReport(data[4], obj[1]);
            report.Distance4 = CheckValueReport(data[5], obj[1]);
            report.Distance5 = CheckValueReport(data[6], obj[1]);
            report.Distance6 = CheckValueReport(data[7], obj[1]);
            report.Distance7 = CheckValueReport(data[8], obj[1]);
            report.Distance8 = CheckValueReport(data[9], obj[1]);
            report.Distance9 = CheckValueReport(data[10], obj[1]);
            report.Distance10 = CheckValueReport(data[11], obj[1]);
            report.Distance11 = CheckValueReport(data[12], obj[1]);
            report.Distance12 = CheckValueReport(data[13], obj[1]);
            report.Distance13 = CheckValueReport(data[14], obj[1]);
            report.Distance14 = CheckValueReport(data[15], obj[1]);
            report.Distance15 = CheckValueReport(data[16], obj[1]);
            report.Distance16 = CheckValueReport(data[17], obj[1]);
            report.Distance17 = CheckValueReport(data[18], obj[1]);
            report.Distance18 = CheckValueReport(data[19], obj[1]);
            report.Distance19 = CheckValueReport(data[20], obj[1]);
            report.Distance20 = CheckValueReport(data[21], obj[1]);
            report.Distance21 = CheckValueReport(data[22], obj[1]);
            report.Distance22 = CheckValueReport(data[23], obj[1]);
            report.Distance23 = CheckValueReport(data[24], obj[1]);
            report.Distance24 = CheckValueReport(data[25], obj[1]);
            return _reportDailyLuongMuaData.Insert(report);
        }
        /// <summary>
        /// Chèn dữ liệu vào bảng hướng gió
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obj"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeCode"></param>
        /// <param name="dateReport"></param>
        /// <param name="dateRequest"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool InsertDailyHuongGio(string[] data, string[] obj, int deviceId, string typeCode, DateTime dateReport, DateTime dateRequest, string message)
        {
            ReportDailyHuongGio report = new ReportDailyHuongGio();
            report.DeviceId = deviceId;
            report.ReportTypeCode = typeCode;
            report.DateReport = dateReport;
            report.DateRequestReport = dateRequest;
            report.ContentReport = message;
            try
            {
                if (data[27] != "*")
                {
                    report.TimeMaxValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[27]), Convert.ToInt32(data[28]), 00);
                }
                if (data[30] != "*")
                {
                    report.TimeMinValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[30]), Convert.ToInt32(data[31]), 00);
                }
            }
            catch (Exception)
            {

            }
            report.HuongGio1 = int.Parse(data[2].Substring(0, 2));
            report.HuongGio2 = int.Parse(data[3].Substring(0, 2));
            report.HuongGio3 = int.Parse(data[4].Substring(0, 2));
            report.HuongGio4 = int.Parse(data[5].Substring(0, 2));
            report.HuongGio5 = int.Parse(data[6].Substring(0, 2));
            report.HuongGio6 = int.Parse(data[7].Substring(0, 2));
            report.HuongGio7 = int.Parse(data[8].Substring(0, 2));
            report.HuongGio8 = int.Parse(data[9].Substring(0, 2));
            report.Distance1 = CheckValueReport(data[2], obj[1]);
            report.Distance2 = CheckValueReport(data[3], obj[1]);
            report.Distance3 = CheckValueReport(data[4], obj[1]);
            report.Distance4 = CheckValueReport(data[5], obj[1]);
            report.Distance5 = CheckValueReport(data[6], obj[1]);
            report.Distance6 = CheckValueReport(data[7], obj[1]);
            report.Distance7 = CheckValueReport(data[8], obj[1]);
            report.Distance8 = CheckValueReport(data[9], obj[1]);
            report.Distance9 = CheckValueReport(data[10], obj[1]);
            report.Distance10 = CheckValueReport(data[11], obj[1]);
            report.Distance11 = CheckValueReport(data[12], obj[1]);
            report.Distance12 = CheckValueReport(data[13], obj[1]);
            report.Distance13 = CheckValueReport(data[14], obj[1]);
            report.Distance14 = CheckValueReport(data[15], obj[1]);
            report.Distance15 = CheckValueReport(data[16], obj[1]);
            report.Distance16 = CheckValueReport(data[17], obj[1]);
            report.Distance17 = CheckValueReport(data[18], obj[1]);
            report.Distance18 = CheckValueReport(data[19], obj[1]);
            report.Distance19 = CheckValueReport(data[20], obj[1]);
            report.Distance20 = CheckValueReport(data[21], obj[1]);
            report.Distance21 = CheckValueReport(data[22], obj[1]);
            report.Distance22 = CheckValueReport(data[23], obj[1]);
            report.Distance23 = CheckValueReport(data[24], obj[1]);
            report.Distance24 = CheckValueReport(data[25], obj[1]);
            report.HuongGioDacTrungNhieuNhat = CheckValueReport(data[26], obj[1]);
            report.HuongGioDacTrungNhieuThuHai = CheckValueReport(data[29], obj[1]);
            return _reportDailyHuongGioData.Insert(report);
        }
        /// <summary>
        /// Chèn dữ liệu vào bảng mực nước
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obj"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeCode"></param>
        /// <param name="dateReport"></param>
        /// <param name="dateRequest"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool InsertDailyMucNuoc(string[] data, string[] obj, int deviceId, string typeCode, DateTime dateReport, DateTime dateRequest, string message)
        {
            ReportDailyMucNuoc report = new ReportDailyMucNuoc();
            report.DeviceId = deviceId;
            report.ReportTypeCode = typeCode;
            report.DateReport = dateReport;
            report.DateRequestReport = dateRequest;
            report.ContentReport = message;
            try
            {
                if (data[27] != "*")
                {
                    report.TimeMaxValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[27]), Convert.ToInt32(data[28]), 00);
                }
                if (data[30] != "*")
                {
                    report.TimeMinValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[30]), Convert.ToInt32(data[31]), 00);
                }
            }
            catch (Exception)
            {

            }
            report.MaxValue = CheckValueReport(data[26], obj[1]);
            report.MinValue = CheckValueReport(data[29], obj[1]);
            report.Distance1 = CheckValueReport(data[2], obj[1]);
            report.Distance2 = CheckValueReport(data[3], obj[1]);
            report.Distance3 = CheckValueReport(data[4], obj[1]);
            report.Distance4 = CheckValueReport(data[5], obj[1]);
            report.Distance5 = CheckValueReport(data[6], obj[1]);
            report.Distance6 = CheckValueReport(data[7], obj[1]);
            report.Distance7 = CheckValueReport(data[8], obj[1]);
            report.Distance8 = CheckValueReport(data[9], obj[1]);
            report.Distance9 = CheckValueReport(data[10], obj[1]);
            report.Distance10 = CheckValueReport(data[11], obj[1]);
            report.Distance11 = CheckValueReport(data[12], obj[1]);
            report.Distance12 = CheckValueReport(data[13], obj[1]);
            report.Distance13 = CheckValueReport(data[14], obj[1]);
            report.Distance14 = CheckValueReport(data[15], obj[1]);
            report.Distance15 = CheckValueReport(data[16], obj[1]);
            report.Distance16 = CheckValueReport(data[17], obj[1]);
            report.Distance17 = CheckValueReport(data[18], obj[1]);
            report.Distance18 = CheckValueReport(data[19], obj[1]);
            report.Distance19 = CheckValueReport(data[20], obj[1]);
            report.Distance20 = CheckValueReport(data[21], obj[1]);
            report.Distance21 = CheckValueReport(data[22], obj[1]);
            report.Distance22 = CheckValueReport(data[23], obj[1]);
            report.Distance23 = CheckValueReport(data[24], obj[1]);
            report.Distance24 = CheckValueReport(data[25], obj[1]);
            return _reportDailyMucNuocData.Insert(report);
        }
        /// <summary>
        /// Chèn dữ liệu vào bảng tốc độ gió
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obj"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeCode"></param>
        /// <param name="dateReport"></param>
        /// <param name="dateRequest"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool InsertDailyTocGio(string[] data, string[] obj, int deviceId, string typeCode, DateTime dateReport, DateTime dateRequest, string message)
        {
            ReportDailyTocDoGio report = new ReportDailyTocDoGio();
            report.DeviceId = deviceId;
            report.ReportTypeCode = typeCode;
            report.DateReport = dateReport;
            report.DateRequestReport = dateRequest;
            report.ContentReport = message;
            try
            {
                if (data[27] != "*")
                {
                    report.TimeMaxValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[28]), Convert.ToInt32(data[29]), 00);
                }
                if (data[30] != "*")
                {
                    report.TimeMinValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[32]), Convert.ToInt32(data[33]), 00);
                }
            }
            catch (Exception)
            {

            }
            report.TocDoGioLonNhat = CheckValueReport(data[26], obj[1]);
            report.HuongGioCuaTocDoLonNhat = CheckValueReport(data[27], obj[1]);
            report.TocDoGioNhoNhat = CheckValueReport(data[30], obj[1]);
            report.HuongGioCuarTocDoNhoNhat = CheckValueReport(data[31], obj[1]);
            report.Distance1 = CheckValueReport(data[2], obj[1]);
            report.Distance2 = CheckValueReport(data[3], obj[1]);
            report.Distance3 = CheckValueReport(data[4], obj[1]);
            report.Distance4 = CheckValueReport(data[5], obj[1]);
            report.Distance5 = CheckValueReport(data[6], obj[1]);
            report.Distance6 = CheckValueReport(data[7], obj[1]);
            report.Distance7 = CheckValueReport(data[8], obj[1]);
            report.Distance8 = CheckValueReport(data[9], obj[1]);
            report.Distance9 = CheckValueReport(data[10], obj[1]);
            report.Distance10 = CheckValueReport(data[11], obj[1]);
            report.Distance11 = CheckValueReport(data[12], obj[1]);
            report.Distance12 = CheckValueReport(data[13], obj[1]);
            report.Distance13 = CheckValueReport(data[14], obj[1]);
            report.Distance14 = CheckValueReport(data[15], obj[1]);
            report.Distance15 = CheckValueReport(data[16], obj[1]);
            report.Distance16 = CheckValueReport(data[17], obj[1]);
            report.Distance17 = CheckValueReport(data[18], obj[1]);
            report.Distance18 = CheckValueReport(data[19], obj[1]);
            report.Distance19 = CheckValueReport(data[20], obj[1]);
            report.Distance20 = CheckValueReport(data[21], obj[1]);
            report.Distance21 = CheckValueReport(data[22], obj[1]);
            report.Distance22 = CheckValueReport(data[23], obj[1]);
            report.Distance23 = CheckValueReport(data[24], obj[1]);
            report.Distance24 = CheckValueReport(data[25], obj[1]);
            return _reportDailyTocDoGioData.Insert(report);
        }
        /// <summary>
        /// Chèn dữ liệu vào bảng tốc độ dòng chảy
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obj"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeCode"></param>
        /// <param name="dateReport"></param>
        /// <param name="dateRequest"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <summary>
        /// Chèn dữ liệu vào bảng Áp suất
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obj"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeCode"></param>
        /// <param name="dateReport"></param>
        /// <param name="dateRequest"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool InsertDailyTocDoDongChay(string[] data, string[] obj, int deviceId, string typeCode, DateTime dateReport, DateTime dateRequest, string message)
        {
            ReportDailyTocDoDongChay report = new ReportDailyTocDoDongChay();
            report.DeviceId = deviceId;
            report.ReportTypeCode = typeCode;
            report.DateReport = dateReport;
            report.DateRequestReport = dateRequest;
            report.ContentReport = message;
            try
            {
                if (data[27] != "*")
                {
                    report.TimeMaxValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[27]), Convert.ToInt32(data[28]), 00);
                }
                if (data[30] != "*")
                {
                    report.TimeMinValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[30]), Convert.ToInt32(data[31]), 00);
                }
            }
            catch (Exception)
            {

            }
            report.MaxValue = CheckValueReport(data[26], obj[1]);
            report.MinValue = CheckValueReport(data[29], obj[1]);
            report.Distance1 = CheckValueReport(data[2], obj[1]);
            report.Distance2 = CheckValueReport(data[3], obj[1]);
            report.Distance3 = CheckValueReport(data[4], obj[1]);
            report.Distance4 = CheckValueReport(data[5], obj[1]);
            report.Distance5 = CheckValueReport(data[6], obj[1]);
            report.Distance6 = CheckValueReport(data[7], obj[1]);
            report.Distance7 = CheckValueReport(data[8], obj[1]);
            report.Distance8 = CheckValueReport(data[9], obj[1]);
            report.Distance9 = CheckValueReport(data[10], obj[1]);
            report.Distance10 = CheckValueReport(data[11], obj[1]);
            report.Distance11 = CheckValueReport(data[12], obj[1]);
            report.Distance12 = CheckValueReport(data[13], obj[1]);
            report.Distance13 = CheckValueReport(data[14], obj[1]);
            report.Distance14 = CheckValueReport(data[15], obj[1]);
            report.Distance15 = CheckValueReport(data[16], obj[1]);
            report.Distance16 = CheckValueReport(data[17], obj[1]);
            report.Distance17 = CheckValueReport(data[18], obj[1]);
            report.Distance18 = CheckValueReport(data[19], obj[1]);
            report.Distance19 = CheckValueReport(data[20], obj[1]);
            report.Distance20 = CheckValueReport(data[21], obj[1]);
            report.Distance21 = CheckValueReport(data[22], obj[1]);
            report.Distance22 = CheckValueReport(data[23], obj[1]);
            report.Distance23 = CheckValueReport(data[24], obj[1]);
            report.Distance24 = CheckValueReport(data[25], obj[1]);
            return _reportDailyTocDoDongChayData.Insert(report);
        }
        /// <summary>
        /// Chèn dữ liệu vào bảng tốc lưu lượng dòng chảy
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obj"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeCode"></param>
        /// <param name="dateReport"></param>
        /// <param name="dateRequest"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <summary>        
        private bool InsertDailyLuuLuongDongChay(string[] data, string[] obj, int deviceId, string typeCode, DateTime dateReport, DateTime dateRequest, string message)
        {
            ReportDailyLuuLuongDongChay report = new ReportDailyLuuLuongDongChay();
            report.DeviceId = deviceId;
            report.ReportTypeCode = typeCode;
            report.DateReport = dateReport;
            report.DateRequestReport = dateRequest;
            report.ContentReport = message;
            try
            {
                if (data[27] != "*")
                {
                    report.TimeMaxValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[27]), Convert.ToInt32(data[28]), 00);
                }
                if (data[30] != "*")
                {
                    report.TimeMinValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[30]), Convert.ToInt32(data[31]), 00);
                }
            }
            catch (Exception)
            {

            }
            report.MaxValue = CheckValueReport(data[26], obj[1]);
            report.MinValue = CheckValueReport(data[29], obj[1]);
            report.Distance1 = CheckValueReport(data[2], obj[1]);
            report.Distance2 = CheckValueReport(data[3], obj[1]);
            report.Distance3 = CheckValueReport(data[4], obj[1]);
            report.Distance4 = CheckValueReport(data[5], obj[1]);
            report.Distance5 = CheckValueReport(data[6], obj[1]);
            report.Distance6 = CheckValueReport(data[7], obj[1]);
            report.Distance7 = CheckValueReport(data[8], obj[1]);
            report.Distance8 = CheckValueReport(data[9], obj[1]);
            report.Distance9 = CheckValueReport(data[10], obj[1]);
            report.Distance10 = CheckValueReport(data[11], obj[1]);
            report.Distance11 = CheckValueReport(data[12], obj[1]);
            report.Distance12 = CheckValueReport(data[13], obj[1]);
            report.Distance13 = CheckValueReport(data[14], obj[1]);
            report.Distance14 = CheckValueReport(data[15], obj[1]);
            report.Distance15 = CheckValueReport(data[16], obj[1]);
            report.Distance16 = CheckValueReport(data[17], obj[1]);
            report.Distance17 = CheckValueReport(data[18], obj[1]);
            report.Distance18 = CheckValueReport(data[19], obj[1]);
            report.Distance19 = CheckValueReport(data[20], obj[1]);
            report.Distance20 = CheckValueReport(data[21], obj[1]);
            report.Distance21 = CheckValueReport(data[22], obj[1]);
            report.Distance22 = CheckValueReport(data[23], obj[1]);
            report.Distance23 = CheckValueReport(data[24], obj[1]);
            report.Distance24 = CheckValueReport(data[25], obj[1]);
            return _reportDailyLuuLuongDongChayData.Insert(report);
        }
        /// Chèn dữ liệu vào bảng Áp suất
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obj"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeCode"></param>
        /// <param name="dateReport"></param>
        /// <param name="dateRequest"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool InsertDailyBucXaMatTroi(string[] data, string[] obj, int deviceId, string typeCode, DateTime dateReport, DateTime dateRequest, string message)
        {
            ReportDailyBucXaMatTroi report = new ReportDailyBucXaMatTroi();
            report.DeviceId = deviceId;
            report.ReportTypeCode = typeCode;
            report.DateReport = dateReport;
            report.DateRequestReport = dateRequest;
            report.ContentReport = message;
            try
            {
                if (data[27] != "*")
                {
                    report.TimeMaxValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[27]), Convert.ToInt32(data[28]), 00);
                }
                if (data[30] != "*")
                {
                    report.TimeMinValue = new DateTime(DateTime.Now.Year, Convert.ToInt32(data[1]), Convert.ToInt32(data[0]), Convert.ToInt32(data[30]), Convert.ToInt32(data[31]), 00);
                }
            }
            catch (Exception)
            {

            }
            report.MaxValue = CheckValueReport(data[26], obj[1]);
            report.MinValue = CheckValueReport(data[29], obj[1]);
            report.Distance1 = CheckValueReport(data[2], obj[1]);
            report.Distance2 = CheckValueReport(data[3], obj[1]);
            report.Distance3 = CheckValueReport(data[4], obj[1]);
            report.Distance4 = CheckValueReport(data[5], obj[1]);
            report.Distance5 = CheckValueReport(data[6], obj[1]);
            report.Distance6 = CheckValueReport(data[7], obj[1]);
            report.Distance7 = CheckValueReport(data[8], obj[1]);
            report.Distance8 = CheckValueReport(data[9], obj[1]);
            report.Distance9 = CheckValueReport(data[10], obj[1]);
            report.Distance10 = CheckValueReport(data[11], obj[1]);
            report.Distance11 = CheckValueReport(data[12], obj[1]);
            report.Distance12 = CheckValueReport(data[13], obj[1]);
            report.Distance13 = CheckValueReport(data[14], obj[1]);
            report.Distance14 = CheckValueReport(data[15], obj[1]);
            report.Distance15 = CheckValueReport(data[16], obj[1]);
            report.Distance16 = CheckValueReport(data[17], obj[1]);
            report.Distance17 = CheckValueReport(data[18], obj[1]);
            report.Distance18 = CheckValueReport(data[19], obj[1]);
            report.Distance19 = CheckValueReport(data[20], obj[1]);
            report.Distance20 = CheckValueReport(data[21], obj[1]);
            report.Distance21 = CheckValueReport(data[22], obj[1]);
            report.Distance22 = CheckValueReport(data[23], obj[1]);
            report.Distance23 = CheckValueReport(data[24], obj[1]);
            report.Distance24 = CheckValueReport(data[25], obj[1]);
            return _reportDailyBucXaMatTroiData.Insert(report);
        }
        #endregion    
    }
}
