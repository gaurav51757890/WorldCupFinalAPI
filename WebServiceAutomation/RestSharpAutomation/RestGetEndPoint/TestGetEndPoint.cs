using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Authenticators;
using RestSharpAutomation.HelperClass.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;

namespace RestSharpAutomation.RestGetEndPoint
{
    [TestClass]
    public class TestGetEndPoint
    {
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/all";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/all";

        [TestMethod]
        public void TestGetUsingRestSharp()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            IRestResponse response = restClient.Get(restRequest);

            if (response.IsSuccessful)
            {
                Console.WriteLine("StatusCode = > " + response.StatusCode);
                Console.WriteLine("Content = > " + response.Content);
            }
        }

        [TestMethod]
        public void TestGetUsingRestSharpInXmlFormat()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("Accept", "application/xml");
            IRestResponse response = restClient.Get(restRequest);

            if (response.IsSuccessful)
            {
                Console.WriteLine("StatusCode = > " + response.StatusCode);
                Console.WriteLine("Content = > " + response.Content);
            }
        }

        [TestMethod]
        public void TestGetUsingRestSharp_JsonDeserialization()
        {

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("Accept", "application/json");
            IRestResponse<List<JsonRootObject>> responseData = restClient.Get<List<JsonRootObject>>(restRequest);
            if (responseData.IsSuccessful)
            {
                JsonRootObject jsonRootObject = null;
                Console.WriteLine("StatusCode = > " + responseData.StatusCode);
                Assert.AreEqual(200, (int)responseData.StatusCode);
                Console.WriteLine("Size of the list = > " + responseData.Data.Count);

                List<JsonRootObject> data = responseData.Data;

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].Id == 6)
                    {
                        jsonRootObject = responseData.Data[i];
                        break;
                    }
                }
                Assert.AreEqual("Alienware", jsonRootObject.BrandName);
            }

        }

        [TestMethod]
        public void TestGetUsingRestSharp_XmlDeserialization()
        {

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("Accept", "application/xml");

            var dotnetxmlderializer = new RestSharp.Deserializers.DotNetXmlDeserializer();



            // IRestResponse<LaptopDetailss> responseData = restClient.Get<LaptopDetailss>(restRequest);
            IRestResponse responseData = restClient.Get(restRequest);
            if (responseData.IsSuccessful)
            {

                Console.WriteLine("StatusCode = > " + responseData.StatusCode);
                Assert.AreEqual(200, (int)responseData.StatusCode);

                Laptop lap = null;
                LaptopDetailss data = dotnetxmlderializer.Deserialize<LaptopDetailss>(responseData);
                Console.WriteLine("Count is = > " + data.Laptop.Count);
                for (int i = 0; i < data.Laptop.Count; i++)
                {
                    if (data.Laptop[i].Id.Equals("6"))
                    {
                        lap = data.Laptop[i];
                        break;
                    }
                }
                Assert.AreEqual("Alienware", lap.BrandName);
            }

        }

        [TestMethod]
        public void TestGetUsingRestSharp_Execute()
        {

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("Accept", "application/json");       
            IRestResponse<List<Laptop>> response = restClient.Execute<List<Laptop>>(restRequest);
        }

        [TestMethod]
        public void TestGetWithXmlUsingHelperClass()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/xml");
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformGetRequest(getUrl, headers);
            Assert.AreEqual(200, (int) restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content, "Content is null");

            IRestResponse<LaptopDetailss> restResponse1 = restClientHelper.PerformGetRequest<LaptopDetailss>(getUrl, headers);
            Assert.AreEqual(200, (int)restResponse1.StatusCode);
            Assert.IsNotNull(restResponse1.Data, "Content is null");

        }

        [TestMethod]
        public void TestGetWithJsonUsingHelperClass()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformGetRequest(getUrl, headers);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content, "Content is null");
            

            IRestResponse<List<JsonRootObject>> restResponse1 = restClientHelper.PerformGetRequest<List<JsonRootObject>>(getUrl, headers);
            Assert.AreEqual(200, (int)restResponse1.StatusCode);
            Assert.IsNotNull(restResponse1.Data.Count, "Content is null");

        }

        [TestMethod]
        public void TestSecureGet()
        {
            IRestClient restClient = new RestClient();
            restClient.Authenticator = new HttpBasicAuthenticator("admin", "welcome");
            IRestRequest restRequest = new RestRequest(secureGetUrl);
            IRestResponse response = restClient.Get(restRequest);
            Assert.AreEqual(200, (int)response.StatusCode);
        }
    }
}
