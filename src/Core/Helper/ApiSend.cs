using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Core.Model;
using System.Linq;
using System.Net;

namespace Core.Helper
{
    public static class ApiSend
    {
        public static async Task<ApiResultModel> PostDataObject(string apiKey, string urlPost, object input)
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
                    HttpResponseMessage responseMessage = await client.PostAsync(urlPost, byteContent);
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
        public static async Task<string> Call_PostDataAsync(object data, string url, string path, Dictionary<string, string> headers)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, $"{url}/{path}"))
                {
                    if (headers != null && headers.Any())
                    {
                        foreach (var item in headers)
                        {
                            if (!string.IsNullOrEmpty(item.Key))
                                request.Headers.Add(item.Key, $"{item.Value}");
                        }
                    }

                    if (data != null)
                    {
                        var stringContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                        {
                            request.Content = stringContent;
                        }
                    }
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    var client = new HttpClient();
                    var response = await client.SendAsync(request);

                    var responseString = await response.Content.ReadAsStringAsync();
                    return responseString;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
