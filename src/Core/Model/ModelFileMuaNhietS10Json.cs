using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{

    public class ModelFileMuaNhietS10Json
    {
        public string StationNo { get; set; }
        public double? Datadate { get; set; }
        public double? T2m { get; set; }
        public double? T2mmax { get; set; }
        public double? T2mmin { get; set; }
        public double? Rh2m { get; set; }
        public double? R2mmax { get; set; }
        public double? R2mmin { get; set; }
        public double? Rain10m { get; set; }
        public double? Rain1h { get; set; }
        public double? Rain19h { get; set; }
        public double? Rain00h { get; set; }
        public double? Battery { get; set; }
    }

}
