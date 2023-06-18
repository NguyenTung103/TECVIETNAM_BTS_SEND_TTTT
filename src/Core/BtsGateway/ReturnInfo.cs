using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web;

namespace bts.udpgateway.integration
{
    public class ReturnInfo
    {
        public ReturnCode Code
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public object Data
        {
            get;
            set;
        }

        public ReturnInfo(ReturnCode code = ReturnCode.Unknown, string msg = null, object data = null)
        {
            if (data is ReturnCode)
            {
                Code = (ReturnCode)data;
                Message = Code.ToString();
                Data = null;
            }
            else if (data is Exception)
            {
                Exception x = (Exception)data;
                Code = ReturnCode.Exception;
                Message = x.Message;
                Data = x.InnerException;
            }
            else if (data != null && code == ReturnCode.Unknown && string.IsNullOrEmpty(msg))
            {
                Code = ReturnCode.Success;
                Message = Code.ToString();
                Data = data;
            }
            else
            {
                Code = code;
                Message = msg ?? code.ToString();
                Data = data;
            }
        }
    }

    public enum ReturnCode : int
    {
        Unknown = -1,
        Success = 0,
        Error = 1,
        Exception = 2
    }
}