using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Helper.Authentication;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Helper.ResponseData;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;

namespace WebServiceAutomation.PutEndPoint
{
    [TestClass]
    public class TestPutEndPoint
    {
        //Using Post request I will create the record.
        //Using Put request i will update the record.
        //Using Get request i will fetch the record and add the validations.
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string putUrl = "http://localhost:8080/laptop-bag/webapi/api/update";
        private string securePostUrl = "http://localhost:8080/laptop-bag/webapi/secure/add";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/find/";
        private string securePutUrl = "http://localhost:8080/laptop-bag/webapi/secure/update";

        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();
        private RestResponse restResponseGet;
        private RestResponse restResponse;

        [TestMethod]
        public void TestPutUsingXmlData()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", xmlMediaType);
            int id = random.Next(1000);
            string xmlData = "<Laptop>" +
                                     "<BrandName>Alienware</BrandName>" +
                                     "<Features>" +
                                        "<Feature>8th Generation Intel® Core™ i5 - 8300H</Feature>" +
                                        "<Feature>Windows 10 Home 64 - bit English</Feature>" +
                                        "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                      "</Features>" +
                                   "<Id>" + id + "</Id>" +
                                   "<LaptopName>Alienware M17</LaptopName>" +
                                "</Laptop>";


            

            HttpContent httpContent = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Accept", "application/xml");
            restResponse =  HttpClientHelper.PerformPostRequest(postUrl,httpContent, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);

            xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                       "<Feature>8th Generation Intel® Core™ i5 - 8300H</Feature>" +
                                       "<Feature>Windows 10 Home 64 - bit English</Feature>" +
                                       "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                       "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                        "<Feature>1 TB of SSD</Feature>" +
                                     "</Features>" +
                                  "<Id>" + id + "</Id>" +
                                  "<LaptopName>Alienware M17</LaptopName>" +
                               "</Laptop>";


            httpContent = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);
            Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(putUrl, httpContent);
            restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PeformGetRequest(getUrl + id, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);
            Laptop data = ResponseDataHelper.DeserializationXmlResponse<Laptop>(restResponse.ResponseData);
            Assert.IsTrue(data.Features.Feature.Contains("1 TB of SSD"), "Item Not Found");

        }


        [TestMethod]
        public void TestPutUsingJsonData()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", jsonMediaType);
            int id = random.Next(1000);
            string jsonData = "{" +
                                  "\"BrandName\": \"Alienware\"," +
                                  "\"Features\": {" +
                                  "\"Feature\": [" +
                                  "\"8th Generation Intel® Core™ i5-8300H\"," +
                                  "\"Windows 10 Home 64-bit English\"," +
                                  "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                                  "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                                  "]" +
                                  "}," +
                                  "\"Id\": " + id + "," +
                                  "\"LaptopName\": \"Alienware M17\"" +
                              "}";
            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Accept", "application/json");
            restResponse = HttpClientHelper.PerformPostRequest(postUrl, httpContent, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);

             jsonData = "{" +
                                   "\"BrandName\": \"Alienware\"," +
                                   "\"Features\": {" +
                                   "\"Feature\": [" +
                                   "\"8th Generation Intel® Core™ i5-8300H\"," +
                                   "\"Windows 10 Home 64-bit English\"," +
                                   "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                                   "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
                                   "\"1 TB of SSD\"" +
                                   "]" +
                                   "}," +
                                   "\"Id\": " + id + "," +
                                   "\"LaptopName\": \"Alienware M17\"" +
                               "}";


            httpContent = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);
            Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(putUrl, httpContent);
            restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PeformGetRequest(getUrl + id, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);
            JsonRootObject data = ResponseDataHelper.DeserializationJsonResponse<JsonRootObject>(restResponse.ResponseData);
            Assert.IsTrue(data.Features.Feature.Contains("1 TB of SSD"), "Item Not Found");

        }

        [TestMethod]
        public void TestPutUsingHelperMethod()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", jsonMediaType);
            int id = random.Next(1000);
            string jsonData = "{" +
                                  "\"BrandName\": \"Alienware\"," +
                                  "\"Features\": {" +
                                  "\"Feature\": [" +
                                  "\"8th Generation Intel® Core™ i5-8300H\"," +
                                  "\"Windows 10 Home 64-bit English\"," +
                                  "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                                  "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                                  "]" +
                                  "}," +
                                  "\"Id\": " + id + "," +
                                  "\"LaptopName\": \"Alienware M17\"" +
                              "}";
            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Accept", "application/json");
            restResponse = HttpClientHelper.PerformPostRequest(postUrl, httpContent, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);

            jsonData = "{" +
                                  "\"BrandName\": \"Alienware\"," +
                                  "\"Features\": {" +
                                  "\"Feature\": [" +
                                  "\"8th Generation Intel® Core™ i5-8300H\"," +
                                  "\"Windows 10 Home 64-bit English\"," +
                                  "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                                  "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
                                  "\"1 TB of SSD\"" +
                                  "]" +
                                  "}," +
                                  "\"Id\": " + id + "," +
                                  "\"LaptopName\": \"Alienware M17\"" +
                              "}";


            httpContent = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);
            restResponse = HttpClientHelper.PeformPutRequest(putUrl, httpContent, httpHeaders);

            //Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(putUrl, httpContent);
            //restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PeformGetRequest(getUrl + id, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);
            JsonRootObject data = ResponseDataHelper.DeserializationJsonResponse<JsonRootObject>(restResponse.ResponseData);
            Assert.IsTrue(data.Features.Feature.Contains("1 TB of SSD"), "Item Not Found");

            restResponse = HttpClientHelper.PeformGetRequest(getUrl + id, httpHeaders);
            Assert.AreEqual(404, restResponse.StatusCode);

        }

        [TestMethod]
        public void TestPutUsingSecureMethod()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", jsonMediaType);
            int id = random.Next(1000);
            string jsonData = "{" +
                                  "\"BrandName\": \"Alienware\"," +
                                  "\"Features\": {" +
                                  "\"Feature\": [" +
                                  "\"8th Generation Intel® Core™ i5-8300H\"," +
                                  "\"Windows 10 Home 64-bit English\"," +
                                  "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                                  "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                                  "]" +
                                  "}," +
                                  "\"Id\": " + id + "," +
                                  "\"LaptopName\": \"Alienware M17\"" +
                              "}";
            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Accept", "application/json");
            string authHeader = "Basic " + Base64StringConverator.GetBase64String("admin", "welcome");
            httpHeaders.Add("Authorization", authHeader);
            restResponse = HttpClientHelper.PerformPostRequest(securePostUrl, httpContent, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);

            jsonData = "{" +
                                  "\"BrandName\": \"Alienware\"," +
                                  "\"Features\": {" +
                                  "\"Feature\": [" +
                                  "\"8th Generation Intel® Core™ i5-8300H\"," +
                                  "\"Windows 10 Home 64-bit English\"," +
                                  "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                                  "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
                                  "\"1 TB of SSD\"" +
                                  "]" +
                                  "}," +
                                  "\"Id\": " + id + "," +
                                  "\"LaptopName\": \"Alienware M17\"" +
                              "}";


            httpContent = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);
            restResponse = HttpClientHelper.PeformPutRequest(securePutUrl, httpContent, httpHeaders);

            //Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(putUrl, httpContent);
            //restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PeformGetRequest(secureGetUrl + id, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);
            JsonRootObject data = ResponseDataHelper.DeserializationJsonResponse<JsonRootObject>(restResponse.ResponseData);
            Assert.IsTrue(data.Features.Feature.Contains("1 TB of SSD"), "Item Not Found");

            /*restResponse = HttpClientHelper.PeformGetRequest(getUrl + id, httpHeaders);
            Assert.AreEqual(404, restResponse.StatusCode);*/

        }
    }
}
