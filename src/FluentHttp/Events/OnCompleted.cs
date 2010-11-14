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
        public event EventHandler<CompletedEventArgs> Completed;

        public FluentHttpRequest OnCompleted(EventHandler<CompletedEventArgs> eventHandler)
        {
            if (eventHandler != null)
                Completed += eventHandler;
            return this;
        }
    }
}