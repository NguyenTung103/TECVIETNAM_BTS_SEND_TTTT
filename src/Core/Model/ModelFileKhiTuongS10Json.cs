using System;
using System.Collections.Generic;
using System.Text;

namespace BtsGetwayService.Model
{
    public class ModelFileKhiTuongS10Json
    {
        /// <summary>
        /// ID Trạm
        /// </summary>
        public string StationNo { get; set; }
        /// <summary>
        /// Ngày gửi
        /// </summary>
        public double? Datadate { get; set; }
        /// <summary>
        /// Nhiet do hien tai
        /// </summary>
        public double? T2m { get; set; }
        /// <summary>
        /// Nhiet do lon nhat
        /// </summary>
        public double? T2mmax { get; set; }
        /// <summary>
        /// Nhiet do nho nhat
        /// </summary>
        public double? T2mmin { get; set; }
        /// <summary>
        /// Do am hien tai
        /// </summary>
        public double? Rh2m { get; set; }
        /// <summary>
        /// Do am lon nhat
        /// </summary>
        public double? R2mmax { get; set; }
        /// <summary>
        /// Do am nho nhat
        /// </summary>
        public double? R2mmin { get; set; }
        /// <summary>
        /// Ap suat
        /// </summary>
        public double? PS { get; set; }
        /// <summary>
        /// Toc do gio lon nhat trong 2p (chu ky tinh 10p)
        /// </summary>
        public double? FF10m { get; set; }
        /// <summary>
        /// Huong gio cua toc do gio lon nhat trong 2p 
        /// </summary>
        public double? DD10m { get; set; }
        /// <summary>
        /// Toc do gio lon nhat trong 2s (chu ky tinh 10p)
        /// </summary>
        public double? FxFx { get; set; }
        /// <summary>
        /// Huong gio cua toc do gio lon nhat trong 2s 
        /// </summary>
        public double? DxDx { get; set; }
        /// <summary>
        /// Tgian xay ra toc do gio lon nhat trong 2s
        /// </summary>
        public double? DtdateFxDx { get; set; }
        /// <summary>
        /// Luong mua trong 10p
        /// </summary>
        public double? Rain10m { get; set; }
        /// <summary>
        /// Luong mua trong 1h
        /// </summary>
        public double? Rain1h { get; set; }
        /// <summary>
        /// Luong mua tu 19h den 19h
        /// </summary>
        public double? Rain19h { get; set; }
        /// <summary>
        /// Luong mua tu 00h den 00h
        /// </summary>
        public double? Rain00h { get; set; }
        /// <summary>
        /// Nguon cung cap 12V; dien ap 12V
        /// </summary>
        public double? Battery { get; set; }
    }
}
