namespace FluentHttp
{
    using System;
    using System.IO;

    /// <summary>
    /// Represents the event args for response headers received.
    /// </summary>
    public class ResponseHeadersReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// The fluent http response.
        /// </summary>
        private readonly FluentHttpResponse _response;

        private readonly object _asyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseHeadersReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        public ResponseHeadersReceivedEventArgs(FluentHttpResponse response, object asyncState)
        {
            _response = response;
            _asyncState = asyncState;
        }

        public object AsyncState
        {
            get { return _asyncState; }
        }

        /// <summary>
        /// Gets the fluent http response.
        /// </summary>
        public FluentHttpResponse Response
        {
            get { return _response; }
        }

        /// <summary>
        /// Gets or sets the response save stream.
        /// </summary>
        public Stream ResponseSaveStream { get; set; }
    }
}