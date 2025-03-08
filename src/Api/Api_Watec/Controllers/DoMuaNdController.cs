using ApiWatec.Models;
using BtsGetwayService.MongoDb.Entity;
using Core.MSSQL.Responsitory.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace ApiWatec.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoMuaNdController : ControllerBase
    {
        public readonly IReportS10Data _reportS10Data;
        public readonly ISiteData _siteData;       
        private readonly ILogger<DoMuaNdController> _logger;

        public DoMuaNdController(ILogger<DoMuaNdController> logger
            , IReportS10Data reportS10Data            
            , ISiteData siteData
         )
        {
            _logger = logger;
            _reportS10Data = reportS10Data;            
            _siteData = siteData;
        }
        [HttpPost("rain")]
        public ActionResult<string> Rain(DoMuaModel doMua)
        {
            DoMuaReponse reponse = new DoMuaReponse();
            string result = "{ \"data\": {";
            reponse.rid = doMua.rid;
            DateTime from, to;
            if (doMua != null && doMua.from != null && doMua.to != null)
            {
                from = DateTime.ParseExact(doMua.from, "yyyy-MM-dd HH:mm", null);
                to = DateTime.ParseExact(doMua.to, "yyyy-MM-dd HH:mm", null);
                if (doMua.sids != null)
                {
                    int indexDevice = 1;
                    foreach (var deviceIdStr in doMua.sids)
                    {
                        if (indexDevice == doMua.sids.Count())
                        {
                            int deviceId = Int32.Parse(deviceIdStr);
                            var dsDataByTimeAndDevice = _reportS10Data.GetByTime(from, to, deviceId).Skip(1);
                            if (dsDataByTimeAndDevice != null && dsDataByTimeAndDevice.Count() > 0)
                            {
                                result += "\""+ deviceId.ToString("D10") + "\": [";
                                int indexDataByDevice = 1;
                                foreach (var item in dsDataByTimeAndDevice)
                                {
                                    if (indexDataByDevice == dsDataByTimeAndDevice.Count())
                                        result += item.MRT;
                                    else
                                        result += item.MRT + ",";
                                    indexDataByDevice++;
                                }
                                result += "]";
                            }
                        }
                        else
                        {
                            int deviceId = Int32.Parse(deviceIdStr);
                            var dsDataByTimeAndDevice = _reportS10Data.GetByTime(from, to, deviceId).Skip(1);
                            if (dsDataByTimeAndDevice != null && dsDataByTimeAndDevice.Count() > 0)
                            {
                                result += "\"" + deviceIdStr + "\": [";
                                int indexDataByDevice = 1;
                                foreach (var item in dsDataByTimeAndDevice)
                                {
                                    if (indexDataByDevice == dsDataByTimeAndDevice.Count())
                                        result += item.MRT;
                                    else
                                        result += item.MRT + ",";
                                    indexDataByDevice++;
                                }
                                result += "],";
                            }
                        }
                        indexDevice++;
                    }
                }
                result += "}, \"rid\": \"" + doMua.rid+ "\"}";
            }            
            return result;
        }
        [HttpGet("test")]
        [Authorize]
        public ActionResult<string> test()
        {           
            return Ok("test");
        }
    }
}