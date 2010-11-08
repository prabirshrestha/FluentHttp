namespace FluentHttp
{
    using System;

    /// <summary>
    /// Event Args for FluentHttpRequest
    /// </summary>
    public class FluentHttpRequestEventArgs : EventArgs
    {
        /// <summary>
        /// Fluent Http Request.
        /// </summary>
        private readonly FluentHttpRequest _fluentHttpRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpRequestEventArgs"/> class.
        /// </summary>
        /// <param name="fluentHttpRequest">
        /// The fluent http request.
        /// </param>
        public FluentHttpRequestEventArgs(FluentHttpRequest fluentHttpRequest)
        {
            _fluentHttpRequest = fluentHttpRequest;
        }

        /// <summary>
        /// Gets the FluentHttpRequest.
        /// </summary>
        public FluentHttpRequest FluentHttpRequest
        {
            get { return _fluentHttpRequest; }
        }
    }
}