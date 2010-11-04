namespace FluentHttp
{

    /// <summary>
    /// Fluent Http Wrapper
    /// </summary>
    public partial class FluentHttpRequest
    {
        /// <summary>
        /// Url to make request at.
        /// </summary>
        private readonly string _url;

        /// <summary>
        /// Http Method.
        /// </summary>
        private readonly string _method;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpRequest"/> class.
        /// </summary>
        /// <param name="url">
        /// The url to make request at.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        public FluentHttpRequest(string url, string method)
        {
            _url = url;
            _method = method;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpRequest"/> class.
        /// </summary>
        /// <param name="url">
        /// The url to make request at.
        /// </param>
        public FluentHttpRequest(string url)
            : this(url, "GET")
        {
        }

        /// <summary>
        /// Gets Method.
        /// </summary>
        public string Method
        {
            get { return _method; }
        }

        /// <summary>
        /// Gets the url to make request at.
        /// </summary>
        public string Url
        {
            get { return _url; }
        }

    }
}