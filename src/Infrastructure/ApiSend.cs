using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Core.Model;

namespace Core.Helper
{
    public static class ApiSend
    {
        public static ApiResultModel PostDataObject(string apiKey, string urlPost, object input)
        {
            ApiResultModel result = new ApiResultModel();
            try
            {
                using (var client = new HttpClient())
                {                  
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("x-api-key", apiKey);                    
                    var myContent = JsonConvert.SerializeObject(input);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage responseMessage = client.PostAsync(urlPost, byteContent).Result;
                    result.Code = responseMessage.StatusCode.ToString();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var resText = responseMessage.Content.ReadAsStringAsync().Result;
                        result.Message = resText;
                        return result;
                    }
                    else
                    {
                        result.Message = "";
                    }                    
                    return result;
                }
            }
            catch (Exception)
            {
                result.Code = "500";
                return result;
            }
        }
    }
}
