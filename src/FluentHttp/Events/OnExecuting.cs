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
        public event EventHandler<ExecutingEventArgs> Executing;

        /// <summary>
        /// Occurs just before the web request is executed.
        /// </summary>
        /// <param name="eventHandler">
        /// The event handler.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequestOld"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequestOld OnExecuting(EventHandler<ExecutingEventArgs> eventHandler)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestOld>() != null);

            if (eventHandler != null)
                Executing += eventHandler;

            return this;
        }
    }
}