using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Model;

namespace WebServiceAutomation.DeleteEndPoint
{
    [TestClass]
    public class TestDeleteEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string deleteUrl = "http://localhost:8080/laptop-bag/webapi/api/delete/";
        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();
        private RestResponse restResponse;

        [TestMethod]
        public void TestDelete()
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


           // Task<HttpResponseMessage> httpResponseMessage = httpClient.DeleteAsync(deleteUrl + id);

            restResponse = HttpClientHelper.PerformDeleteRequest(deleteUrl + id);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PeformGetRequest(getUrl + id, httpHeaders);
            Assert.AreEqual(404, restResponse.StatusCode);


        }

    }
}
