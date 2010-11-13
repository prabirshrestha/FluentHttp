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
        public event EventHandler<CompletedEventArgs> Completed;

        public FluentHttpRequest OnCompleted(EventHandler<CompletedEventArgs> eventHandler)
        {
            return OnCompleted(eventHandler, false);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentHttpRequest OnCompleted(EventHandler<CompletedEventArgs> eventHandler, bool remove)
        {
            if (eventHandler != null)
            {
                if (remove)
                    Completed -= eventHandler;
                else
                    Completed += eventHandler;
            }

            return this;
        }
    }
}