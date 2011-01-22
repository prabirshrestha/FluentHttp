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
        /// Occurs when response is read.
        /// </summary>
        public event EventHandler<ResponseReadEventArgs> ResponseRead;

        /// <summary>
        /// Occurs when the response is read.
        /// </summary>
        /// <param name="eventHandler">
        /// The event handler.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequestOld"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequestOld OnResponseRead(EventHandler<ResponseReadEventArgs> eventHandler)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestOld>() != null);

            if (eventHandler != null)
                ResponseRead += eventHandler;

            return this;
        }
    }
}