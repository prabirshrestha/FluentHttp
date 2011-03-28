namespace FluentHttp
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Represents a Fluent Http Request.
    /// </summary>
    public class FluentHttpRequest
    {
        /// <summary>
        /// The buffer size.
        /// </summary>
        private const int BufferSize = 4096;

        /// <summary>
        /// The base url.
        /// </summary>
        private string _baseUrl;

        /// <summary>
        /// The resource path.
        /// </summary>
        private string _resourcePath;

        /// <summary>
        /// The http method.
        /// </summary>
        private string _httpMethod;

        /// <summary>
        /// The http web request factory.
        /// </summary>
        private Func<FluentHttpRequest, string, IHttpWebRequest> _httpWebRequestFactory;

        /// <summary>
        /// The fluent http headers.
        /// </summary>
        private FluentHttpHeaders _headers;

        /// <summary>
        /// The fluent query strings.
        /// </summary>
        private FluentQueryStrings _queryStrings;

        private FluentHttpRequestBody _body;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpRequest"/> class.
        /// </summary>
        public FluentHttpRequest()
        {
            Initialize();
        }

        /// <summary>
        /// Occurs when the response headers are received.
        /// </summary>
        public event EventHandler<ResponseHeadersReceivedEventArgs> ResponseHeadersReceived;

        /// <summary>
        /// Adds a forward slash if not present.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// Returns a string starting with /.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string AddStartingSlashInNotPresent(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "/";
            }

            // if not null or empty
            if (input[0] != '/')
            {
                // if doesn't start with / then add /
                return "/" + input;
            }
            else
            {
                // else return the original input.
                return input;
            }
        }

        /// <summary>
        /// Url decode the input string.
        /// </summary>
        /// <param name="input">
        /// The string to url decode.
        /// </param>
        /// <returns>
        /// The url decoded string.
        /// </returns>
        public static string UrlDecode(string input)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.UrlDecode(input);
#elif SILVERLIGHT
            return System.Windows.Browser.HttpUtility.UrlDecode(input);
#else
            return External.HttpUtility.UrlDecode(input);
#endif
        }

        /// <summary>
        /// Url encode the input string.
        /// </summary>
        /// <param name="input">
        /// The string to url encode.
        /// </param>
        /// <returns>
        /// The url encoded string.
        /// </returns>
        public static string UrlEncode(string input)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.UrlEncode(input);
#elif SILVERLIGHT
            return System.Windows.Browser.HttpUtility.UrlEncode(input);
#else
            return External.HttpUtility.UrlEncode(input);
#endif
        }

        /// <summary>
        /// Sets the base url.
        /// </summary>
        /// <param name="url">
        /// The base url.
        /// </param>
        /// <returns>
        /// The fluent http request.
        /// </returns>
        public FluentHttpRequest BaseUrl(string url)
        {
            _baseUrl = url;
            return this;
        }

        /// <summary>
        /// Gets the base url.
        /// </summary>
        /// <returns>
        /// The base url.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetBaseUrl()
        {
            return _baseUrl;
        }

        /// <summary>
        /// Sets the resource path.
        /// </summary>
        /// <param name="resourcePath">
        /// The resource path.
        /// </param>
        /// <returns>
        /// The fluent http request.
        /// </returns>
        public FluentHttpRequest ResourcePath(string resourcePath)
        {
            _resourcePath = string.IsNullOrEmpty(resourcePath)
                                ? resourcePath
                                : AddStartingSlashInNotPresent(resourcePath);
            return this;
        }

        /// <summary>
        /// Gets the resource path.
        /// </summary>
        /// <returns>
        /// The resource path.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetResourcePath()
        {
            return _resourcePath ?? string.Empty;
        }

        /// <summary>
        /// Sets the http method.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <returns>
        /// The fluent http request.
        /// </returns>
        public FluentHttpRequest Method(string method)
        {
            _httpMethod = method;
            return this;
        }

        /// <summary>
        /// Gets the http method.
        /// </summary>
        /// <returns>
        /// The http method.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetMethod()
        {
            return _httpMethod;
        }

        /// <summary>
        /// Sets the http web request factory.
        /// </summary>
        /// <param name="httpWebRequestFactory">
        /// The http web request factory.
        /// </param>
        /// <returns>
        /// The fluent http request.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public FluentHttpRequest HttpWebRequestFactory(Func<FluentHttpRequest, string, IHttpWebRequest> httpWebRequestFactory)
        {
            _httpWebRequestFactory = httpWebRequestFactory;
            return this;
        }

        /// <summary>
        /// Gets the http web request factory.
        /// </summary>
        /// <returns>
        /// Func method for creating HttpWebRequest.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Func<FluentHttpRequest, string, IHttpWebRequest> GetHttpWebRequestFactory()
        {
            return _httpWebRequestFactory;
        }

        /// <summary>
        /// Access http headers.
        /// </summary>
        /// <param name="headers">
        /// The headers.
        /// </param>
        /// <returns>
        /// The fluent http request.
        /// </returns>
        public FluentHttpRequest Headers(Action<FluentHttpHeaders> headers)
        {
            if (headers != null)
            {
                headers(_headers);
            }

            return this;
        }

        /// <summary>
        /// Gets the http headers.
        /// </summary>
        /// <returns>
        /// The http headers.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentHttpHeaders GetHeaders()
        {
            return _headers;
        }

        /// <summary>
        /// Access query strings.
        /// </summary>
        /// <param name="queryStrings">
        /// The query strings.
        /// </param>
        /// <returns>
        /// The fluent http request.
        /// </returns>
        public FluentHttpRequest QueryStrings(Action<FluentQueryStrings> queryStrings)
        {
            if (queryStrings != null)
            {
                queryStrings(_queryStrings);
            }

            return this;
        }

        /// <summary>
        /// Gets the query strings.
        /// </summary>
        /// <returns>
        /// The query strings.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentQueryStrings GetQueryStrings()
        {
            return _queryStrings;
        }

        /// <summary>
        /// Occurs when http response headers are received.
        /// </summary>
        /// <param name="onResponseHeadersReceived">
        /// On response headers received.
        /// </param>
        /// <returns>
        /// The fluent http request.
        /// </returns>
        public FluentHttpRequest OnResponseHeadersReceived(EventHandler<ResponseHeadersReceivedEventArgs> onResponseHeadersReceived)
        {
            if (onResponseHeadersReceived != null)
            {
                ResponseHeadersReceived += onResponseHeadersReceived;
            }

            return this;
        }

        /// <summary>
        /// Access the request body.
        /// </summary>
        /// <param name="body">
        /// The request body.
        /// </param>
        /// <returns>
        /// Returns the Fluent http request.
        /// </returns>
        public FluentHttpRequest Body(Action<FluentHttpRequestBody> body)
        {
            if (body != null)
            {
                body(_body);
            }

            return this;
        }

        /// <summary>
        /// Gets the request body.
        /// </summary>
        /// <returns>
        /// The resquest body.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentHttpRequestBody GetBody()
        {
            return _body;
        }

        /// <summary>
        /// Starts the http request.
        /// </summary>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The async result.
        /// </returns>
        public IAsyncResult BeginExecute(AsyncCallback callback, object state)
        {
            AuthenticateIfRequried();

            var requestUrl = BuildRequestUrl();

            var httpWebHelper = new HttpWebHelper();
            // todo add cookies
            var httpWebRequest = httpWebHelper.CreateHttpWebRequest(requestUrl, GetMethod(), GetHeaders().GetHeaderCollection(), null);
            PrepareHttpWebRequest(httpWebRequest);

            var asyncResult = new FluentHttpAsyncResult(this);

            var enumerableAsync = httpWebHelper.ExecuteAsync(
                httpWebRequest,
                GetBody().Stream,
                responseHeadersReceived =>
                {
                    asyncResult.Response = new FluentHttpResponse(asyncResult.Request, responseHeadersReceived.Response);
                    var args = new ResponseHeadersReceivedEventArgs(asyncResult.Response);
                    OnResponseHeadersRecived(args);
                    responseHeadersReceived.ResponseSaveStream = args.ResponseSaveStream;
                });

            Prabir.Async.Async.Run(
                enumerableAsync.GetEnumerator(),
                ex =>
                {
                    asyncResult.Exception = ex;
                    asyncResult.SetAsyncWaitHandle();
                });

            return asyncResult;
        }

        /// <summary>
        /// Ends the http request.
        /// </summary>
        /// <param name="asyncResult">
        /// The async result.
        /// </param>
        /// <returns>
        /// Returns the <see cref="FluentHttpResponse"/>.
        /// </returns>
        public FluentHttpResponse EndExecute(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }

            var ar = asyncResult as FluentHttpAsyncResult;
            if (ar == null)
            {
                throw new ArgumentException("asyncResult");
            }

            // wait for the request to end
            ar.AsyncWaitHandle.WaitOne();

            // propagate the exception to the one who calls EndRequest.
            if (ar.Exception != null)
            {
                throw ar.Exception;
            }

            return ar.Response;
        }

        /// <summary>
        /// Notify response headers received.
        /// </summary>
        /// <param name="e">
        /// The event args.
        /// </param>
        protected void OnResponseHeadersRecived(ResponseHeadersReceivedEventArgs e)
        {
            if (ResponseHeadersReceived != null)
            {
                ResponseHeadersReceived(this, e);
            }
        }

        /// <summary>
        /// Initializes Fluent Http Request.
        /// </summary>
        private void Initialize()
        {
            _headers = new FluentHttpHeaders();
            _queryStrings = new FluentQueryStrings();
            _body = new FluentHttpRequestBody();
        }

        /// <summary>
        /// Authenticates the <see cref="FluentHttpRequest"/> if it has an authenticator.
        /// </summary>
        private void AuthenticateIfRequried()
        {
            //var authenticator = this.GetAuthenticator();
            //if (authenticator != null)
            //{
            //    authenticator.Authenticate(this);
            //}
        }

        /// <summary>
        /// Builds the request url.
        /// </summary>
        /// <returns>
        /// The request url.
        /// </returns>
        private string BuildRequestUrl()
        {
            var sb = new StringBuilder();

            var baseUrl = GetBaseUrl();

            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException("baseUrl");
            }

            sb.Append(GetBaseUrl());
            sb.Append(GetResourcePath());
            sb.Append("?");

            foreach (var qs in GetQueryStrings().GetQueryStringCollection())
            {
                // these querystrings are already url encoded.
                sb.AppendFormat("{0}={1}&", qs.Name, qs.Value);
            }

            // remove the last & or ?
            --sb.Length;

            return sb.ToString();
        }

        private void PrepareHttpWebRequest(IHttpWebRequest httpWebRequest)
        {
            // todo: set additional stuffs in web request.
        }
    }
}