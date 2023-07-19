using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Setting
{
    public class AppSetting
    {
        public string FolderLuuTruFile { get; set; }        
        public int IsSendTTTT { get; set; }        
    }    
    public class AppApiWatecSetting
    {
        public string UrlPost { get; set; }
        public string ApiKey { get; set; }
        public int IsChooseGroup { get; set; }
        public int IsSendWatec { get; set; }
        public string ApiWatecKey { get; set; }
        public string ApiWatecUrl { get; set; }
    }
    public class AppSettingUDP
    {
        public int UdpPort { get; set; }        
        public bool ToDatabase { get; set; }        
        public bool IsUseRabbitMQ { get; set; }        
    }
}
