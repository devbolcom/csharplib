using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAPI_Client.Exception
{
    public class BasicApiException : System.ApplicationException
    {
        public string Status { get; set; }

        public BasicApiException() {}
        public BasicApiException(string status, string message)
            : base(message)
        {            
            this.Status = status;
        }        
    }
}
