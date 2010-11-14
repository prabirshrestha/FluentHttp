namespace FluentHttp
{
    using System;
    using System.Net;

    public class FluentHttpResponse
    {
        public FluentHttpResponse()
        {
            ResponseStatus = ResponseStatus.Non;
        }

        public ResponseStatus ResponseStatus { get; set; }

        public HttpWebResponse HttpWebResponse { get; set; }

        public WebHeaderCollection Headers { get; set; }

        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public string CharacterSet { get; set; }
        public string ContentEncoding { get; set; }
        public CookieCollection Cookies { get; set; }
        public bool IsMutuallyAuthenticated { get; set; }
        public DateTime LastModified { get; set; }
        public string Method { get; set; }
        public Version ProtocolVersion { get; set; }
        public Uri ResponseUri { get; set; }
        public string Server { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public string StatusDescription { get; set; }
    }
}