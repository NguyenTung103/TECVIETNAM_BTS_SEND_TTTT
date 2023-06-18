using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    [Table("RegisterSMS")]
    public class RegisterSMS
    {
        public int Id { get; set; }
        public string DanhSachSDT { get; set; }
        public string CodeObservation { get; set; }
        public string SMSServerId { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateSend { get; set; }
        public int DeviceId { get; set; }
        public int GroupId { get; set; }
        public Nullable<bool> IsAll { get; set; }      
    }    
}
