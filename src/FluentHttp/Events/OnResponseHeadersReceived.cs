namespace FluentHttp
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Fluent Http Wrapper
    /// </summary>
    public partial class FluentHttpRequestOld
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
        /// Returns <see cref="FluentHttpRequestOld"/>
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequestOld OnResponseHeadersRecevied(EventHandler<ResponseHeadersReceivedEventArgs> eventHandler)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestOld>() != null);

            if (eventHandler != null)
                ResponseHeadersReceived += eventHandler;

            return this;
        }
    }
}