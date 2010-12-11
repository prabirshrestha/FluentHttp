namespace FluentHttp
{
    using System.IO;

    /// <summary>
    /// Http Request Body
    /// </summary>
    interface IHttpRequestBody
    {
        /// <summary>
        /// Gets Readable stream.
        /// </summary>
        Stream Stream { get; }

        /// <summary>
        /// Gets the content length of the Request Body.
        /// </summary>
        long ContentLength { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to auto dispose stream once completed.
        /// </summary>
        bool AutoDisposeStream { get; set; }

        /// <summary>
        /// Gets the content-type.
        /// </summary>
        string ContentType { get; }
    }
}