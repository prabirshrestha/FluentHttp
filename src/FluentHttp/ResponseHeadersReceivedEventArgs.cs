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

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseHeadersReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        public ResponseHeadersReceivedEventArgs(FluentHttpResponse response)
        {
            _response = response;
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