using Bol.OpenAPI.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace Bol.OpenAPI.Client
{
    /// <summary>
    /// OpenAPI client.
    /// </summary>
    public class OpenApiClient
    {
        private const string URL_PREFIX = "https://openapi.bol.com/openapi/services/rest";
        private string accessKeyId;
        private string secretAccessKey;

        /// <summary>
        /// Constructs the OpenAPI client.
        /// </summary>
        /// <param name="accessKeyId">The access key id.</param>
        /// <param name="secretAccessKey">The secret access key.</param>
        public OpenApiClient(string accessKeyId, string secretAccessKey)
        {
            this.accessKeyId = accessKeyId;
            this.secretAccessKey = secretAccessKey;
        }

        /// <summary>
        /// Pings the OpenAPI server.
        /// </summary>
        /// <returns>True when the server pongs back.</returns>
        public Boolean Ping()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/utils/v3/ping");
            request.Method = "GET";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey, null, null);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Close();

            return HttpStatusCode.OK == response.StatusCode;
        }

        /// <summary>
        /// Searches for products.
        /// </summary>
        /// <param name="searchResultsRequest">The search results request.</param>
        /// <returns>The search results response.</returns>
        public SearchResultsResponse Search(SearchResultsRequest searchResultsRequest)
        {
            SearchResultsResponse searchResultsResponse = null;

            // Prepare request
            NameValueCollection queryParams = new NameValueCollection();
            if (searchResultsRequest.Term != null)
            {
                queryParams.Add("term", searchResultsRequest.Term);
            }
            if (searchResultsRequest.Offset != null)
            {
                queryParams.Add("offset", searchResultsRequest.Offset.ToString());
            }
            if (searchResultsRequest.NrProducts != null)
            {
                queryParams.Add("nrProducts", searchResultsRequest.NrProducts.ToString());
            }
            if (searchResultsRequest.SortingMethod != null)
            {
                queryParams.Add("sortingMethod", EnumUtils.stringValueOf(searchResultsRequest.SortingMethod));
            }
            if (searchResultsRequest.SortingAscending != null)
            {
                queryParams.Add("sortingAscending", searchResultsRequest.SortingAscending.ToString());
            }
            if (searchResultsRequest.IncludeProducts != null)
            {
                queryParams.Add("includeProducts", searchResultsRequest.IncludeProducts.ToString());
            }
            if (searchResultsRequest.IncludeCategories != null)
            {
                queryParams.Add("includeCategories", searchResultsRequest.IncludeCategories.ToString());
            }
            if (searchResultsRequest.IncludeRefinements != null)
            {
                queryParams.Add("includeRefinements", searchResultsRequest.IncludeRefinements.ToString());
            }
            if (searchResultsRequest.IncludeAttributes != null)
            {
                queryParams.Add("includeAttributes", searchResultsRequest.IncludeAttributes.ToString());
            }

            string queryString = ToQueryString(queryParams);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/catalog/v3/searchresults/" + queryString);
            request.Method = "GET";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey, null, queryParams);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Load XML document
            if (HttpStatusCode.OK == response.StatusCode)
            {
                XmlSerializer ser = new XmlSerializer(typeof(SearchResultsResponse));
                object obj = ser.Deserialize(response.GetResponseStream());
                searchResultsResponse = (SearchResultsResponse)obj;
                response.Close();
            }

            return searchResultsResponse;
        }

        /// <summary>
        /// Gets the product or category/refinement results list.
        /// </summary>
        /// <param name="listResultRequest">The list result request.</param>
        /// <returns>The list result response.</returns>
        public ListResultResponse GetList(ListResultRequest listResultRequest)
        {
            ListResultResponse listResultResponse = null;

            // Prepare request
            NameValueCollection queryParams = new NameValueCollection();
            if (listResultRequest.Offset != null)
            {
                queryParams.Add("offset", listResultRequest.Offset.ToString());
            }
            if (listResultRequest.NrProducts != null)
            {
                queryParams.Add("nrProducts", listResultRequest.NrProducts.ToString());
            }
            if (listResultRequest.SortingMethod != null)
            {
                queryParams.Add("sortingMethod", EnumUtils.stringValueOf(listResultRequest.SortingMethod));
            }
            if (listResultRequest.SortingAscending != null)
            {
                queryParams.Add("sortingAscending", listResultRequest.SortingAscending.ToString());
            }
            if (listResultRequest.IncludeProducts != null)
            {
                queryParams.Add("includeProducts", listResultRequest.IncludeProducts.ToString());
            }
            if (listResultRequest.IncludeCategories != null)
            {
                queryParams.Add("includeCategories", listResultRequest.IncludeCategories.ToString());
            }
            if (listResultRequest.IncludeRefinements != null)
            {
                queryParams.Add("includeRefinements", listResultRequest.IncludeRefinements.ToString());
            }
            if (listResultRequest.IncludeAttributes != null)
            {
                queryParams.Add("includeAttributes", listResultRequest.IncludeAttributes.ToString());
            }

            string queryString = ToQueryString(queryParams);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/catalog/v3/listresults/" + EnumUtils.stringValueOf(listResultRequest.Type) + "/" + HttpUtility.UrlEncode(combineCategoryAndRefinementIds(listResultRequest.CategoryId, listResultRequest.RefinementIds), UTF8Encoding.UTF8) + "/" + queryString);
            request.Method = "GET";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey, null, queryParams);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Load XML document
            if (HttpStatusCode.OK == response.StatusCode)
            {
                XmlSerializer ser = new XmlSerializer(typeof(ListResultResponse));
                object obj = ser.Deserialize(response.GetResponseStream());
                listResultResponse = (ListResultResponse)obj;
                response.Close();
            }

            return listResultResponse;
        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <returns>The product response.</returns>
        public ProductResponse GetProduct(string id)
        {
            ProductResponse productResponse = null;

            // Prepare request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/catalog/v3/products/" + id);
            request.Method = "GET";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Load XML document
            if (HttpStatusCode.OK == response.StatusCode)
            {
                XmlSerializer ser = new XmlSerializer(typeof(ProductResponse));
                object obj = ser.Deserialize(response.GetResponseStream());
                productResponse = (ProductResponse)obj;
                response.Close();
            }

            return productResponse;
        }

        /// <summary>
        /// Converts a name-value collection into a query string.
        /// </summary>
        /// <param name="nvc">The name-value collection.</param>
        /// <returns>The query string.</returns>
        private string ToQueryString(NameValueCollection nvc)
        {
            if (nvc != null && nvc.Count > 0)
            {
                // Does not support multi-value parameters, but for now we can accept that
                return "?" + string.Join("&", Array.ConvertAll(nvc.AllKeys, key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(nvc[key]))));
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Combines the category and refinement ids.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="refinementIds">The list of refinement ids.</param>
        /// <returns>The combined string of category and refinement ids.</returns>
        private String combineCategoryAndRefinementIds(string categoryId, List<string> refinementIds)
        {
            StringBuilder sb = new StringBuilder();
            if (categoryId != null)
            {
                sb.Append(categoryId);
            }
            if (refinementIds != null && refinementIds.Count > 0)
            {
                sb.Append(' ');

                for (int i = 0; i < refinementIds.Count; i++)
                {
                    sb.Append(refinementIds[i]);

                    if (i + 1 < refinementIds.Count)
                    {
                        sb.Append(' ');
                    }
                }
            }

            return sb.ToString();
        }
    }
}
