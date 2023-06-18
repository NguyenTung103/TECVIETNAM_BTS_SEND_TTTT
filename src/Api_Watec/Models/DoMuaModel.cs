using System.Collections.Generic;

namespace ApiWatec.Models
{
    /// <summary>
    /// input
    /// </summary>
    public class DoMuaModel
    {
        /// <summary>
        /// Danh sách id trạm
        /// </summary>
        public List<string> sids { get; set; }
        /// <summary>
        /// Thời gian bắt đầu cần lấy số liệu
        /// </summary>
        public string from { get; set; }
        /// <summary>
        /// Thời gian bắt đầu cần lấy số liệu
        /// </summary>
        public string to { get; set; }
        /// <summary>
        /// Id của yêu cầu
        /// </summary>
        public string rid { get; set; }

    }
}
