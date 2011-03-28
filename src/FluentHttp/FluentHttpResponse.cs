namespace FluentHttp
{
    /// <summary>
    /// Represents a Fluent Http Response.
    /// </summary>
    public class FluentHttpResponse
    {
        /// <summary>
        /// The http web response.
        /// </summary>
        private readonly IHttpWebResponse _httpWebResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpResponse"/> class.
        /// </summary>
        /// <param name="httpWebResponse">
        /// The http web response.
        /// </param>
        public FluentHttpResponse(IHttpWebResponse httpWebResponse)
        {
            _httpWebResponse = httpWebResponse;
        }

        /// <summary>
        /// Gets the http web response..
        /// </summary>
        public IHttpWebResponse HttpWebResponse
        {
            get { return _httpWebResponse; }
        }
    }
}