﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BtsGetwayService.MSSQL.Entity
{
    [Table("ReportDailyTocDoGio")]
    public class ReportS10
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
    }
}