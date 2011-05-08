namespace FluentHttp
{
    using System;
    using System.IO;
    using System.Net;

    /// <summary>
    /// Represents the http web response wrapper.
    /// </summary>
#if FLUENTHTTP_CORE_INTERNAL
    internal
#else
    public
#endif
 class HttpWebResponseWrapper : IHttpWebResponse
    {
        /// <summary>
        /// The http web response.
        /// </summary>
        private readonly HttpWebResponse httpWebResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWebResponseWrapper"/> class.
        /// </summary>
        /// <param name="httpWebResponse">
        /// The http web response.
        /// </param>
        public HttpWebResponseWrapper(HttpWebResponse httpWebResponse)
        {
            if (httpWebResponse == null)
            {
                throw new ArgumentNullException("httpWebResponse");
            }

            this.httpWebResponse = httpWebResponse;
        }

        /// <summary>
        /// Gets the http method.
        /// </summary>
        public string Method
        {
            get { return this.httpWebResponse.Method; }
        }

        /// <summary>
        /// Gets the content length.
        /// </summary>
        public long ContentLength
        {
            get { return this.httpWebResponse.ContentLength; }
        }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        public string ContentType
        {
            get { return this.httpWebResponse.ContentType; }
        }

        /// <summary>
        /// Gets the response uri.
        /// </summary>
        public Uri ResponseUri
        {
            get { return this.httpWebResponse.ResponseUri; }
        }

        /// <summary>
        /// Gets the http status code.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return this.httpWebResponse.StatusCode; }
        }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        public string StatusDescription
        {
            get { return this.httpWebResponse.StatusDescription; }
        }

#if !SILVERLIGHT

        /// <summary>
        /// Gets a value indicating whether reponse is mutually authenticated.
        /// </summary>
        public bool IsMutuallyAuthenticated
        {
            get { return this.httpWebResponse.IsMutuallyAuthenticated; }
        }

        /// <summary>
        /// Gets the http cookies.
        /// </summary>
        public CookieCollection Cookies
        {
            get { return this.httpWebResponse.Cookies; }
        }

        /// <summary>
        /// Gets the http headers.
        /// </summary>
        public WebHeaderCollection Headers
        {
            get { return this.httpWebResponse.Headers; }
        }

        /// <summary>
        /// Gets the server.
        /// </summary>
        public string Server
        {
            get { return this.httpWebResponse.Server; }
        }

        /// <summary>
        /// Gets the protocol version.
        /// </summary>
        public Version ProtocolVersion
        {
            get { return this.httpWebResponse.ProtocolVersion; }
        }

        /// <summary>
        /// Gets the content encoding.
        /// </summary>
        public string ContentEncoding
        {
            get { return this.httpWebResponse.ContentEncoding; }
        }

        /// <summary>
        /// Gets the last modified date and time.
        /// </summary>
        public DateTime LastModified
        {
            get { return this.httpWebResponse.LastModified; }
        }

        /// <summary>
        /// Gets the character set.
        /// </summary>
        public string CharacterSet
        {
            get { return this.httpWebResponse.CharacterSet; }
        }

#endif
        /// <summary>
        /// Gets the response stream.
        /// </summary>
        /// <returns>
        /// The response stream.
        /// </returns>
        public Stream GetResponseStream()
        {
            return this.httpWebResponse.GetResponseStream();
        }
    }
}