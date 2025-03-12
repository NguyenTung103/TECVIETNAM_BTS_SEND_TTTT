using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    [Table("Site")]
    public partial class Site
    {
        public int Id { get; set; }

        public int Area_Id { get; set; }

        public int Group_Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Latitude { get; set; }

        public string Longtitude { get; set; }

        public string TimeZone { get; set; }
        public string Code_Group { get; set; }

        public System.DateTime CreateDay { get; set; }

        public Nullable<System.DateTime> UpdateDay { get; set; }

        public Nullable<int> CreateBy { get; set; }

        public Nullable<int> UpdateBy { get; set; }

        public bool IsActive { get; set; }

        public Nullable<int> DeviceId { get; set; }
        public Nullable<int> TypeSiteId { get; set; }
    }
    public partial class Site
    {        
        public string Code { get; set; }
    }
}
