using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public class ModelFileGioS10Json
    {
        public string StationNo { get; set; }
        public double? Datadate { get; set; }
        public double? FF10m { get; set; }
        public double? DD10m { get; set; }
        public double? FxFx { get; set; }
        public double? DxDx { get; set; }
        public double? DtdateFxDx { get; set; }
        public double? Battery { get; set; }
    }

}
