namespace FluentHttp
{
    using System;

    /// <summary>
    /// Event Args for ExecutingEventArgs
    /// </summary>
    public class ExecutingEventArgs : EventArgs
    {
        /// <summary>
        /// Fluent Http Request.
        /// </summary>
        private readonly FluentHttpRequest _fluentHttpRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutingEventArgs"/> class.
        /// </summary>
        /// <param name="fluentHttpRequest">
        /// The fluent http request.
        /// </param>
        public ExecutingEventArgs(FluentHttpRequest fluentHttpRequest)
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