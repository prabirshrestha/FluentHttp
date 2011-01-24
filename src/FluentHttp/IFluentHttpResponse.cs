namespace FluentHttp
{
    using System;
    using System.Net;

    /// <summary>
    /// Represents a FluentHttpResponse.
    /// </summary>
    public interface IFluentHttpResponse
    {
        /// <summary>
        /// Gets the <see cref="IFluentHttpRequest"/>.
        /// </summary>
        IFluentHttpRequest Request { get; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        Exception Exception { get; }

        /// <summary>
        /// Gets the response status.
        /// </summary>
        ResponseStatus ResponseStatus { get; }

        // copied from http web requests.

        /// <summary>
        /// Gets the http headers.
        /// </summary>
        WebHeaderCollection Headers { get; }

        /// <summary>
        /// Gets the content length.
        /// </summary>
        long ContentLength { get; }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Gets the character set.
        /// </summary>
        string CharacterSet { get; }

        /// <summary>
        /// Gets the content encoding.
        /// </summary>
        string ContentEncoding { get; }

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        CookieCollection Cookies { get; }

        /// <summary>
        /// Gets a value indicating whether IsMutuallyAuthenticated.
        /// </summary>
        bool IsMutuallyAuthenticated { get; }

        /// <summary>
        /// Gets the last  modified date and time.
        /// </summary>
        DateTime LastModified { get; }

        /// <summary>
        /// Gets the http method.
        /// </summary>
        string Method { get; }

        /// <summary>
        /// Gets the protocol version.
        /// </summary>
        Version ProtocolVersion { get; }

        /// <summary>
        /// Gets the response uri.
        /// </summary>
        Uri ResponseUri { get; }

        /// <summary>
        /// Gets the server.
        /// </summary>
        string Server { get; }

        /// <summary>
        /// Gets the http status code.
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        string StatusDescription { get; }
    }
}