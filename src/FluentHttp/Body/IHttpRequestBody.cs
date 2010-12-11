namespace FluentHttp
{
    /// <summary>
    /// Http Request Body
    /// </summary>
    interface IHttpRequestBody
    {
        /// <summary>
        /// Gets the content length of the Request Body.
        /// </summary>
        long ContentLength { get; }
    }
}