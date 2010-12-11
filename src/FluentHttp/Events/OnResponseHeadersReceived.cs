namespace FluentHttp
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Fluent Http Wrapper
    /// </summary>
    public partial class FluentHttpRequest
    {
        /// <summary>
        /// Occurs just before executing the web request.
        /// </summary>
        public event EventHandler<ResponseHeadersReceivedEventArgs> ResponseHeadersReceived;

        /// <summary>
        /// Occurs when http response headers are received.
        /// </summary>
        /// <param name="eventHandler">
        /// The event handler.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest OnResponseHeadersRecevied(EventHandler<ResponseHeadersReceivedEventArgs> eventHandler)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            if (eventHandler != null)
                ResponseHeadersReceived += eventHandler;

            return this;
        }
    }
}