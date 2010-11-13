namespace FluentHttp
{
    using System.Net;

    public class FluentHttpResponse
    {
        public FluentHttpResponse()
        {
            ResponseStatus = ResponseStatus.Non;
        }

        public ResponseStatus ResponseStatus { get; set; }

        public HttpWebResponse HttpWebResponse { get; set; }

        public long ContentLength { get; set; }
        public string ContentType { get; set; }

        public WebHeaderCollection Headers { get; set; }
    }
}