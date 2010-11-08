namespace FluentHttp
{
    /// <summary>
    /// Fluent Http Wrapper
    /// </summary>
    public partial class FluentHttpRequest
    {
        /// <summary>
        /// Executes the method synchronously.
        /// </summary>
        /// <returns>
        /// </returns>
        public FluentHttpResponse Execute()
        {
            if (Executing != null)
                Executing(this, new FluentHttpRequestEventArgs(this));

            AuthenticateIfRequried();

            var request = CreateHttpWebRequest(this);

            return null;
        }
    }
}