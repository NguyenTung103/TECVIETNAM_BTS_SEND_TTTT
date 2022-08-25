using System;
using System.Collections.Generic;
using System.Text;

namespace BtsGetwayService.MSSQL.Entity
{
    public class RegionalGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }

        public string Contact { get; set; }

        public Nullable<bool> Status { get; set; }

        public Nullable<System.DateTime> CreateDay { get; set; }

        public Nullable<System.DateTime> UpdateDay { get; set; }

        public Nullable<int> CreateBy { get; set; }

        public Nullable<int> UpdateBy { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }
        public string FtpIp { get; set; }
        public string FtpAccount { get; set; }
        public string FtpPassword { get; set; }
        public string FtpDirectory { get; set; }

        public bool IsActive { get; set; }
        public bool IsSendTTTT { get; set; }

    }
}
