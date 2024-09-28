using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APIManagerVB
{
    public interface IRequestBase
    {
        #region Basics of API Requests
        /// <summary>
        /// HTTP Method (GET, POST, PUT, PATCH, DELETE)
        /// </summary>
        public HttpMethod Method { get; set; }
        /// <summary>
        /// API Base URL
        /// </summary>
        public string BaseURL { get; }
        /// <summary>
        /// API Endpoint (End of URL)
        /// </summary>
        public string Endpoint { get; }
        
        /// <summary>
        /// API Request Log
        /// </summary>
        public List<NetLog> Logs { get; }
        
        /// <summary>
        /// Executed API Request?
        /// </summary>
        public bool Executed { get; }

        /// <summary>
        /// API Request Timeout in seconds
        /// </summary>
        public int AnswerTimeout { get; }

        /// <summary>
        /// Request is not processing JSON data if it is true
        /// </summary>
        public bool JSONRequestNotProcessing { get; set; }
        #endregion

        #region HTTP API Data
        /// <summary>
        /// API Headers
        /// </summary>
        public Dictionary<string, string> Headers { get; }

        /// <summary>
        /// Neccessary API Authorization Type (NONE, OTHER, BEARER)
        /// </summary>
        public AuthType Auth { get; }

        /// <summary>
        /// API Body Data Type (NONE, OTHER, JSON, URLENCODED)
        /// </summary>
        public BodyType BodyData { get; }

        /// <summary>
        /// API Body Data
        /// </summary>
        public object? Body { get; }

        #endregion

        #region HTTP API Response
        /// <summary>
        /// HTTP Response Status Code
        /// </summary>
        public HttpStatusCode StatusCode { get; }
       
        /// <summary>
        /// HTTP Response Data in Text
        /// </summary>
        public string ResponseText { get; }

        /// <summary>
        /// HTTP Response Data in JSON if parsable
        /// </summary>
        public JObject? ResponseJson { get; }
        #endregion

        #region HttpClient Data
        public HttpClient? Client { get; set; }
        #endregion
    }
}
