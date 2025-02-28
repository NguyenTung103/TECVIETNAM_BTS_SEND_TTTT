using bts.udpgateway.integration;
using Infrastructure.Udp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Test;

namespace UnitTest
{
    public class UnitTest1 : IClassFixture<BaseTest>
    {
        private readonly IHost _host;
        private readonly IUdpService _udpService;
        public UnitTest1(BaseTest baseTest)
        {
            _host = baseTest.TestHost;
            _udpService = _host.Services.GetService<IUdpService>();
        }
        [Fact]
        public async Task Test1()
        {
            var subMessage = "EQID=01020405201;S10;11:25:00-14/02/25;MRA0001.1;MRT0012.0;MRD0000.6;MRH0000.6;MRE0000.6;MRF0000.6;MRG0000.6;MRB0002.5;MRS0002.5;MVC12.1;MGS081899;END;";
            ReturnInfo returnInfo =await _udpService.Insert(subMessage);
        }
    }
}