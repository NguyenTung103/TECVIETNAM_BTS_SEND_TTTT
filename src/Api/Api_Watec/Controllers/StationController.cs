using ApiWatec.Models;
using BtsGetwayService.MongoDb.Entity;
using Core.Constant;
using Core.Model.ReponseModel;
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
    public class StationController : ControllerBase
    {
        public readonly ISiteService _siteService;
        private readonly ILogger<DoMuaNdController> _logger;

        public StationController(ILogger<DoMuaNdController> logger
            , ISiteService siteService
         )
        {
            _logger = logger;
            _siteService = siteService;
        }
        [HttpGet("GetListStation")]
        [Authorize]
        public async Task<ActionResult<ReponseData>> GetListStation([FromQuery] List<string> decviceId = null)
        {
            ReponseData reponse = new ReponseData();
            reponse.status = ObservationConstant.THANH_CONG;
            try
            {
                reponse.result = await _siteService.GetDanhSachTram(decviceId);
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