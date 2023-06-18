using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    [Table("ReportType")]
    public partial class ReportType
    {

        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

    }
}
