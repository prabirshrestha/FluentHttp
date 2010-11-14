namespace FluentHttp
{
    using System;

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
            if (eventHandler != null)
                ResponseRead += eventHandler;
            return this;
        }
    }
}