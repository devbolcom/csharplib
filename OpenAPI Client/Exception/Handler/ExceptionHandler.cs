using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenAPI_Client.Exception.Handler
{
    class ExceptionHandler
    {
        public static BasicApiException HandleBasicApiException(HttpWebResponse response)
        {             
            XmlSerializer ser = new XmlSerializer(typeof(Error));
            object obj = ser.Deserialize(response.GetResponseStream());
            Error error = (Error)obj;

            return new BasicApiException(error.Status, error.Message);
        }
    }
}
