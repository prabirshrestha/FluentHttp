namespace FluentHttp
{
    using System;
    using System.IO;
    using System.Net;

    /// <summary>
    /// Represents the http web request wrapper.
    /// </summary>
    public class HttpWebRequestWrapper : IHttpWebRequest
    {
        /// <summary>
        /// The http web request.
        /// </summary>
        private readonly HttpWebRequest httpWebRequest;

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

            this.httpWebRequest = httpWebRequest;
        }

        /// <summary>
        /// Gets or sets the http method.
        /// </summary>
        public string Method
        {
            get { return this.httpWebRequest.Method; }
            set { this.httpWebRequest.Method = value; }
        }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public string ContentType
        {
            get { return this.httpWebRequest.ContentType; }
            set { this.httpWebRequest.ContentType = value; }
        }

        /// <summary>
        /// Gets or sets the http headers.
        /// </summary>
        public WebHeaderCollection Headers
        {
            get { return this.httpWebRequest.Headers; }
            set { this.httpWebRequest.Headers = value; }
        }

        /// <summary>
        /// Gets or sets the content length.
        /// </summary>
        public long ContentLength
        {
            get { return this.httpWebRequest.ContentLength; }
            set { this.httpWebRequest.ContentLength = value; }
        }

        /// <summary>
        /// Gets or sets the if modified since.
        /// </summary>
        public DateTime IfModifiedSince
        {
            get { return this.httpWebRequest.IfModifiedSince; }
            set { this.httpWebRequest.IfModifiedSince = value; }
        }

        /// <summary>
        /// Gets or sets the referer.
        /// </summary>
        public string Referer
        {
            get { return this.httpWebRequest.Referer; }
            set { this.httpWebRequest.Referer = value; }
        }

        /// <summary>
        /// Gets or sets the transfer encoding.
        /// </summary>
        public string TransferEncoding
        {
            get { return this.httpWebRequest.TransferEncoding; }
            set { this.httpWebRequest.TransferEncoding = value; }
        }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        public string UserAgent
        {
            get { return this.httpWebRequest.UserAgent; }
            set { this.httpWebRequest.UserAgent = value; }
        }

        /// <summary>
        /// Gets or sets the cookie container.
        /// </summary>
        public CookieContainer CookieContainer
        {
            get { return this.httpWebRequest.CookieContainer; }
            set { this.httpWebRequest.CookieContainer = value; }
        }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        public int Timeout
        {
            get { return this.httpWebRequest.Timeout; }
            set { this.httpWebRequest.Timeout = value; }
        }

        /// <summary>
        /// Gets or sets the proxy.
        /// </summary>
        public IWebProxy Proxy
        {
            get { return this.httpWebRequest.Proxy; }
            set { this.httpWebRequest.Proxy = value; }
        }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        public ICredentials Credentials
        {
            get { return this.httpWebRequest.Credentials; }
            set { this.httpWebRequest.Credentials = value; }
        }

        /// <summary>
        /// Gets or sets the decompression method.
        /// </summary>
        public DecompressionMethods AutomaticDecompression
        {
            get { return this.httpWebRequest.AutomaticDecompression; }
            set { this.httpWebRequest.AutomaticDecompression = value; }
        }

        /// <summary>
        /// Gets the request uri.
        /// </summary>
        public Uri RequestUri
        {
            get { return this.httpWebRequest.RequestUri; }
        }

        /// <summary>
        /// Gets or sets the expect.
        /// </summary>
        public string Expect
        {
            get { return this.httpWebRequest.Expect; }
            set { this.httpWebRequest.Expect = value; }
        }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        public string Connection
        {
            get { return this.httpWebRequest.Connection; }
            set { this.httpWebRequest.Connection = value; }
        }

        /// <summary>
        /// Gets or sets the accept.
        /// </summary>
        public string Accept
        {
            get { return this.httpWebRequest.Accept; }
            set { this.httpWebRequest.Accept = value; }
        }

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
            return this.httpWebRequest.BeginGetResponse(callback, state);
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
            return this.httpWebRequest.BeginGetRequestStream(callback, state);
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
            var httpWebResponse = (HttpWebResponse)this.httpWebRequest.EndGetResponse(asyncResult);

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
            return this.httpWebRequest.EndGetRequestStream(asyncResult);
        }
    }
}