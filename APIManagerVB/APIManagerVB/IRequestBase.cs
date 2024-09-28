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
        public RequestMethod Method { get; set; }
        public string BaseURL { get; }
        public string Endpoint { get; }
        public List<NetLog> Logs { get; }
        public bool Executed { get; }
        public int AnswerTimeout { get; }
        public bool JsonAnswerProcessing { get; set; }
        #endregion
        #region HTTP API Data
        public Dictionary<string, string> Headers { get; }
        public AuthType Auth { get; }
        public BodyType BodyData { get; }
        public object? Body { get; }
        #endregion
        #region HTTP API Response
        public HttpStatusCode StatusCode { get; }
        public string ResponseText { get; }
        public JObject? ResponseJson { get; }
        #endregion
        #region HttpClient Data
        public HttpClient? Client { get; set; }
        public CancellationTokenSource Cts { get; set; }
        #endregion
    }
}
