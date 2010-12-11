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
        public event EventHandler<CompletedEventArgs> Completed;

        /// <summary>
        /// Occurs when the web request is completed.
        /// </summary>
        /// <param name="eventHandler">
        /// The event handler.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest OnCompleted(EventHandler<CompletedEventArgs> eventHandler)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            if (eventHandler != null)
                Completed += eventHandler;

            return this;
        }
    }
}