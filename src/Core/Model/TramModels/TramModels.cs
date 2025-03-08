using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.TramModels
{
    public class SiteModel
    {
        public string station_code { get; set; }
        public string station_name { get; set; }
        public string province_code { get; set; }
        public string province_name { get; set; }
        public string district_code { get; set; }
        public string latitude { get; set; }
        public string longtitude { get; set; }
        public string altitude { get; set; }
        public object licenses { get; set; }
        public string step_data { get; set; }
        public string status { get; set; }
        public string alert_status { get; set; }
        public string forecast_status { get; set; }
        public string disease_status { get; set; }
        public string current_date { get; set; }
        public string msisdn { get; set; }
        public string field_user_name { get; set; }
        public string station_type { get; set; }
        public string forecast_time { get; set; }
        public int? station_port { get; set; }
        public string address { get; set; }
        public object river_basin { get; set; }
        public string obs_type { get; set; }
    }    
}
