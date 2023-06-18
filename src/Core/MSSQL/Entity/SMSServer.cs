using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    [Table("SmsServer")]
    public class SMSServer
    {
        public int Id { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public string AddressIP { get; set; }       
    }
}
