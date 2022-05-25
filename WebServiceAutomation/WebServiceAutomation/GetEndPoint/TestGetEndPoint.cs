using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebServiceAutomation.Helper.Authentication;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Helper.ResponseData;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;

namespace WebServiceAutomation.GetEndPoint
{
    [TestClass]
    public class TestGetEndPoint
    {
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/all";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/all";

        [TestMethod]
        public void TestGetAllEndPoint()
        {
            //Step-1 To Create the HTTP Client
            HttpClient httpClient = new HttpClient();
            //Step-2 Create the request and execute it.
            httpClient.GetAsync(getUrl);
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointWithUri()
        {
            //Step-1 To Create the HTTP Client
            HttpClient httpClient = new HttpClient();

            //Step-2 Create the request and execute it.
            Uri uri = new Uri(getUrl);
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(uri);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;

            //Extract the status code from Response
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code is : -> " + statusCode);
            Console.WriteLine("Status Code is : -> " + (int)statusCode);

            //Extract the content from the Response
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine("Content is : -> " + data);
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointWithInvalidUri()
        {
            //Step-1 To Create the HTTP Client
            HttpClient httpClient = new HttpClient();

            //Step-2 Create the request and execute it.
            Uri uri = new Uri(getUrl + "hello");
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(uri);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;

            //Extract the status code from Response
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code is : -> " + statusCode);
            Console.WriteLine("Status Code is : -> " + (int)statusCode);

            //Extract the content from the Response
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine("Content is : -> " + data);
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointInJsonFormat()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
            requestHeaders.Add("Accept", "application/json");
            //Step-2 Create the request and execute it.
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUrl);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            //Extract the status code from Response
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code is : -> " + statusCode);
            Console.WriteLine("Status Code is : -> " + (int)statusCode);
            //Extract the content from the Response
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine("Content is : -> " + data);
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetEndPointWithSyncMethod()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
            requestHeaders.Add("Accept", "application/json");

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, getUrl);
            Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(requestMessage);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            //Extract the status code from Response
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            // Console.WriteLine("Status Code is : -> " + statusCode);
            //  Console.WriteLine("Status Code is : -> " + (int)statusCode);
            //Extract the content from the Response
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine("Content is : -> " + data);

            RestResponse restResponse = new RestResponse((int)statusCode, responseData.Result);
            Console.WriteLine(restResponse.ToString());

            httpClient.Dispose();

        }

        [TestMethod]
        public void TestDeserializationOfJsonResponse()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
            requestHeaders.Add("Accept", "application/json");
            //Step-2 Create the request and execute it.
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUrl);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            //Extract the status code from Response
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code is : -> " + statusCode);
            Console.WriteLine("Status Code is : -> " + (int)statusCode);
            //Extract the content from the Response
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine("Content is : -> " + data);
            List<JsonRootObject> jsonRootObject = JsonConvert.DeserializeObject<List<JsonRootObject>>(responseData.Result);
            Console.WriteLine(jsonRootObject[0].ToString());
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestDeserializationOfXmlResponse()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
            requestHeaders.Add("Accept", "application/xml");
            //Step-2 Create the request and execute it.
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUrl);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            //Extract the status code from Response
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code is : -> " + statusCode);
            Console.WriteLine("Status Code is : -> " + (int)statusCode);
            //Extract the content from the Response
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            //   Console.WriteLine("Content is : -> " + data);

            //Step-1
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LaptopDetailss));

            //Step-2 Create the instance of TextReader
            TextReader textReader = new StringReader(responseData.Result);

            //Step-3
            LaptopDetailss xmlData = (LaptopDetailss)xmlSerializer.Deserialize(textReader);

            Console.WriteLine(xmlData.ToString());

            Assert.AreEqual(200, (int)statusCode);
            Assert.IsNotNull(httpResponseMessage.Content);

          //  Assert.IsTrue(xmlData.Laptop.Features.Feature.Contains("Windows 10 Home 64-bit English"), "Item Not found");


        }

        [TestMethod]
        public void GetUsingHelperMethod()
        {
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Accept", "application/json");

            RestResponse restResponse = HttpClientHelper.PeformGetRequest(getUrl, httpHeaders);

            //List<JsonRootObject> jsonRootObject = JsonConvert.DeserializeObject<List<JsonRootObject>>(restResponse.ResponseData);
            //Console.WriteLine(jsonRootObject[0].ToString());

            List<JsonRootObject> data = ResponseDataHelper.DeserializationJsonResponse<List<JsonRootObject>>(restResponse.ResponseData);
            Console.WriteLine(data.ToString());
        }

        [TestMethod]
        public void TestSecureGetEndPoint()
        {
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Accept", "application/json");

            string authHeader = "Basic " + Base64StringConverator.GetBase64String("admin", "welcome");
            httpHeaders.Add("Authorization", authHeader);
            RestResponse restResponse = HttpClientHelper.PeformGetRequest(secureGetUrl, httpHeaders);

            Assert.AreEqual(200, (int)restResponse.StatusCode);

            List<JsonRootObject> data = ResponseDataHelper.DeserializationJsonResponse<List<JsonRootObject>>(restResponse.ResponseData);
            Console.WriteLine(data.ToString());
        }

        [TestMethod]
        public void TestEndGetPoint_async()
        {
            Task t1 = new Task(GetEndPoint());
            t1.Start();
            Task t2 = new Task(GetEndPoint());
            t2.Start();
            Task t3 = new Task(GetEndPoint());
            t3.Start();
            Task t4 = new Task(GetEndPoint());
            t4.Start();

            t1.Wait();
            t2.Wait();
            t3.Wait();
            t4.Wait();
        }

        private Action GetEndPoint()
        {
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Accept", "application/json");
            return new Action(() =>
            {
                HttpClientHelper.PeformGetRequest("http://localhost:8080/laptop-bag/webapi/delay/", httpHeaders);
            });
        }



    }
}
