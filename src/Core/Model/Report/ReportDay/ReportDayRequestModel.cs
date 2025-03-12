using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.Report.ReportDay
{
    public class ReportDayRequestModel
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public int type { get; set; }
        public string sensorTarget { get; set; }
        public string stationCodes { get; set; }
    }
}
