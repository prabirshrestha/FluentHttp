namespace FluentHttp
{
    using System;
    using System.ComponentModel;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpRequest"/> class.
        /// </summary>
        public FluentHttpRequest()
        {
            Initialize();
        }

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes Fluent Http Request.
        /// </summary>
        private void Initialize()
        {
            _headers = new FluentHttpHeaders();
            _queryStrings = new FluentQueryStrings();
        }
    }
}