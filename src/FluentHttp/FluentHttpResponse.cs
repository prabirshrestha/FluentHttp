namespace FluentHttp
{
    public class FluentHttpResponse
    {
        public FluentHttpResponse()
        {
            ResponseStatus = ResponseStatus.Non;
        }

        public ResponseStatus ResponseStatus { get; set; }
    }
}