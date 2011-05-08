namespace FluentHttp
{
    using System;
    using System.IO;
    using System.Net;

    /// <summary>
    /// Represents the http web response.
    /// </summary>
#if FLUENTHTTP_CORE_INTERNAL
    internal
#else
    public
#endif
 interface IHttpWebResponse
    {
        /// <summary>
        /// Gets the http method.
        /// </summary>
        string Method { get; }

        /// <summary>
        /// Gets the content length.
        /// </summary>
        long ContentLength { get; }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Gets the response uri.
        /// </summary>
        Uri ResponseUri { get; }

        /// <summary>
        /// Gets the http status code.
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        string StatusDescription { get; }

#if !SILVERLIGHT

        /// <summary>
        /// Gets a value indicating whether reponse is mutually authenticated.
        /// </summary>
        bool IsMutuallyAuthenticated { get; }

        /// <summary>
        /// Gets the http cookies.
        /// </summary>
        CookieCollection Cookies { get; }

        /// <summary>
        /// Gets the http headers.
        /// </summary>
        WebHeaderCollection Headers { get; }

        /// <summary>
        /// Gets the server.
        /// </summary>
        string Server { get; }

        /// <summary>
        /// Gets the protocol version.
        /// </summary>
        Version ProtocolVersion { get; }

        /// <summary>
        /// Gets the content encoding.
        /// </summary>
        string ContentEncoding { get; }

        /// <summary>
        /// Gets the last modified date and time.
        /// </summary>
        DateTime LastModified { get; }

        /// <summary>
        /// Gets the character set.
        /// </summary>
        string CharacterSet { get; }

#endif

        /// <summary>
        /// Gets the response stream.
        /// </summary>
        /// <returns>
        /// The response stream.
        /// </returns>
        Stream GetResponseStream();
    }
}