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
        public event EventHandler<CompletedEventArgs> Completed;

        /// <summary>
        /// Occurs when the web request is completed.
        /// </summary>
        /// <param name="eventHandler">
        /// The event handler.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequestOld"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequestOld OnCompleted(EventHandler<CompletedEventArgs> eventHandler)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestOld>() != null);

            if (eventHandler != null)
                Completed += eventHandler;

            return this;
        }
    }
}