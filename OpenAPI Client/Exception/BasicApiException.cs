using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAPI_Client.Exception
{
    class BasicApiException : System.ApplicationException
    {
        public BasicApiException() {}
        public BasicApiException(string status, string message) {}        
    }
}
