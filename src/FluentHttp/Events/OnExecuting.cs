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
        public event EventHandler<FluentHttpRequestEventArgs> Executing;

        public FluentHttpRequest OnExecutingDo(EventHandler<FluentHttpRequestEventArgs> eventHandler)
        {
            return OnExecutingDo(eventHandler, false);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentHttpRequest OnExecutingDo(EventHandler<FluentHttpRequestEventArgs> eventHandler, bool remove)
        {
            if (eventHandler != null)
            {
                if (remove)
                    Executing -= eventHandler;
                else
                    Executing += eventHandler;
            }

            return this;
        }
    }
}