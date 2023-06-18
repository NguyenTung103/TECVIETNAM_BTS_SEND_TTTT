using Api.Core.Filters;
using Core.Entities;
using Core.Interfaces;
using Core.Logging;
using Core.MSSQL.Responsitory.Interface;
using GatewayApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace GatewayApi.Controllers
{   
    [ApiController]
    [Route("[controller]")]
    [ApiKeyHeaderCheck]
    public class UdpServiceController : ControllerBase
    {
        public readonly IWorkerMessageQueueService _workerMessageQueueService;
        public readonly IMasterMessageQueueService _masterMessageQueueService;       

        public UdpServiceController(
              IWorkerMessageQueueService workerMessageQueueService
            , IMasterMessageQueueService masterMessageQueueService
         )
        {
            _workerMessageQueueService = workerMessageQueueService;
            _masterMessageQueueService = masterMessageQueueService;            
        }
        [HttpPost("PostMessage")]
        public ActionResult<ResponeData> PostMessage(MessageModel model)
        {
            ResponeData result = new ResponeData();
            try
            {
                if (!string.IsNullOrEmpty(model.Message))
                {
                    var message = new QueueMessage()
                    {
                        Id = "DevicePostEvent"
                    };                    
                    message.Datas.Add("Message", model.Message);                    
                    _workerMessageQueueService.PublishWorkerTask(message);
                    //_logger.LogInformation("Ký tài liệu lúc " + DateTime.Now.ToString() + " Trường: " + signModel.TenTruong + ", Nhân sự: " + signModel.TenNhanSu + ", Số lượng file: " + string.Join(",", signModel.FileUnsign.Count()));
                }
                result.Status = 200;
                result.Data = model.Message;
                result.Total = 1;
                result.Message = "Thành công";
                return result;
            }
            catch (Exception ex)
            {
                result.Status = 500;
                result.Data = model.Message;
                result.Total = 1;
                result.Message = "Thành công";
                return result;
            }
        }
    }
}
