using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;

namespace RestSharpAutomation.RestPutEndPoint
{
    [TestClass]
    public class TestPutEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string putUrl = "http://localhost:8080/laptop-bag/webapi/api/update";
        private Random random = new Random();

        [TestMethod]
        public void TestPutUsingJsonData()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
 
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
            headers.Add("Content-Type", "application/json");
            headers.Add("Accept", "application/json");
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse response = restClientHelper.PerformPostRequest(postUrl, headers, jsonData);

            jsonData = "{" +
                                "\"BrandName\": \"Alienware\"," +
                                "\"Features\": {" +
                                "\"Feature\": [" +
                                "\"8th Generation Intel® Core™ i5-8300H\"," +
                                "\"Windows 10 Home 64-bit English\"," +
                                "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                                "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
                                 "\"New Data As per Put request\"" +
                                "]" +
                                "}," +
                                "\"Id\": " + id + "," +
                                "\"LaptopName\": \"Alienware M17\"" +
                            "}";

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(putUrl);
            restRequest.AddHeaders(headers);
            restRequest.AddJsonBody(jsonData);
            IRestResponse<JsonRootObject> response1 = restClient.Put<JsonRootObject>(restRequest);
            Assert.AreEqual(200, (int) response1.StatusCode);

            response1 = restClientHelper.PerformGetRequest<JsonRootObject>(getUrl + id, headers);
            Assert.AreEqual("New Data As per Put request", response1.Data.Features.Feature[4]);
        }


        [TestMethod]
        public void TestPutUsingXmlData()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();

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
            headers.Add("Content-Type", "application/xml");
            headers.Add("Accept", "application/xml");
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse response = restClientHelper.PerformPostRequest(postUrl, headers, xmlData);

             xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                       "<Feature>8th Generation Intel® Core™ i5 - 8300H</Feature>" +
                                       "<Feature>Windows 10 Home 64 - bit English</Feature>" +
                                       "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                       "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                       "<Feature>New Data As per Put request</Feature>" +
                                     "</Features>" +
                                  "<Id>" + id + "</Id>" +
                                  "<LaptopName>Alienware M17</LaptopName>" +
                               "</Laptop>";

            IRestClient restClient = new RestClient();


            IRestRequest restRequest = new RestRequest(putUrl);
            restRequest.AddHeaders(headers);
            restRequest.AddParameter("XmlData",xmlData, ParameterType.RequestBody);
            IRestResponse<Laptop> response1 = restClient.Put<Laptop>(restRequest);
            var dotnetxmlderializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
            var laptop = dotnetxmlderializer.Deserialize<Laptop>(response1);

            Assert.AreEqual(200, (int)response1.StatusCode );

            response1 = restClientHelper.PerformGetRequest<Laptop>(getUrl + id, headers);
            Assert.AreEqual("New Data As per Put request", response1.Data.Features.Feature[4]);
        }


        [TestMethod]
        public void TestPutUsingHelperClassJsonData()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();

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

            headers.Add("Content-Type", "application/json");
            headers.Add("Accept", "application/json");
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse response = restClientHelper.PerformPostRequest(postUrl, headers, jsonData);

            jsonData = "{" +
                                "\"BrandName\": \"Alienware\"," +
                                "\"Features\": {" +
                                "\"Feature\": [" +
                                "\"8th Generation Intel® Core™ i5-8300H\"," +
                                "\"Windows 10 Home 64-bit English\"," +
                                "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                                "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
                                 "\"New Data As per Put request\"" +
                                "]" +
                                "}," +
                                "\"Id\": " + id + "," +
                                "\"LaptopName\": \"Alienware M17\"" +
                            "}";

            //IRestClient restClient = new RestClient();
            //IRestRequest restRequest = new RestRequest(putUrl);
            //restRequest.AddHeaders(headers);
            //restRequest.AddJsonBody(jsonData);
            //IRestResponse<JsonRootObject> response1 = restClient.Put<JsonRootObject>(restRequest);
            //Assert.AreEqual(200, (int)response1.StatusCode);

            IRestResponse<JsonRootObject> response1 = restClientHelper.PerformPutRequest<JsonRootObject>(putUrl, headers, jsonData);

            response1 = restClientHelper.PerformGetRequest<JsonRootObject>(getUrl + id, headers);
            Assert.AreEqual("New Data As per Put request", response1.Data.Features.Feature[4]);
        }


    }
}
