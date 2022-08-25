using System;
using System.Collections.Generic;
using System.Text;

namespace BtsGetwayService.MSSQL.Entity
{
    public class Site
    {
        public int Id { get; set; }

        public int Area_Id { get; set; }

        public int Group_Id { get; set; }
        public int TypeSiteId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }       

        public bool IsActive { get; set; }

        public Nullable<int> DeviceId { get; set; }
    }
}
