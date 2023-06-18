namespace GatewayApi.Models
{
    public class MessageModel
    {
        public string Message { get; set; }
    }
    public class ResponeData
    {      
        public int Status { get; set; }
        public string Message { get; set; }
        public int Total { get; set; }
        public dynamic Data { get; set; }
    }
    public static class StatusResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public static int NOTFOUND = 404;
        /// <summary>
        /// 
        /// </summary>
        public static int SUCCESS = 200;
        /// <summary>
        /// 
        /// </summary>
        public static int ERROR = 500;
        /// <summary>
        /// 
        /// </summary>
        public static int ERROR_UPDATE_DB = 501;
        /// <summary>
        /// 
        /// </summary>
        public static int UNAUTHORIZE = 401;
        /// <summary>
        /// 
        /// </summary>
        public static int ERROR_INPUT_DATA = 402;
        /// <summary>
        /// 
        /// </summary>
        public static int FORBIDDEN_CHUA_CAP_PHEP_TRUY_CAP = 403;
        /// <summary>
        /// 
        /// </summary>
        public static int WRRONG_LOGGIN = 406;
    }
}
