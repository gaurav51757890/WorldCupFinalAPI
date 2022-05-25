using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.XmlModel;

namespace RestSharpAutomation.RestDeleteEndPoint
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
      
        [TestMethod]
        public void TestDeleteRequest()
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

            IRestClient restClient1 = new RestClient();
            IRestRequest restRequest1 = new RestRequest (deleteUrl + id);
            restRequest1.AddHeader("Accept","*/*");
            IRestResponse restResponse= restClient1.Delete(restRequest1);
            
        }

        [TestMethod]
        public void TestDeleteRequestUsingHelperMethod()
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
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse<Laptop> response = restClientHelper.PerformPostRequest<Laptop>(postUrl, headers, xmlData);

            headers = new Dictionary<string, string>()
            {{ "Accept","*/*"} };
            IRestResponse<Laptop> deleteresponse = restClientHelper.PerformDeleteRequest<Laptop>(deleteUrl + id, headers, null);
        }
    }
}
