using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APIManagerVB.Request
{
    public class Asynchronous
    {
        #region Basics of API Requests
        /// <summary>
        /// HTTP Method (GET, POST, PUT, PATCH, DELETE)
        /// </summary>
        public RequestMethod Method { get; set; }
        /// <summary>
        /// API Base URL
        /// </summary>
        public string BaseURL { get; private set; } = string.Empty;
        /// <summary>
        /// API Endpoint (End of URL)
        /// </summary>
        public string Endpoint { get; set; } = string.Empty;

        /// <summary>
        /// API Request Log
        /// </summary>
        public List<NetLog> Logs { get; private set; } = new List<NetLog>();

        /// <summary>
        /// Executed API Request?
        /// </summary>
        public bool Executed { get; private set; } = false;

        /// <summary>
        /// API Request Timeout in seconds
        /// </summary>
        public int AnswerTimeout { get; set; }

        /// <summary>
        /// Request is processing JSON data if it is true
        /// </summary>
        public bool JsonAnswerProcessing { get; set; } = true;
        #endregion

        #region HTTP API Data
        /// <summary>
        /// API Headers
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Neccessary API Authorization Type (NONE, OTHER, BEARER)
        /// </summary>
        public AuthType Auth { get; set; }

        /// <summary>
        /// API Body Data Type (NONE, OTHER, JSON, URLENCODED)
        /// </summary>
        public BodyType BodyData { get; set; }

        /// <summary>
        /// API Body Data
        /// </summary>
        public object? Body { get; set; }

        #endregion

        #region HTTP API Response
        /// <summary>
        /// HTTP Response Status Code
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// HTTP Response Data in Text
        /// </summary>
        public string ResponseText { get; private set; } = string.Empty;

        /// <summary>
        /// HTTP Response Data in JSON if parsable
        /// </summary>
        public JObject ResponseJson { get; private set; } = new JObject();
        #endregion

        #region HttpClient Data
        /// <summary>
        /// HttpClient for API Request
        /// </summary>
        public HttpClient? Client { get; set; }

        /// <summary>
        /// CancellationTokenSource for cancelling request
        /// </summary>
        public CancellationTokenSource Cts { get; set; }

        /// <summary>
        /// Asyncronous Request CancellationToken for cancelling request
        /// </summary>
        private CancellationToken CtsToken { get; set; }
        #endregion

        #region Constructors
        public Asynchronous(RequestMethod method, string baseURL)
        {
            Method = method;
            BaseURL = baseURL;
            Logs.Add(new NetLog(LogType.INFO, "Asyncronous request created", $"Method: {Method.ToString()}"));

            //Create CancellationTokenSource and CancellationToken
            Cts = new CancellationTokenSource();
            CtsToken = Cts.Token;
        }

        public Asynchronous(RequestMethod method, string baseURL, string endpoint) : this(method, baseURL)
        {
            Endpoint = endpoint;

            //Check if baseURL ends with a slash
            if (BaseURL.EndsWith('/') && endpoint.StartsWith('/'))
            {
                Logs.Add(new NetLog(LogType.WARNING, "BaseURL ends with a slash and endpoint starts with a slash", "Removing one of them to avoid double slashes"));
            }
        }

        public Asynchronous(RequestMethod method, string baseURL, string endpoint, AuthType auth) : this(method, baseURL, endpoint)
        {
            Auth = auth;
        }

        public Asynchronous(RequestMethod method, string baseURL, string endpoint, AuthType auth, string bearer) : this(method, baseURL, endpoint)
        {
            Auth = auth;
            //Add Bearer token to headers if AuthType is Bearer
            if (auth == AuthType.BEARER)
            {
                Headers.Add("Authorization", $"Bearer {bearer}");
            }
            else
            {
                //If AuthType is not Bearer, but Bearer token is exist, log a warning
                if (!string.IsNullOrEmpty(bearer))
                {
                    Logs.Add(new NetLog(LogType.WARNING, "Bearer token", $"Bearer token is not needed for this AuthType: {Auth.ToString()}"));
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executing API Request
        /// </summary>
        public async Task Execute()
        {
            Logs.Add(new NetLog(LogType.INFO, "Asyncronous request executing started", $"URL: {BaseURL}{Endpoint}"));
            try
            {
                using (Client = new HttpClient())
                {
                    //Set Security Protocol
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    //Set Client Timeout
                    Client.Timeout = TimeSpan.FromSeconds(AnswerTimeout);

                    //Accept JSON Media Type if JSON proccessing is true
                    if (JsonAnswerProcessing)
                    {
                        Headers.Add("Accept", "application/json");
                        Logs.Add(new NetLog(LogType.INFO, "Accept set: application/json"));
                    }

                    //Add Headers to Client
                    foreach (KeyValuePair<string, string> header in Headers)
                    {
                        Client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                    //Body Data Processing
                    HttpContent content = new StringContent(string.Empty);
                    switch (BodyData)
                    {
                        case BodyType.NONE:
                            break;
                        case BodyType.OTHER:
                            //Parsing StringContent
                            content = new StringContent(Body?.ToString() ?? string.Empty);
                            Logs.Add(new NetLog(LogType.INFO, "BodyData is OTHER", $"Body: {Body?.ToString() ?? "NULL"}"));
                            break;
                        case BodyType.JSON:
                            Body = JObject.FromObject(Body ?? "{}");
                            content = new StringContent(Body.ToString() ?? "{}", Encoding.UTF8, "application/json");
                            Logs.Add(new NetLog(LogType.INFO, "BodyData JSON parsed!"));
                            break;
                        case BodyType.URLENCODED:
                            //If BodyData is URLENCODED, convert Body to URL Encoded
                            if (Body is Dictionary<string, string> body)
                            {
                                Body = new FormUrlEncodedContent(body);
                                content = (FormUrlEncodedContent)Body;
                                Logs.Add(new NetLog(LogType.INFO, "BodyData FormURLEncoded parsed!"));
                            }
                            else
                            {
                                Logs.Add(new NetLog(LogType.ERROR, "BodyData is URLENCODED but Body is not a Dictionary<string, string>", $"Body: {Body?.ToString() ?? "NULL"}"));
                                //Parsing StringContent to avoid null reference exception
                                content = new StringContent(Body?.ToString() ?? string.Empty);
                            }
                            break;
                        default:
                            content = new StringContent(string.Empty);
                            break;
                    }

                    //Execute Request
                    HttpResponseMessage response;
                    switch (Method)
                    {
                        case RequestMethod.GET:
                            response = await Client.GetAsync($"{BaseURL}{Endpoint}", CtsToken);
                            break;
                        case RequestMethod.POST:
                            response = await Client.PostAsync($"{BaseURL}{Endpoint}", content, CtsToken);
                            break;
                        case RequestMethod.PUT:
                            response = await Client.PutAsync($"{BaseURL}{Endpoint}", content, CtsToken);
                            break;
                        case RequestMethod.PATCH:
                            response = await Client.PatchAsync($"{BaseURL}{Endpoint}", content, CtsToken);
                            break;
                        case RequestMethod.DELETE:
                            response = await Client.DeleteAsync($"{BaseURL}{Endpoint}", CtsToken);
                            break;
                        default:
                            Logs.Add(new NetLog(LogType.ERROR, "HTTP Method is not valid", $"Method: {Method.ToString()}"));
                            response = await Client.GetAsync($"{BaseURL}{Endpoint}", CtsToken);
                            break;
                    }

                    Logs.Add(new NetLog(LogType.INFO, "Request executed!"));

                    //Set Response Status Code
                    StatusCode = response.StatusCode;
                    Logs.Add(new NetLog(LogType.INFO, $"HTTP Status: {response.StatusCode}"));

                    //Set Response Text
                    ResponseText = await response.Content.ReadAsStringAsync();
                    //Check if ResponseText is empty
                    if (string.IsNullOrEmpty(ResponseText))
                    {
                        Logs.Add(new NetLog(LogType.WARNING, "Response Text is empty", "Response Text is empty"));
                    }
                    else if (JsonAnswerProcessing)
                    {
                        //Response Text to JSON
                        try
                        {
                            ResponseJson = JObject.Parse((ResponseText == string.Empty ? "{}" : ResponseText));
                            Logs.Add(new NetLog(LogType.INFO, "Response parsed to JSON"));
                        }
                        catch (Exception ex)
                        {
                            Logs.Add(new NetLog(LogType.ERROR, "Response could not be parsed to JSON! (RFC 8259)", ex.Message));
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Logs.Add(new NetLog(LogType.ERROR, "Cancelled API Request", $"Timeout: {AnswerTimeout} second(s)"));
            }
            catch (Exception ex)
            {
                Logs.Add(new NetLog(LogType.ERROR, ex.Message, $"{(ex.StackTrace?.ToString() ?? "No StackTrace")}\t{ex.Source}"));
            }
            finally
            {
                Executed = true;
            }
        }
        #endregion
    }
}
