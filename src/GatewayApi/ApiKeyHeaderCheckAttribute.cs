using GatewayApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Api.Core.Filters
{
    /// <summary>
    /// Check 
    /// </summary>
    public class ApiKeyHeaderCheckAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        public ApiKeyHeaderCheckAttribute() : base(typeof(ApiKeyHeaderCheckFilter))
        {
        }

        private class ApiKeyHeaderCheckFilter : IActionFilter
        {
            public void OnActionExecuting(ActionExecutingContext context)
            {
                var requestKey = context.HttpContext.Request.Headers["ApiKey"];
                if (string.IsNullOrEmpty(requestKey))
                {
                    context.Result = new JsonResult(new
                    {
                        status = StatusResponse.UNAUTHORIZE,
                        message = "Lỗi token không được để trống",
                        error_code = "Authorized"
                    });                    
                    return;
                }
                else
                {
                    if (requestKey.Count <= 0)
                    {
                        context.Result = new JsonResult(new
                        {
                            status = StatusResponse.UNAUTHORIZE,
                            message = "Lỗi nhập sai mã token",
                            error_code = "Authorized"
                        });
                        return;
                    }

                    if ("3EC79C17-63ED-4166-BD58-04397B94312C" != requestKey && "quangich123" != requestKey)
                    {
                        context.Result = new JsonResult(new
                        {
                            status = StatusResponse.UNAUTHORIZE,
                            message = "Lỗi nhập sai mã token",
                            error_code = "Authorized"
                        });
                        return;
                    }
                }

            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }
}
