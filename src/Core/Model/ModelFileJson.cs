using System;
using System.Collections.Generic;
using System.Text;

namespace BtsGetwayService.Model
{

    public class ModelFileTramKhiTuongJson
    {
        /// <summary>
        /// ID Trạm
        /// </summary>
        public string StationNo { get; set; }
        /// <summary>
        /// Ngày gửi
        /// </summary>
        public long Datadate { get; set; }
        /// <summary>
        /// Hướng gió (trung bình 2 phút)
        /// </summary>
        public float DD10m { get; set; }
        /// <summary>
        /// Tốc độ gió (trung bình 2 phút)
        /// </summary>
        public float FF10m { get; set; }
        /// <summary>
        /// Hướng gió giật
        /// </summary>
        public float DxDx { get; set; }
        /// <summary>
        /// Tốc độ gió giật
        /// </summary>
        public float FxFx { get; set; }       
        /// <summary>
        /// Nhiệt độ không khí
        /// </summary>
        public float T2m { get; set; }        
        /// <summary>
        /// Độ ẩm tương đối
        /// </summary>
        public float Rh2m { get; set; }
        /// <summary>
        /// Khí áp mực trạm
        /// </summary>
        public float PS { get; set; }
        /// <summary>
        /// Lượng mưa 24h
        /// </summary>
        public float Rain24h { get; set; }   
        /// <summary>
        /// Tốc độ gió BWS
        /// </summary>
        public float FTFT { get; set; }  
        /// <summary>
        /// Hướng gió BAP
        /// </summary>
        public float DTDT { get; set; }        
    }
    public class ModelFileTramThuyVanJson
    {
        /// <summary>
        /// ID Trạm
        /// </summary>
        public string StationNo { get; set; }
        /// <summary>
        /// Ngày gửi
        /// </summary>
        public long Datadate { get; set; }      
        /// <summary>
        /// Mực nước
        /// </summary>
        public float WL { get; set; }           
    }
}
