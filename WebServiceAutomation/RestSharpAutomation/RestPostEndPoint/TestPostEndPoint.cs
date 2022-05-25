using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.XmlModel;

namespace RestSharpAutomation.RestPostEndPoint
{
    [TestClass]
    public class TestPostEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private Random random = new Random();

        [TestMethod]
        public void TestPostWithJsonData()
        {
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
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Accept", "application/xml");
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(postUrl);
            restRequest.AddHeaders(headers);
            restRequest.AddJsonBody(jsonData);
            IRestResponse response = restClient.Post(restRequest);
            Console.WriteLine(response.Content);

        }

        [TestMethod]
        private Laptop GetLaptopObject()
        {
            int id = random.Next(1000);
            Laptop laptop = new Laptop();
            laptop.BrandName = "Sample Brand Name";
            laptop.LaptopName = "Sample Laptop Name";
            laptop.Id = "" + id;

            Features feautres = new Features();
            List<string> featureList = new List<string>();
            featureList.Add("Sample feautre");
            feautres.Feature = featureList;
            laptop.Features = feautres;

            return laptop;

        }

        [TestMethod]
        public void TestPostWithModelObject()
        {
            
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Accept", "application/xml");
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(postUrl);
            restRequest.AddHeaders(headers);
            restRequest.AddJsonBody(GetLaptopObject());
            IRestResponse response = restClient.Post(restRequest);
            Console.WriteLine(response.Content);
            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [TestMethod]
        public void TestPostWithModelObject_HelperMethod ()
        {

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Accept", "application/xml"); 
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse<Laptop> response = restClientHelper.PerformPostRequest<Laptop>(postUrl, headers, GetLaptopObject());
            Console.WriteLine(response.Data.BrandName);


        }

        [TestMethod]
        public void TestPostWithXmlData()
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
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/xml");
            headers.Add("Accept", "application/xml");
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(postUrl);
            restRequest.AddHeaders(headers);
            //restRequest.AddXmlBody(xmlData);
            restRequest.AddParameter("XmlData", xmlData, ParameterType.RequestBody);
            IRestResponse<Laptop> response = restClient.Post<Laptop>(restRequest);
            Console.WriteLine(response.Content);
            Assert.AreEqual(200, (int)response.StatusCode);

        }

        [TestMethod]
        public void TestPostWithModelObject_Xml()
        {

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/xml");
            headers.Add("Accept", "application/xml");
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(postUrl);
            restRequest.AddHeaders(headers);
            //restRequest.RequestFormat = DataFormat.Xml;
            restRequest.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
            restRequest.AddParameter("XmlData", restRequest.XmlSerializer.Serialize(GetLaptopObject()), ParameterType.RequestBody);
            IRestResponse response = restClient.Post(restRequest);
            Console.WriteLine(response.Content);
            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [TestMethod]
        public void TestPostWithModelObjectUsingHelper_Xml()
        {

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/xml");
            headers.Add("Accept", "application/xml");



            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse<Laptop> response= restClientHelper.PerformPostRequest<Laptop>(postUrl, headers, GetLaptopObject());

            //IRestClient restClient = new RestClient();
            //IRestRequest restRequest = new RestRequest(postUrl);
            //restRequest.AddHeaders(headers);
            ////restRequest.RequestFormat = DataFormat.Xml;
            //restRequest.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
            //restRequest.AddParameter("XmlData", restRequest.XmlSerializer.Serialize(GetLaptopObject()), ParameterType.RequestBody);
            //IRestResponse response = restClient.Post(restRequest);
            //Console.WriteLine(response.Content);
            //Assert.AreEqual(200, (int)response.StatusCode);
        }

        [TestMethod]
        public void TestPostWithStringObjectUsingHelper_Xml()
        {

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/xml");
            headers.Add("Accept", "application/xml");
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

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse<Laptop> response = restClientHelper.PerformPostRequest<Laptop>(postUrl, headers, xmlData);

            //IRestClient restClient = new RestClient();
            //IRestRequest restRequest = new RestRequest(postUrl);
            //restRequest.AddHeaders(headers);
            ////restRequest.RequestFormat = DataFormat.Xml;
            //restRequest.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
            //restRequest.AddParameter("XmlData", restRequest.XmlSerializer.Serialize(GetLaptopObject()), ParameterType.RequestBody);
            //IRestResponse response = restClient.Post(restRequest);
            //Console.WriteLine(response.Content);
            //Assert.AreEqual(200, (int)response.StatusCode);
        }

    }
}
