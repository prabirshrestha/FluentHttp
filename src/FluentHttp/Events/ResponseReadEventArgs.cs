namespace FluentHttp
{
    using System;

    /// <summary>
    /// Event Args for FluentHttpResponseEventArgs
    /// </summary>
    public class ResponseHeadersReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Fluent Http Response.
        /// </summary>
        private readonly FluentHttpResponse _fluentHttpResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseHeadersReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="fluentHttpResponse">
        /// The fluent http response.
        /// </param>
        public ResponseHeadersReceivedEventArgs(FluentHttpResponse fluentHttpResponse)
        {
            if (fluentHttpResponse == null)
                throw new ArgumentNullException("fluentHttpResponse");

            _fluentHttpResponse = fluentHttpResponse;
        }

        /// <summary>
        /// Gets the FluentHttpResponse.
        /// </summary>
        public FluentHttpResponse FluentHttpResponse
        {
            get { return _fluentHttpResponse; }
        }

        public long ContentLength
        {
            get { return _fluentHttpResponse.ContentLength; }
        }

        public string ContentType
        {
            get { return _fluentHttpResponse.ContentType; }
        }

        public object UserState { get; set; }

    }
}