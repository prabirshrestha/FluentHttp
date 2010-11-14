namespace FluentHttp
{
    using System;

    /// <summary>
    /// Fluent Http Wrapper
    /// </summary>
    public partial class FluentHttpRequest
    {
        /// <summary>
        /// Occurs just before executing the web request.
        /// </summary>
        public event EventHandler<ResponseHeadersReceivedEventArgs> ResponseHeadersReceived;

        public FluentHttpRequest OnResponseHeadersRecevied(EventHandler<ResponseHeadersReceivedEventArgs> eventHandler)
        {
            if (eventHandler != null)
                ResponseHeadersReceived += eventHandler;
            return this;
        }
    }
}