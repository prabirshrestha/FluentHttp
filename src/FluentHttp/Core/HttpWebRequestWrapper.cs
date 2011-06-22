namespace FluentHttp
{
    using System;
    using System.IO;
    using System.Net;

    /// <summary>
    /// Represents the http web request wrapper.
    /// </summary>
#if FLUENTHTTP_CORE_INTERNAL
    internal
#else
    public
#endif
 class HttpWebRequestWrapper : IHttpWebRequest
    {
        /// <summary>
        /// The http web request.
        /// </summary>
        private readonly HttpWebRequest _httpWebRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWebRequestWrapper"/> class.
        /// </summary>
        /// <param name="httpWebRequest">
        /// The http web request.
        /// </param>
        public HttpWebRequestWrapper(HttpWebRequest httpWebRequest)
        {
            if (httpWebRequest == null)
            {
                throw new ArgumentNullException("httpWebRequest");
            }

            _httpWebRequest = httpWebRequest;
        }

        /// <summary>
        /// Gets or sets the http method.
        /// </summary>
        public string Method
        {
            get { return _httpWebRequest.Method; }
            set { _httpWebRequest.Method = value; }
        }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public string ContentType
        {
            get { return _httpWebRequest.ContentType; }
            set { _httpWebRequest.ContentType = value; }
        }

        /// <summary>
        /// Gets or sets the http headers.
        /// </summary>
        public WebHeaderCollection Headers
        {
            get { return _httpWebRequest.Headers; }
            set { _httpWebRequest.Headers = value; }
        }

#if !WINDOWS_PHONE
        /// <summary>
        /// Gets or sets the content length.
        /// </summary>
        public long ContentLength
        {
            get { return _httpWebRequest.ContentLength; }
            set { _httpWebRequest.ContentLength = value; }
        }
#endif

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        public string UserAgent
        {
            get { return _httpWebRequest.UserAgent; }
            set { _httpWebRequest.UserAgent = value; }
        }

        /// <summary>
        /// Gets or sets the cookie container.
        /// </summary>
        public CookieContainer CookieContainer
        {
            get { return _httpWebRequest.CookieContainer; }
            set { _httpWebRequest.CookieContainer = value; }
        }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        public ICredentials Credentials
        {
            get { return _httpWebRequest.Credentials; }
            set { _httpWebRequest.Credentials = value; }
        }

        /// <summary>
        /// Gets the request uri.
        /// </summary>
        public Uri RequestUri
        {
            get { return _httpWebRequest.RequestUri; }
        }

        /// <summary>
        /// Gets or sets the accept.
        /// </summary>
        public string Accept
        {
            get { return _httpWebRequest.Accept; }
            set { _httpWebRequest.Accept = value; }
        }

#if !SILVERLIGHT
        /// <summary>
        /// Gets or sets the proxy.
        /// </summary>
        public IWebProxy Proxy
        {
            get { return _httpWebRequest.Proxy; }
            set { _httpWebRequest.Proxy = value; }
        }

        /// <summary>
        /// Gets or sets the if modified since.
        /// </summary>
        public DateTime IfModifiedSince
        {
            get { return _httpWebRequest.IfModifiedSince; }
            set { _httpWebRequest.IfModifiedSince = value; }
        }

        /// <summary>
        /// Gets or sets the referer.
        /// </summary>
        public string Referer
        {
            get { return _httpWebRequest.Referer; }
            set { _httpWebRequest.Referer = value; }
        }

        /// <summary>
        /// Gets or sets the transfer encoding.
        /// </summary>
        public string TransferEncoding
        {
            get { return _httpWebRequest.TransferEncoding; }
            set { _httpWebRequest.TransferEncoding = value; }
        }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        public int Timeout
        {
            get { return _httpWebRequest.Timeout; }
            set { _httpWebRequest.Timeout = value; }
        }

        /// <summary>
        /// Gets or sets the decompression method.
        /// </summary>
        public DecompressionMethods AutomaticDecompression
        {
            get { return _httpWebRequest.AutomaticDecompression; }
            set { _httpWebRequest.AutomaticDecompression = value; }
        }

        /// <summary>
        /// Gets or sets the expect.
        /// </summary>
        public string Expect
        {
            get { return _httpWebRequest.Expect; }
            set { _httpWebRequest.Expect = value; }
        }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        public string Connection
        {
            get { return _httpWebRequest.Connection; }
            set { _httpWebRequest.Connection = value; }
        }
#endif

        /// <summary>
        /// Begins the get response.
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
        public IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            return _httpWebRequest.BeginGetResponse(callback, state);
        }

        /// <summary>
        /// Beggins getting the request stream.
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
        public IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            return _httpWebRequest.BeginGetRequestStream(callback, state);
        }

        /// <summary>
        /// Ends the http web get response.
        /// </summary>
        /// <param name="asyncResult">
        /// The async result.
        /// </param>
        /// <returns>
        /// The http web response.
        /// </returns>
        public IHttpWebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            var httpWebResponse = (HttpWebResponse)_httpWebRequest.EndGetResponse(asyncResult);

            return new HttpWebResponseWrapper(httpWebResponse);
        }

        /// <summary>
        /// Ends the get request stream.
        /// </summary>
        /// <param name="asyncResult">
        /// The async result.
        /// </param>
        /// <returns>
        /// The request stream.
        /// </returns>
        public Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return _httpWebRequest.EndGetRequestStream(asyncResult);
        }

#if !SILVERLIGHT

        public IHttpWebResponse GetResponse()
        {
            return new HttpWebResponseWrapper((HttpWebResponse)_httpWebRequest.GetResponse());
        }

        public Stream GetRequestStream()
        {
            return _httpWebRequest.GetRequestStream();
        }

#endif

    }
}