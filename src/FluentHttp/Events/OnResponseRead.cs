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
        /// Occurs when response is read.
        /// </summary>
        public event EventHandler<ResponseReadEventArgs> ResponseRead;

        public FluentHttpRequest OnResponseRead(EventHandler<ResponseReadEventArgs> eventHandler)
        {
            return OnResponseRead(eventHandler, false);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentHttpRequest OnResponseRead(EventHandler<ResponseReadEventArgs> eventHandler, bool remove)
        {
            if (eventHandler != null)
            {
                if (remove)
                    ResponseRead -= eventHandler;
                else
                    ResponseRead += eventHandler;
            }

            return this;
        }
    }
}