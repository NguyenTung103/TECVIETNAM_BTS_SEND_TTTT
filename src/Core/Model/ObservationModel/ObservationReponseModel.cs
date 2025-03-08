using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.ObservationModel
{    
    public class ObservationReponseModel
    {
        public string station_code { get; set; }
        public string sensor_ch { get; set; }
        public string sensor_name { get; set; }
        public string sensor_target { get; set; }
        public string status { get; set; }
    }

}
