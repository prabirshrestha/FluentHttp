namespace FluentHttp
{
    using System;

    /// <summary>
    /// Represents a Fluent Http Request.
    /// </summary>
    public class FluentHttpRequest
    {
        /// <summary>
        /// Starts the http request.
        /// </summary>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The async result.
        /// </returns>
        public IAsyncResult BeginExecute(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ends the http request.
        /// </summary>
        /// <param name="asyncResult">
        /// The async result.
        /// </param>
        /// <returns>
        /// Returns the <see cref="FluentHttpResponse"/>.
        /// </returns>
        public FluentHttpResponse EndExecute(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }
    }
}