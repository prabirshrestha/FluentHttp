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
        public event EventHandler<ExecutingEventArgs> Executing;

        /// <summary>
        /// Occurs just before the web request is executed.
        /// </summary>
        /// <param name="eventHandler">
        /// The event handler.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest OnExecuting(EventHandler<ExecutingEventArgs> eventHandler)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            if (eventHandler != null)
                Executing += eventHandler;

            return this;
        }
    }
}