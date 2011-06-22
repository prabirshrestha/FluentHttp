namespace FluentHttp
{
    using System;
    using System.IO;
    using System.Net;

    /// <summary>
    /// Represents the http web request.
    /// </summary>
#if FLUENTHTTP_CORE_INTERNAL
    internal
#else
    public
#endif
 interface IHttpWebRequest
    {
        /// <summary>
        /// Gets or sets the http method.
        /// </summary>
        string Method { get; set; }

        /// <summary>
        /// Gets or sets the http headers.
        /// </summary>
        WebHeaderCollection Headers { get; set; }

#if !WINDOWS_PHONE
        /// <summary>
        /// Gets or sets the content length.
        /// </summary>
        long ContentLength { get; set; }
#endif

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the cookie container.
        /// </summary>
        CookieContainer CookieContainer { get; set; }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        ICredentials Credentials { get; set; }

        /// <summary>
        /// Gets the request uri.
        /// </summary>
        Uri RequestUri { get; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the accept.
        /// </summary>
        string Accept { get; set; }

#if !SILVERLIGHT
        /// <summary>
        /// Gets or sets the proxy.
        /// </summary>
        IWebProxy Proxy { get; set; }

        /// <summary>
        /// Gets or sets the if modified since.
        /// </summary>
        DateTime IfModifiedSince { get; set; }

        /// <summary>
        /// Gets or sets the referer.
        /// </summary>
        string Referer { get; set; }

        /// <summary>
        /// Gets or sets the transfer encoding.
        /// </summary>
        string TransferEncoding { get; set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the decompression method.
        /// </summary>
        DecompressionMethods AutomaticDecompression { get; set; }

        /// <summary>
        /// Gets or sets the expect.
        /// </summary>
        string Expect { get; set; }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        string Connection { get; set; }
#endif

        /// <summary>
        /// Begins the get response.
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
        IAsyncResult BeginGetResponse(AsyncCallback callback, object state);

        /// <summary>
        /// Beggins getting the request stream.
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
        IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state);

        /// <summary>
        /// Ends the http web get response.
        /// </summary>
        /// <param name="asyncResult">
        /// The async result.
        /// </param>
        /// <returns>
        /// The http web response.
        /// </returns>
        IHttpWebResponse EndGetResponse(IAsyncResult asyncResult);

        /// <summary>
        /// Ends the get request stream.
        /// </summary>
        /// <param name="asyncResult">
        /// The async result.
        /// </param>
        /// <returns>
        /// The request stream.
        /// </returns>
        Stream EndGetRequestStream(IAsyncResult asyncResult);

#if !SILVERLIGHT

        IHttpWebResponse GetResponse();

        Stream GetRequestStream();

#endif
    }
}