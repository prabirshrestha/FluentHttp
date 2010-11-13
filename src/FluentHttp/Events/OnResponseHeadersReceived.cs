namespace FluentHttp
{
    using System;
    using System.ComponentModel;

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
            return OnResponseHeadersRecevied(eventHandler, false);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentHttpRequest OnResponseHeadersRecevied(EventHandler<ResponseHeadersReceivedEventArgs> eventHandler, bool remove)
        {
            if (eventHandler != null)
            {
                if (remove)
                    ResponseHeadersReceived -= eventHandler;
                else
                    ResponseHeadersReceived += eventHandler;
            }

            return this;
        }
    }
}