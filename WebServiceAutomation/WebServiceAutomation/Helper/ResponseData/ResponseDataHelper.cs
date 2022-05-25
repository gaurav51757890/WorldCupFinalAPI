using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebServiceAutomation.Helper.ResponseData
{
    public class ResponseDataHelper
    {

        //JsonResponse
        //Xml Response
        

        //Laptop
        //LaptopHybrid
        //LaptopHybridV2

        public static T DeserializationJsonResponse<T>(string responseData) where T : class
        {
           return JsonConvert.DeserializeObject<T>(responseData);
        }

        public static T DeserializationXmlResponse<T>(string responseData) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            //Step-2 Create the instance of TextReader
            TextReader textReader = new StringReader(responseData);

            return (T)xmlSerializer.Deserialize(textReader);
        }

    }
}
