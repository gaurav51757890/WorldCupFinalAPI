using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model;

namespace WebServiceAutomation.Helper.Request
{
    public class HttpClientHelper
    {
        private static HttpClient httpClient;
        private static HttpRequestMessage httpRequestMessage;
        private static RestResponse restResponse;

        private static HttpClient AddHeadersAndCreateHttpClient(Dictionary<string, string> httpHeaders)
        {
            HttpClient httpClient = new HttpClient();
            if(httpHeaders != null)
            {
                foreach (string key in httpHeaders.Keys)
                {
                    httpClient.DefaultRequestHeaders.Add(key, httpHeaders[key]);
                }
            }
          
            return httpClient;
        }
        private static HttpRequestMessage CreateRequestMessage(string requestUrl, HttpMethod httpMethod, HttpContent httpContent)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, requestUrl);
            if (!(httpMethod == HttpMethod.Get))
            {
                httpRequestMessage.Content = httpContent;
            }
            if (!(httpMethod == HttpMethod.Delete))
            {
                httpRequestMessage.Content = httpContent;
            }

            return httpRequestMessage;
        }

        private static RestResponse SendRequest(string requestUrl, HttpMethod httpMethod, HttpContent httpContent, Dictionary<string, string> httpHeaders)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpHeaders);
            httpRequestMessage = CreateRequestMessage(requestUrl, httpMethod, httpContent);
            try
            {
                Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception err)
            {
                restResponse = new RestResponse(500, err.Message);
            }
            finally
            {
                httpRequestMessage?.Dispose();
                httpClient?.Dispose();
            }

            return restResponse;
        }

        public static RestResponse PeformGetRequest (string requestUrl, Dictionary<string, string> httpHeaders)
        {
          return SendRequest(requestUrl, HttpMethod.Get, null, httpHeaders);
        }

        public static RestResponse PerformPostRequest(string requestUrl, HttpContent httpContent, Dictionary<string, string> httpHeaders)
        {
            return SendRequest(requestUrl, HttpMethod.Post, httpContent, httpHeaders);
        }

        public static RestResponse PerformPostRequest(string requestUrl, string data, string mediaType,  Dictionary<string, string> httpHeaders)
        {
            HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediaType);
            return PerformPostRequest(requestUrl, httpContent, httpHeaders);
        }

        public static RestResponse PeformPutRequest(string requestUrl, string content, string mediaType, Dictionary<string, string> httpHeaders)
        {
           HttpContent httpContent = new StringContent(content, Encoding.UTF8, mediaType);
           return SendRequest(requestUrl, HttpMethod.Put, httpContent, httpHeaders);
        }

        public static RestResponse PeformPutRequest(string requestUrl,HttpContent httpContent, Dictionary<string, string> httpHeaders)
        {
            return SendRequest(requestUrl, HttpMethod.Put, httpContent, httpHeaders);
        }

        public static RestResponse PerformDeleteRequest(string requestUrl)
        {
          return  SendRequest(requestUrl, HttpMethod.Delete, null, null);
        }
    }
}
