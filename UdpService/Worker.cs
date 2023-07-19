using bts.udpgateway.integration;
using BtsGetwayService;
using Core.Interfaces;
using Infrastructure.Udp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Setting;
using Microsoft.Extensions.Options;
using Core.Entities;

namespace UdpService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IUdpService _udpService;        
        static private UdpClient _udpClient = null;
        static private IPEndPoint _endpoint = null;
        private AppSettingUDP _appSettingUDP;
        //public readonly IWorkerMessageQueueService _workerMessageQueueService;
        //public readonly IMasterMessageQueueService _masterMessageQueueService;
        public Worker(ILogger<Worker> logger,
            IUdpService udpService,
            //IWorkerMessageQueueService workerMessageQueueService,
            //IMasterMessageQueueService masterMessageQueueService,
            IOptions<AppSettingUDP> options
            )
        {
            _logger = logger;
            _udpService = udpService;
            _appSettingUDP = options.Value;
            //_workerMessageQueueService = workerMessageQueueService;
            //_masterMessageQueueService = masterMessageQueueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker starting at: {0}", DateTimeOffset.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            while (true)
            {
                try
                {
                    if (_udpClient == null)
                        _udpClient = new UdpClient(_appSettingUDP.UdpPort);
                    if (_endpoint == null)
                        _endpoint = new IPEndPoint(IPAddress.Any, _appSettingUDP.UdpPort);
                    // Get thong tin tra ve
                    byte[] bytes = _udpClient.Receive(ref _endpoint);
                    string package = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    var subPackge = package;
                    string fpx = null;
                    if (package.StartsWith("EQID="))
                    {
                        var indexDevice = package.IndexOf(';');
                        fpx = subPackge.Substring(5, indexDevice - 5);
                    }

                    if (_appSettingUDP.ToDatabase)
                    {
                        var strs = package.Split(new string[] { "END;" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string item in strs)
                        {
                            if (String.IsNullOrWhiteSpace(item))
                                continue;

                            string subMessage = item + "END;";
                            if(_appSettingUDP.IsUseRabbitMQ)
                            {
                                //var message = new QueueMessage()
                                //{
                                //    Id = "DevicePostEvent"
                                //};
                                //message.Datas.Add("Message", subMessage);
                                //_workerMessageQueueService.PublishWorkerTask(message);
                            }
                            else
                            {
                                ReturnInfo returnInfo = _udpService.Insert(subMessage).GetAwaiter().GetResult();
                                if (returnInfo.Code != ReturnCode.Success)
                                {
                                    // Ghi mesage nhận được                                   
                                    //IOHelper.WriteMSG(subMessage, fpx);
                                }
                            }
                           
                           
                            if (subMessage.Contains(";REQ;") && subMessage.Contains(";TIME;END;"))
                            {
                                var commandBack = string.Format("cmdC24={0:00}:{1:00}:{2:00}-{3:00}/{4:00}/{5:00}-{6:d};", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year % 100, DateTime.Now.DayOfWeek + 1);
                                byte[] dgram = Encoding.ASCII.GetBytes(commandBack);
                                _udpClient.Send(dgram, dgram.Length, _endpoint);
                            }
                            if (subMessage.StartsWith("EQID="))
                            {
                                _udpService.SendCommand(subMessage, _endpoint, _udpClient);
                            }
                        }
                    }
                    //if (ConfigHelper.ToFilesystem)
                    //{
                    //    if (package.Contains("S10"))
                    //        IOHelper.WritePKG(package, fpx);
                    //}
                }
                catch (SocketException sex)
                {
                    if (_udpClient != null)
                    {
                        if (_udpClient.Client != null)
                        {
                            _udpClient.Client.Shutdown(SocketShutdown.Both);
                            _udpClient.Client.Close();
                        }
                        _udpClient.Close();
                        _udpClient = null;
                    }
                }                
            }
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Worker stopping at: {0}", DateTimeOffset.Now.ToString("dd/MM/yyyy HH:mm"));
            return base.StopAsync(cancellationToken);
        }
    }
}
