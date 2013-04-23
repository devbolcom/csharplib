using Bol.OpenAPI.Utils;
using OpenAPI_Client.Exception;
using OpenAPI_Client.Exception.Handler;
using OpenAPI_Client.Request;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
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
            Boolean result = false;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/utils/v3/ping");
            request.Method = "GET";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey, null, null);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                result = HttpStatusCode.OK == response.StatusCode;
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    throw ExceptionHandler.HandleBasicApiException(response);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (response != null) response.Close();
            }

            return result;
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
            if (searchResultsRequest.ListId != null)
            {
                queryParams.Add("listId", searchResultsRequest.ListId);
            }

            string queryString = ToQueryString(queryParams);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/catalog/v3/searchresults/" + queryString);
            request.Method = "GET";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey, null, queryParams);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();                
                // Load XML document
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    XmlSerializer ser = new XmlSerializer(typeof(SearchResultsResponse));
                    object obj = ser.Deserialize(response.GetResponseStream());
                    searchResultsResponse = (SearchResultsResponse)obj;                    
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    throw ExceptionHandler.HandleBasicApiException(response);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (response != null) response.Close();
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
            if (listResultRequest.ListId != null)
            {
                queryParams.Add("listId", listResultRequest.ListId);
            }

            string queryString = ToQueryString(queryParams);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/catalog/v3/listresults/" + EnumUtils.stringValueOf(listResultRequest.Type) + "/" + HttpUtility.UrlEncode(combineCategoryAndRefinementIds(listResultRequest.CategoryId, listResultRequest.RefinementIds), UTF8Encoding.UTF8) + "/" + queryString);
            request.Method = "GET";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey, null, queryParams);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                // Load XML document
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    XmlSerializer ser = new XmlSerializer(typeof(ListResultResponse));
                    object obj = ser.Deserialize(response.GetResponseStream());
                    listResultResponse = (ListResultResponse)obj;
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    throw ExceptionHandler.HandleBasicApiException(response);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (response != null) response.Close();
            }

            

            return listResultResponse;
        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <returns>The product response.</returns>
        public ProductResponse GetProduct(string id, Boolean? includeAttributes)
        {
            ProductResponse productResponse = null;

            // Prepare request
            NameValueCollection queryParams = new NameValueCollection();
            if (includeAttributes != null)
            {
                queryParams.Add("includeAttributes", includeAttributes.ToString());
            }

            string queryString = ToQueryString(queryParams);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/catalog/v3/products/" + id + "/" + queryString);
            request.Method = "GET";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey, null, queryParams);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                // Load XML document
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    XmlSerializer ser = new XmlSerializer(typeof(ProductResponse));
                    object obj = ser.Deserialize(response.GetResponseStream());
                    productResponse = (ProductResponse)obj;
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    throw ExceptionHandler.HandleBasicApiException(response);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (response != null) response.Close();
            }            

            return productResponse;
        }

        /// <summary>
        /// Gets the product recommendations.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <returns>The product recommendations response.</returns>
        public ProductRecommendationsResponse GetProductRecommendations(ProductRecommendationsRequest productRecommendationsRequest)
        {
            ProductRecommendationsResponse productRecommendationsResponse = null;

            // Prepare request
            NameValueCollection queryParams = new NameValueCollection();
            if (productRecommendationsRequest.Offset != null)
            {
                queryParams.Add("offset", productRecommendationsRequest.Offset.ToString());
            }
            if (productRecommendationsRequest.NrProducts != null)
            {
                queryParams.Add("nrProducts", productRecommendationsRequest.NrProducts.ToString());
            }
            if (productRecommendationsRequest.IncludeProducts != null)
            {
                queryParams.Add("includeProducts", productRecommendationsRequest.IncludeProducts.ToString());
            }
            if (productRecommendationsRequest.IncludeAttributes != null)
            {
                queryParams.Add("includeAttributes", productRecommendationsRequest.IncludeAttributes.ToString());
            }
            if (productRecommendationsRequest.IncludeAllOffers != null)
            {
                queryParams.Add("includeAllOffers", productRecommendationsRequest.IncludeAllOffers.ToString());
            }

            string queryString = ToQueryString(queryParams);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/catalog/v3/recommendations/" + productRecommendationsRequest.Id + "/" + queryString);
            request.Method = "GET";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey, null, queryParams);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                // Load XML document
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    XmlSerializer ser = new XmlSerializer(typeof(ProductRecommendationsResponse));
                    object obj = ser.Deserialize(response.GetResponseStream());
                    productRecommendationsResponse = (ProductRecommendationsResponse)obj;
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    throw ExceptionHandler.HandleBasicApiException(response);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (response != null) response.Close();
            }            

            return productRecommendationsResponse;
        }

        /// <summary>
        /// Gets an anonymous session.
        /// </summary>
        /// <returns>The anonymous session id.</returns>
        public string GetAnonymousSession()
        {
            string sessionId = null;

            // Prepare request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/auth/v3/session/");
            request.Method = "GET";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                // Load XML document
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    XmlSerializer ser = new XmlSerializer(typeof(SessionResponse));
                    object obj = ser.Deserialize(response.GetResponseStream());
                    SessionResponse sessionResponse = (SessionResponse)obj;
                    sessionId = sessionResponse.SessionId;
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    throw ExceptionHandler.HandleBasicApiException(response);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (response != null) response.Close();
            }

            return sessionId;
        }

        /// <summary>
        /// Gets the basket.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>The basket reponse.</returns>
        public BasketResponse GetBasket(string sessionId)
        {
            BasketResponse basketResponse = null;

            // Prepare request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/checkout/v3/baskets/");
            request.Method = "GET";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey, sessionId, null);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                // Load XML document
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    XmlSerializer ser = new XmlSerializer(typeof(BasketResponse));
                    object obj = ser.Deserialize(response.GetResponseStream());
                    basketResponse = (BasketResponse)obj;
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    throw ExceptionHandler.HandleBasicApiException(response);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (response != null) response.Close();
            }

            return basketResponse;
        }

        /// <summary>
        /// Adds the item to the basket.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="offerId">The offer id.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="ipAddress">The IP address.</param>
        public Boolean AddItemToBasket(string sessionId, String offerId, Int32 quantity, string ipAddress)
        {
            Boolean result = false;

            // Prepare request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/checkout/v3/baskets/" + offerId + "/" + quantity + "/" + ipAddress);
            request.Method = "POST";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey, sessionId, null, null);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                result = HttpStatusCode.Created == response.StatusCode;
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    throw ExceptionHandler.HandleBasicApiException(response);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (response != null) response.Close();
            }

            return result;
        }

        /// <summary>
        /// Removes the item from the basket. 
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="basketItemId">The basket item id.</param>
        /// <returns>Whether the item wasremoved or not.</returns>
        public Boolean RemoveItemFromBasket(string sessionId, string basketItemId)
        {
            Boolean result = false;

            // Prepare request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/checkout/v3/baskets/" + basketItemId);
            request.Method = "DELETE";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey, sessionId, null, null);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                result = HttpStatusCode.OK == response.StatusCode;
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    throw ExceptionHandler.HandleBasicApiException(response);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (response != null) response.Close();
            }

            return result;
        }

        /// <summary>
        /// Changes the item quantity in the basket.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="basketItemId">The basket item id.</param>
        /// <param name="quantity">The quantity.</param>
        /// <returns>Whether the quantity was changed or not.</returns>
        public Boolean ChangeItemQuantity(string sessionId, string basketItemId, Int32 quantity)
        {
            Boolean result = false;

            // Prepare request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_PREFIX + "/checkout/v3/baskets/" + basketItemId + "/" + quantity);
            request.Method = "PUT";

            // Handle request
            AuthUtils.HandleRequest(request, accessKeyId, secretAccessKey, sessionId, null, null);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                result = HttpStatusCode.OK == response.StatusCode;
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    throw ExceptionHandler.HandleBasicApiException(response);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (response != null) response.Close();
            }

            return result;
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
