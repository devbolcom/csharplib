using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Bol.OpenAPI.Utils
{
    public static class AuthUtils
    {
        private const string HEADER_CONTENT_MD5 = "Content-MD5";
        private const string HEADER_CONTENT_TYPE = "Content-Type";
        private const string HEADER_DATE = "Date";
        private const string HEADER_OAI = "X-OpenAPI";
        private const string HEADER_OAI_AUTH = "X-OpenAPI-Authorization";
        private const string HEADER_OAI_DATE = "X-OpenAPI-Date";
        private const string HEADER_OAI_SESSION_ID = "X-OpenAPI-Session-ID";
        private static MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
        private static UTF8Encoding encoding = new System.Text.UTF8Encoding();

        /// <summary>
        /// Handles the request, by adding the required headers for an OpenAPI-RS call.
        /// </summary>
        /// <param name="request">The HTTP web request.</param>
        /// <param name="accessKeyId">The access key ID.</param>
        /// <param name="secretAccessKey">The secret access key.</param>
        public static void HandleRequest(HttpWebRequest request, string accessKeyId, string secretAccessKey)
        {
            HandleRequest(request, accessKeyId, secretAccessKey, null, null, null);
        }

        /// <summary>
        /// Handles the request, by adding the required headers for an OpenAPI-RS call.
        /// </summary>
        /// <param name="request">The HTTP web request.</param>
        /// <param name="accessKeyId">The access key ID.</param>
        /// <param name="secretAccessKey">The secret access key.</param>
        /// <param name="sessionId">The session id.</param>
        public static void HandleRequest(HttpWebRequest request, string accessKeyId, string secretAccessKey, string sessionId, NameValueCollection parameters)
        {
            HandleRequest(request, accessKeyId, secretAccessKey, sessionId, null, parameters);
        }

        /// <summary>
        /// Handles the request, by adding the required headers for an OpenAPI-RS call.
        /// </summary>
        /// <param name="request">The HTTP web request.</param>
        /// <param name="accessKeyId">The access key ID.</param>
        /// <param name="secretAccessKey">The secret access key.</param>
        /// <param name="sessionId">The session id.</param>
        /// <param name="body">The request body to hash.</param>
        /// <param name="parameters">The HTTP post/query params.</param>
        public static void HandleRequest(HttpWebRequest request, string accessKeyId, string secretAccessKey, string sessionId, string body, NameValueCollection parameters)
        {
            // Session-ID (optional)
            if (sessionId != null)
            {
                request.Headers[HEADER_OAI_SESSION_ID] = sessionId;
            }

            // Content-Type
            if (request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }

            // Accept
            request.Accept = "application/xml";

            // Content-MD5 (optional)
            if (body != null)
            {
                byte[] originalBytes = encoding.GetBytes(body);
                byte[] encodedBytes = cryptoServiceProvider.ComputeHash(originalBytes);
                request.Headers[HEADER_CONTENT_MD5] = BitConverter.ToString(encodedBytes);
            }

            // Date
            DateTime dateTime = DateTime.Now;
            request.Date = dateTime;

            // Authorization
            string stringToSign = AuthUtils.CreateStringToSign(request, parameters);
            request.Headers[HEADER_OAI_AUTH] = accessKeyId + ":" + AuthUtils.CalculateHMAC256(stringToSign, secretAccessKey);
        }

        /// <summary>
        /// Creates the string to needs to be signed based on the required data embedded within the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP web request.</param>
        /// <returns>The string that needs to be signed for the current request.</returns>
        public static string CreateStringToSign(HttpWebRequest request, NameValueCollection parameters)
        {
            StringBuilder sb = new StringBuilder(256);

            // HTTP method
            if (request.Method != null)
            {
                sb.Append(request.Method);
            }
            sb.Append("\n");

            // Content-MD5
            if (request.Headers[HEADER_CONTENT_MD5] != null)
            {
                sb.Append(request.Headers[HEADER_CONTENT_MD5]);
            }
            sb.Append("\n");

            // Content-Type
            if (request.Headers[HEADER_CONTENT_TYPE] != null)
            {
                sb.Append(request.Headers[HEADER_CONTENT_TYPE]);
            }
            sb.Append("\n");

            // Date
            if (request.Headers[HEADER_OAI_DATE] != null)
            {
                sb.Append(request.Headers[HEADER_OAI_DATE]);
            }
            else if (request.Headers[HEADER_DATE] != null)
            {
                sb.Append(request.Headers[HEADER_DATE]);
            }
            sb.Append("\n");

            // Canonicalized OpenAPI headers
            SortedDictionary<string, string> sortedHeaders = new SortedDictionary<string, string>();
            WebHeaderCollection headerNames = request.Headers;

            // Sort the OpenAPI headers alphabetically, in lower case
            foreach (string headerName in headerNames)
            {
                if (headerName.StartsWith(HEADER_OAI, StringComparison.OrdinalIgnoreCase)
                    && !headerName.Equals(HEADER_OAI_AUTH, StringComparison.OrdinalIgnoreCase))
                {
                    sortedHeaders.Add(headerName, request.Headers[headerName]);
                }
            }

            // Add the sorted OpenAPI headers with their value
            foreach (KeyValuePair<string, string> keyValuePair in sortedHeaders)
            {
                sb.Append(keyValuePair.Key.ToLower());
                sb.Append(":");

                if (keyValuePair.Value != null)
                {
                    sb.Append(keyValuePair.Value);
                }

                sb.Append("\n");
            }

            // Canonicalized resource (exclude query parameters)
            if (request.RequestUri != null)
            {
                sb.Append(request.RequestUri.AbsolutePath);
            }
            else
            {
                sb.Append('/');
            }

            // Sort the HTTP post/query parameters alphabetically
            SortedDictionary<string, string> sortedParams = new SortedDictionary<string, string>();
            if (parameters != null && parameters.Count > 0)
            {
                foreach (string key in parameters)
                {
                    string[] values = parameters.GetValues(key);
                    if (values != null)
                    {
                        sortedParams.Add(key, values[0]);
                    }
                }
            }
            else
            {
                sb.Append("\n");
            }

            // Add the sorted parameters with their value
            foreach (KeyValuePair<string, string> keyValuePair in sortedParams)
            {
                sb.Append("\n");
                sb.Append("&" + keyValuePair.Key);
                sb.Append("=");

                if (keyValuePair.Value != null)
                {
                    sb.Append(keyValuePair.Value);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Calculates the HMAC256 string based on the given string and secret access key.
        /// </summary>
        /// <param name="stringToSign">The string to sign.</param>
        /// <param name="secretAccessKey">The secret access key to sign the string with.</param>
        /// <returns>The calculated HMAC256 string.</returns>
        public static string CalculateHMAC256(string stringToSign, string secretAccessKey)
        {
            HMACSHA256 hmac = new HMACSHA256(encoding.GetBytes(secretAccessKey));
            byte[] hash = hmac.ComputeHash(encoding.GetBytes(stringToSign));

            return Convert.ToBase64String(hash);
        }
    }
}
