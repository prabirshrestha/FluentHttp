namespace FluentHttp
{
    using System;

    /// <summary>
    /// Fluent Http Wrapper
    /// </summary>
    public partial class FluentHttpRequest
    {
        /// <summary>
        /// Base url of the request.
        /// </summary>
        private readonly string _baseUrl;

        /// <summary>
        /// Name of the method
        /// </summary>
        private string _method;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpRequest"/> class.
        /// </summary>
        /// <param name="baseUrl">
        /// The url to make request at.
        /// </param>
        public FluentHttpRequest(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentNullException("baseUrl");

            _baseUrl = baseUrl;
            _method = "GET";
        }

        /// <summary>
        /// Gets the base url to make request at.
        /// </summary>
        public string BaseUrl
        {
            get { return _baseUrl; }
        }

        /// <summary>
        /// Sets the http method.
        /// </summary>
        /// <param name="method">
        /// The http method.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throws <see cref="ArgumentNullException"/> if method is null or empty.
        /// </exception>
        public FluentHttpRequest Method(string method)
        {
            if (string.IsNullOrEmpty(method))
                throw new ArgumentNullException("method");

            _method = method;

            return this;
        }

        public string GetMethod()
        {
            return _method;
        }

    }
}