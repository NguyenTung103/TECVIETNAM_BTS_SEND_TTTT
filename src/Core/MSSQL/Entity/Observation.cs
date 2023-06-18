using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    [Table("Observation")]
    public class Observation
    {
        public int Id { get; set; }
        public Nullable<bool> Noti_Alarm { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> CreateDay { get; set; }
        public Nullable<System.DateTime> UpdateDay { get; set; }
        public string Low_Value { get; set; }
        public string Hight_Value { get; set; }
        public Nullable<bool> IsBieuDo { get; set; }
    }
}
