using ApiWatec.Models;
using BtsGetwayService.MongoDb.Entity;
using Core.Constant;
using Core.Model;
using Core.Model.ReponseModel;
using Core.Model.Report;
using Core.Model.Report.ReportDay;
using Core.MSSQL.Responsitory.Interface;
using Infrastructure.Udp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        [HttpPost("GetReportByDay")]
        //[Authorize]
        public async Task<ActionResult<ReponseData>> GetReportByDay([FromBody]ReportDayRequestModel model)
        {
            ReponseData reponse = new ReponseData();
            reponse.status = ObservationConstant.THANH_CONG;
            try
            {
                reponse.result = await _reportS10Service.GetReportByDay(model);
            }
            catch (Exception ex)
            {
                reponse.status = ObservationConstant.THAT_BAI;
                reponse.Message = ex.Message;
            }
            return reponse;
        }
        [HttpPost("GetListSensor")]
        //[Authorize]
        public async Task<ActionResult<ReponseData>> GetListSensor([FromBody] ObservationRequestModel model)
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
    }
}