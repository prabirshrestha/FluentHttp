namespace FluentHttp
{
    using System;

    /// <summary>
    /// Event Args for CompletedEventArgs
    /// </summary>
    public class CompletedEventArgs : EventArgs
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
        public CompletedEventArgs(FluentHttpResponse fluentHttpResponse)
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

        public object UserState { get; set; }

        public ResponseStatus ResponseStatus
        {
            get { return _fluentHttpResponse.ResponseStatus; }
        }

    }
}