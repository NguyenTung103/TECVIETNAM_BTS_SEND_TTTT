using ApiWatec.Models;
using BtsGetwayService.MongoDb.Entity;
using Core.Constant;
using Core.Model;
using Core.Model.ReponseModel;
using Core.Model.Report;
using Core.Model.Report.ReportDay;
using Core.MSSQL.Responsitory.Interface;
using Dapper;
using Infrastructure.Udp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWatec.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        public readonly IReportS10Service _reportS10Service;
        public readonly IObservationService _observationService;
        private readonly ILogger<DoMuaNdController> _logger;

        public ReportController(ILogger<DoMuaNdController> logger
            , IReportS10Service reportS10Service
            , IObservationService observationService
         )
        {
            _logger = logger;
            _reportS10Service = reportS10Service;
            _observationService = observationService;
        }        
        [HttpPost("data")]
        [Authorize]
        public async Task<ActionResult<ReponseData>> GetReportByTime([FromBody] ReportDayRequestModel model)
        {
            ReponseData reponse = new ReponseData();
            reponse.status = ObservationConstant.THANH_CONG;
            try
            {
                var (type, typeTime) = ObservationConstant.PARAMS_API_REPORT[model.type];
                string format = "dd-MM-yyyy", query = "";
                DateTime fromDate = DateTime.Today;
                DateTime toDate = DateTime.Now;                
                if (!string.IsNullOrEmpty(model.fromDate))
                {
                    fromDate = DateTime.ParseExact(model.fromDate, format, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(model.toDate))
                {
                    toDate = DateTime.ParseExact(model.toDate, format, CultureInfo.InvariantCulture);
                }
                if(type==1)
                {
                    reponse.result = await _reportS10Service.GetReportSumByTime(fromDate, toDate, model.sensorTarget, model.stationCodes, typeTime);
                }
                else
                {
                    reponse.result = await _reportS10Service.GetReportByDay(fromDate, toDate, model.sensorTarget, model.stationCodes, typeTime);
                }               

            }
            catch (Exception ex)
            {
                reponse.status = ObservationConstant.THAT_BAI;
                reponse.Message = ex.Message;
            }
            return reponse;
        }
        [HttpPost("GetListSensorByStation")]
        [Authorize]
        public async Task<ActionResult<ReponseData>> GetListSensorByStation([FromBody] ObservationRequestModel model)
        {
            ReponseData reponse = new ReponseData();
            reponse.status = ObservationConstant.THANH_CONG;
            try
            {
                reponse.result = await _observationService.GetAllObservation(model);
            }
            catch (Exception ex)
            {
                reponse.status = ObservationConstant.THAT_BAI;
                reponse.Message = ex.Message;
            }
            return reponse;
        }
        [HttpPost("GetListSensorByStations")]
        [Authorize]
        public async Task<ActionResult<ReponseData>> GetListSensorByStations([FromBody] DsObservationRequestModel model)
        {
            ReponseData reponse = new ReponseData();
            reponse.status = ObservationConstant.THANH_CONG;
            try
            {
                reponse.result = await _observationService.GetDsObservation(model);
            }
            catch (Exception ex)
            {
                reponse.status = ObservationConstant.THAT_BAI;
                reponse.Message = ex.Message;
            }
            return reponse;
        }
    }
}