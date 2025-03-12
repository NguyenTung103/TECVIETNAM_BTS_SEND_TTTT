using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.Report.ReportDay
{
    public class ReportDayReponseModel
    {
        public string Time { get; set; }
        public Dictionary<string, DataReportDayValue> Data { get; set; }
    }
    public class ReportSumByTimeReponseModel
    {
        public string Time { get; set; }
        public Dictionary<string, decimal> Data { get; set; }
    }
    public class DataReportDayValue
    {      
        public string Average { get; set; }
        public string Min { get; set; }
        public string Last { get; set; }
        public string Max { get; set; }
        public string Sum { get; set; }
    }
    public class DataReportDayEntity
    {
        public DateTime TimeSlot { get; set; }
        public int DeviceId { get; set; }
        public int TypeSiteId { get; set; }
        public string Average { get; set; }
        public string Code_Group { get; set; }
        public string Min { get; set; }
        public string Last { get; set; }
        public string Max { get; set; }
        public string Sum { get; set; }
    }
}
