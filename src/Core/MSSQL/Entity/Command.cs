using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    [Table("Command")]
    public class Commands
    {

        public int Id { get; set; }

        public Nullable<int> Device_Id { get; set; }

        public string Command_Content { get; set; }

        public Nullable<bool> Status { get; set; }

        public Nullable<System.DateTime> CreateDay { get; set; }

        public Nullable<int> CreateBy { get; set; }

        public Nullable<int> UpdateBy { get; set; }

        public Nullable<System.DateTime> UpdateDay { get; set; }

    }
}
