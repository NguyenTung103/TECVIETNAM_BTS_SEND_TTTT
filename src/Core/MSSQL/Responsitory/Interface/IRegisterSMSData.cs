using bts.udpgateway;
using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.MSSQL.Responsitory.Interface
{
    public interface IRegisterSMSData : IDapperBaseData<RegisterSMS>
    {
        IEnumerable<RegisterSMS> GetRegisterSMS(int deviceid);
    }
}
