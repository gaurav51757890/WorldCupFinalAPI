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

namespace WebServiceAutomation.PostEndPoint
{
    [TestClass]
    public class TestPostEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string securePostUrl = "http://localhost:8080/laptop-bag/webapi/secure/add";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/find/";
        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();
        private RestResponse restResponseGet;
        private RestResponse restResponse;
        [TestMethod]
        public void TestPostEndPointWithJson()
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
                                    "\"Id\": "+id+"," +
                                    "\"LaptopName\": \"Alienware M17\"" +
                                "}";

            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);

            Task<HttpResponseMessage> httpResponseMessage =  httpClient.PostAsync(postUrl, httpContent);
            /* HttpResponseMessage responseMessage = httpResponseMessage.Result;
             HttpStatusCode statusCode = responseMessage.StatusCode;*/

            HttpStatusCode statusCode = httpResponseMessage.Result.StatusCode;

            HttpContent content = httpResponseMessage.Result.Content;
            Task<string>  responseData = content.ReadAsStringAsync();
            string data = responseData.Result;

            RestResponse restResponse= new RestResponse((int)statusCode, data);
            Assert.AreEqual(200, restResponse.StatusCode);
            Assert.IsNotNull(restResponse.ResponseData, "Response Data is not null");

            Task<HttpResponseMessage>  getResponse = httpClient.GetAsync(getUrl + id);
            restResponseGet = new RestResponse((int)getResponse.Result.StatusCode, getResponse.Result.Content.ReadAsStringAsync().Result);

            JsonRootObject jsonObject = JsonConvert.DeserializeObject<JsonRootObject>(getResponse.Result.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(id, jsonObject.Id);


        }

        [TestMethod]
        public void TestPostEndPointWithXml()
        {
            HttpClient httpClient = new HttpClient();
            int id = random.Next(1000);
            string xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                       "<Feature>8th Generation Intel® Core™ i5 - 8300H</Feature>" +
                                       "<Feature>Windows 10 Home 64 - bit English</Feature>" +
                                       "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                       "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                     "</Features>" +
                                  "<Id>"+id+"</Id>" +
                                  "<LaptopName>Alienware M17</LaptopName>" +
                               "</Laptop>";

            HttpContent httpContent = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);

            Task<HttpResponseMessage> httpResponseMessage = httpClient.PostAsync(postUrl, httpContent);
 
            HttpStatusCode statusCode = httpResponseMessage.Result.StatusCode;

            HttpContent content = httpResponseMessage.Result.Content;
            Task<string> responseData = content.ReadAsStringAsync();
            string data = responseData.Result;

            RestResponse restResponse = new RestResponse((int)statusCode, data);
            Assert.AreEqual(200, restResponse.StatusCode);
            Assert.IsNotNull(restResponse.ResponseData, "Response Data is not null");

            httpResponseMessage = httpClient.GetAsync(getUrl + id);
            if(!httpResponseMessage.Result.IsSuccessStatusCode)
            {
                Assert.Fail("The HTTP response was not successfull");
            }

             restResponse = new RestResponse((int)statusCode, data);

            //Step-1
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Laptop));

            //Step-2 Create the instance of TextReader
            TextReader textReader = new StringReader(restResponse.ResponseData);

            Laptop xmldata = (Laptop)xmlSerializer.Deserialize(textReader);

            Assert.IsTrue(xmldata.Features.Feature.Contains("Windows 10 Home 64 - bit English"), "Item Not found");
        }

        [TestMethod]
        public void TestPostUsingHelperClass()
        {
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

            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Accept", "application/xml");

            restResponse = HttpClientHelper.PerformPostRequest(postUrl, xmlData, xmlMediaType, httpHeaders);

            Assert.AreEqual(200, (int)restResponse.StatusCode);
           Laptop xmlDataU=  ResponseDataHelper.DeserializationXmlResponse<Laptop>(restResponse.ResponseData);

        }

        [TestMethod]
        public void TestSecurePostEndPoint()
        {
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

            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Accept", "application/xml");
            string authHeader = "Basic " + Base64StringConverator.GetBase64String("admin", "welcome");
            httpHeaders.Add("Authorization", authHeader);

            restResponse = HttpClientHelper.PerformPostRequest(securePostUrl, xmlData, xmlMediaType, httpHeaders);

            Assert.AreEqual(200, (int)restResponse.StatusCode);

            restResponse= HttpClientHelper.PeformGetRequest(secureGetUrl + id, httpHeaders);

            Assert.AreEqual(200, (int)restResponse.StatusCode);


            Laptop xmlDataU = ResponseDataHelper.DeserializationXmlResponse<Laptop>(restResponse.ResponseData);
            Assert.AreEqual("Alienware", xmlDataU.BrandName);

        }

    }
}
