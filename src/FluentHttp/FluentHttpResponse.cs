namespace FluentHttp
{
    using System;
    using System.Net;

    public class FluentHttpResponse
    {
        private readonly FluentHttpRequest _fluentHttpRequest;

        public FluentHttpResponse(FluentHttpRequest fluentHttpRequest, HttpWebResponse httpWebResponse)
        {
            ResponseStatus = ResponseStatus.Non;
            _fluentHttpRequest = fluentHttpRequest;

            if (httpWebResponse != null)
            {
                ContentLength = httpWebResponse.ContentLength;
                ContentType = httpWebResponse.ContentType;
                Headers = httpWebResponse.Headers;
                CharacterSet = httpWebResponse.CharacterSet;
                ContentEncoding = httpWebResponse.ContentEncoding;
                Cookies = httpWebResponse.Cookies;
                IsMutuallyAuthenticated = httpWebResponse.IsMutuallyAuthenticated;
                LastModified = httpWebResponse.LastModified;
                Method = httpWebResponse.Method;
                ProtocolVersion = httpWebResponse.ProtocolVersion;
                ResponseUri = httpWebResponse.ResponseUri;
                Server = httpWebResponse.Server;
                StatusCode = httpWebResponse.StatusCode;
                StatusDescription = httpWebResponse.StatusDescription;
            }
        }

        public FluentHttpRequest Request { get { return _fluentHttpRequest; } }

        public Exception Exception { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        // Copied from http web response.
        // TODO: might need to make it readonly.
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