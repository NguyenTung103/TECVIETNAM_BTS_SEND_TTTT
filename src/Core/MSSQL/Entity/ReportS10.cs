using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bts.udpgateway
{
    [Table("ReportS10")]
    public partial class ReportS10
    {
        public int ID { get; set; }
        public Nullable<int> DeviceId { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<double> MTI { get; set; }
        public Nullable<double> MTX { get; set; }
        public Nullable<double> MTM { get; set; }
        public Nullable<double> MHU { get; set; }
        public Nullable<double> MHX { get; set; }
        public Nullable<double> MHM { get; set; }
        public Nullable<double> MAV { get; set; }
        public Nullable<double> MSM { get; set; }
        public Nullable<double> MDM { get; set; }
        public Nullable<double> MSS { get; set; }
        public Nullable<double> MDS { get; set; }
        public Nullable<double> MHR { get; set; }
        public Nullable<double> MRT { get; set; }
        public Nullable<double> MRH { get; set; }
        public Nullable<double> MRC { get; set; }
        public Nullable<double> MRB { get; set; }
        public Nullable<double> MVC { get; set; }
        public Nullable<double> MFX { get; set; }
        public Nullable<double> MFD { get; set; }
        public Nullable<double> MHD { get; set; }
        public Nullable<double> MFL { get; set; }
        public Nullable<double> MFF { get; set; }
        public Nullable<double> MFV { get; set; }
        // Hải văn
        public Nullable<double> MTS { get; set; }
        public Nullable<double> MEC { get; set; }
        public Nullable<double> MEV { get; set; }
        // Thủy điện hòa bình
        public Nullable<double> MRA { get; set; }
        public Nullable<double> MRD { get; set; }
        public Nullable<double> MRE { get; set; }
        public Nullable<double> MRF { get; set; }
        public Nullable<double> MRG { get; set; }
        public Nullable<double> MRS { get; set; }
    }
}
