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
        public event EventHandler<ExecutingEventArgs> Executing;

        public FluentHttpRequest OnExecuting(EventHandler<ExecutingEventArgs> eventHandler)
        {
            if (eventHandler != null)
                Executing += eventHandler;
            return this;
        }
    }
}