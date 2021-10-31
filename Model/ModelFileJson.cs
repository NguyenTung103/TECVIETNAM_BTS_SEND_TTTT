using System;
using System.Collections.Generic;
using System.Text;

namespace BtsGetwayService.Model
{

    public class ModelFileJson
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
        /// Mực nước
        /// </summary>
        public float WL { get; set; }
        /// <summary>
        /// Nhiệt độ không khí
        /// </summary>
        public float T2m { get; set; }
        /// <summary>
        /// Nhiệt độ điểm sương
        /// </summary>
        public float Td2m { get; set; }
        /// <summary>
        /// Độ ẩm tương đối
        /// </summary>
        public float Rh2m { get; set; }
        /// <summary>
        /// Nhiệt độ đất
        /// </summary>
        public float Tg { get; set; }
        /// <summary>
        /// Nhiệt độ nước
        /// </summary>
        public float Tw { get; set; }
        /// <summary>
        /// Khí áp mực trạm
        /// </summary>
        public float PS { get; set; }
        /// <summary>
        /// Khí áp trung bình mực biển
        /// </summary>
        public float PMSL { get; set; }
        /// <summary>
        /// Lượng mưa 10 phút qua
        /// </summary>
        public float Rain10m { get; set; }
        /// <summary>
        /// Lượng mưa 1 giờ qua
        /// </summary>
        public float Rain1h { get; set; }
        /// <summary>
        /// Ngày 
        /// </summary>
        public long Year { get; set; }
    }
}
