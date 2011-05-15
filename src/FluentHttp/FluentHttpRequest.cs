
namespace FluentHttp
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Represents a Fluent Http Request.
    /// </summary>
    public class FluentHttpRequest
    {
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

        /// <summary>
        /// The fluent http request body.
        /// </summary>
        private FluentHttpRequestBody _body;

        /// <summary>
        /// The fluent authenticator.
        /// </summary>
        private IFluentAuthenticator _authenticator;

#if !SILVERLIGHT

        /// <summary>
        /// The proxy.
        /// </summary>
        private IWebProxy _proxy;

        /// <summary>
        /// The credentials
        /// </summary>
        private ICredentials _credentials;

#endif

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
        /// Converts stream to string.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// The string.
        /// </returns>
        public static string ToString(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
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
        public virtual string GetBaseUrl()
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
        public virtual string GetResourcePath()
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
        public virtual string GetMethod()
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
        public virtual Func<FluentHttpRequest, string, IHttpWebRequest> GetHttpWebRequestFactory()
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
        public virtual FluentHttpHeaders GetHeaders()
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
        public virtual FluentQueryStrings GetQueryStrings()
        {
            return _queryStrings;
        }

        /// <summary>
        /// Sets the authenticator.
        /// </summary>
        /// <param name="authenticator">
        /// The authenticator.
        /// </param>
        /// <returns>
        /// The fluent http request.
        /// </returns>
        public FluentHttpRequest AuthenticateUsing(IFluentAuthenticator authenticator)
        {
            _authenticator = authenticator;
            return this;
        }

        /// <summary>
        /// Sets the authenticator.
        /// </summary>
        /// <param name="authenticator">
        /// The authenticator.
        /// </param>
        /// <returns>
        /// The fluent http request.
        /// </returns>
        public FluentHttpRequest AuthenticateUsing(Func<IFluentAuthenticator> authenticator)
        {
            return authenticator != null ? AuthenticateUsing(authenticator()) : this;
        }

        /// <summary>
        /// Gets the authenticator.
        /// </summary>
        /// <returns>
        /// The authenticator.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual IFluentAuthenticator GetAuthenticator()
        {
            return _authenticator;
        }

#if !SILVERLIGHT

        /// <summary>
        /// Sets the proxy.
        /// </summary>
        /// <param name="proxy">
        /// The proxy.
        /// </param>
        /// <returns>
        /// The fluent http request.
        /// </returns>
        public FluentHttpRequest Proxy(IWebProxy proxy)
        {
            _proxy = proxy;
            return this;
        }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <returns>
        /// The proxy.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual IWebProxy GetProxy()
        {
            return _proxy;
        }

        /// <summary>
        /// Sets the credentails.
        /// </summary>
        /// <param name="credentials">
        /// The credentails.
        /// </param>
        /// <returns>
        /// The fluent http request.
        /// </returns>
        public FluentHttpRequest Credentials(ICredentials credentials)
        {
            _credentials = credentials;
            return this;
        }

        /// <summary>
        /// Gets the credentials.
        /// </summary>
        /// <returns>
        /// The credentials.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual ICredentials GetCredentials()
        {
            return _credentials;
        }

#endif

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
        public virtual FluentHttpRequestBody GetBody()
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
        public virtual IAsyncResult BeginExecute(AsyncCallback callback, object state)
        {
            // todo execute callback
            AuthenticateIfRequried();

            var requestUrl = BuildRequestUrl();

            var httpWebHelper = new HttpWebHelper();

            // todo add cookies
            var httpWebRequest = httpWebHelper.CreateHttpWebRequest(requestUrl, GetMethod(), (System.Collections.Generic.IEnumerable<Pair<string,string>>) GetHeaders().GetHeaderCollection(), null);
            PrepareHttpWebRequest(httpWebRequest);

            var asyncResult = new FluentHttpAsyncResult(this, callback, state);

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

            AsyncEnumerator.Async.Run(
                enumerableAsync.GetEnumerator(),
                ex =>
                {
                    if (asyncResult.Response != null)
                    {
                        asyncResult.Response.ResponseStatus = ResponseStatus.Error;
                    }

                    asyncResult.Exception = ex;
                    asyncResult.IsCompleted = true;
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
        public virtual FluentHttpResponse EndExecute(IAsyncResult asyncResult)
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
            ar.IsCompleted = true;

            // propagate the exception to the one who calls EndRequest.
            if (ar.Exception != null)
            {
                throw ar.Exception;
            }

            ar.Response.ResponseStatus = ResponseStatus.Completed;

            return ar.Response;
        }

#if TPL

        /// <summary>
        /// Converts the <see cref="FluentHttpRequest"/> to Task.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="taskCreationOptions">
        /// The task creation options.
        /// </param>
        /// <returns>
        /// Returns the task of <see cref="FluentHttpResponse"/>.
        /// </returns>
        public System.Threading.Tasks.Task<FluentHttpResponse> ToTask(object state, System.Threading.Tasks.TaskCreationOptions taskCreationOptions)
        {
            return System.Threading.Tasks.Task.Factory.FromAsync<FluentHttpResponse>(BeginExecute, EndExecute, state, taskCreationOptions);
        }

        /// <summary>
        /// Converts the <see cref="FluentHttpRequest"/> to Task.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// Returns the task of <see cref="FluentHttpResponse"/>.
        /// </returns>
        public System.Threading.Tasks.Task<FluentHttpResponse> ToTask(object state)
        {
            return this.ToTask(state, System.Threading.Tasks.TaskCreationOptions.None);
        }

        /// <summary>
        /// Converts the <see cref="FluentHttpRequest"/> to Task.
        /// </summary>
        /// <returns>
        /// Returns the task of <see cref="FluentHttpResponse"/>.
        /// </returns>
        public System.Threading.Tasks.Task<FluentHttpResponse> ToTask()
        {
            return this.ToTask(null);
        }

#endif

        /// <summary>
        /// Builds the request url.
        /// </summary>
        /// <returns>
        /// The request url.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string BuildRequestUrl()
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
            var authenticator = GetAuthenticator();
            if (authenticator != null)
            {
                authenticator.Authenticate(this);
            }
        }

        /// <summary>
        /// Prepares http web request.
        /// </summary>
        /// <param name="httpWebRequest">
        /// The http web request.
        /// </param>
        private void PrepareHttpWebRequest(IHttpWebRequest httpWebRequest)
        {
#if !SILVERLIGHT

            var proxy = GetProxy();
            if (proxy != null)
            {
                httpWebRequest.Proxy = proxy;
            }

            var credentials = GetCredentials();
            if (credentials != null)
            {
                httpWebRequest.Credentials = credentials;
            }
#endif
        }
    }
}