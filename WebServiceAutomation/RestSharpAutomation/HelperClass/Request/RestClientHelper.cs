using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAutomation.HelperClass.Request
{
    public class RestClientHelper
    {
        private IRestClient GetRestClient()
        {
            IRestClient restClient = new RestClient();
            return restClient;
        }

        private IRestRequest GetRestRequest(string url, Dictionary<string, string> headers, Method method, object body)
        {
            IRestRequest restRequest = new RestRequest(url, method);
            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    restRequest.AddHeader(key, headers[key]);
                }
            }
            if (body != null)
            {
                if (headers.ContainsKey("Accept"))
                {
                    if (headers.ContainsValue("application/json"))
                    {
                        restRequest.AddJsonBody(body);
                    }
                    else if(headers.ContainsValue("application/xml"))
                    {
                        restRequest.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
                       restRequest.AddParameter("XmlData",body.GetType().Equals(typeof(string)) ? body : restRequest.XmlSerializer.Serialize(body),ParameterType.RequestBody);
                    }
                }

            }

            return restRequest;
        }

        private IRestResponse SendRequest(IRestRequest restRequest)
        {
            IRestClient restClient = GetRestClient();
            IRestResponse response = restClient.Execute(restRequest);
            return response;
        }

        private IRestResponse<T> SendRequest<T>(IRestRequest restRequest) where T : new()
        {
            IRestClient restClient = GetRestClient();
            IRestResponse<T> response = restClient.Execute<T>(restRequest);

            if (response.ContentType.Equals("application/xml"))
            {
                var dotnetxmlderializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
                response.Data = dotnetxmlderializer.Deserialize<T>(response);
            }

            return response;
        }

        public IRestResponse PerformGetRequest(string url, Dictionary<string, string> headers)
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.GET, null);
            IRestResponse response = SendRequest(restRequest);
            return response;
        }

        public IRestResponse<T> PerformGetRequest<T>(string url, Dictionary<string, string> headers) where T : new()
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.GET, null);
            IRestResponse<T> response = SendRequest<T>(restRequest);
            return response;
        }

        public IRestResponse<T> PerformPostRequest<T>(string url, Dictionary<string, string> headers, object body) where T : new()
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.POST, body);
            IRestResponse<T> response = SendRequest<T>(restRequest);
            return response;
        }

        public IRestResponse PerformPostRequest(string url, Dictionary<string, string> headers, object body)
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.POST, body);
            IRestResponse response = SendRequest(restRequest);
            return response;
        }

        public IRestResponse<T> PerformPutRequest<T>(string url, Dictionary<string, string> headers, object body) where T : new()
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.PUT, body);
            IRestResponse<T> response = SendRequest<T>(restRequest);
            return response;
        }

        public IRestResponse PerformPutRequest(string url, Dictionary<string, string> headers, object body)
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.PUT, body);
            IRestResponse response = SendRequest(restRequest);
            return response;
        }

        public IRestResponse<T> PerformDeleteRequest<T>(string url, Dictionary<string, string> headers, object body) where T : new()
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.DELETE, body);
            IRestResponse<T> response = SendRequest<T>(restRequest);
            return response;
        }

        public IRestResponse PerformDeleteRequest(string url, Dictionary<string, string> headers, object body)
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.DELETE, body);
            IRestResponse response = SendRequest(restRequest);
            return response;
        }
    }
}
