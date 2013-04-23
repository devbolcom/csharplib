using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAPI_Client.Request
{
    public class ProductRecommendationsRequest
    {
        public ProductRecommendationsRequest(string id)
        {
            this.Id = id;        
        }

        public string Id { get; set; }
        public Boolean? IncludeProducts { get; set; }
        public Boolean? IncludeAttributes { get; set; }
        public Boolean? IncludeAllOffers { get; set; }
        public Int32? NrProducts { get; set; }
        public Int64? Offset { get; set; }
    }
}
