using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public class ModelFileThuyVanS10Json
    {
        /// <summary>
        /// ID Trạm
        /// </summary>
        public string StationNo { get; set; }
        /// <summary>
        /// Ngày gửi
        /// </summary>
        public double Datadate { get; set; }
        /// <summary>
        /// Mực nước
        /// </summary>
        public float WL { get; set; }
    }
}
