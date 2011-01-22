namespace FluentHttp
{
    using System;
    using System.IO;

    /// <summary>
    /// Represent a Fluent Http Request.
    /// </summary>
    public interface IFluentHttpRequest
    {
        /// <summary>
        /// Gets the base url.
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        /// Sets the resource path.
        /// </summary>
        /// <param name="resourcePath">
        /// The resource path.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest ResourcePath(string resourcePath);

        /// <summary>
        /// Sets the http method.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <returns>
        /// Returns <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest Method(string method);

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
        /// Returns the state.
        /// </returns>
        IAsyncResult BeginRequest(AsyncCallback callback, object state);

        /// <summary>
        /// Ends the http request.
        /// </summary>
        /// <param name="asyncResult">
        /// The async result.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpResponse"/>.
        /// </returns>
        IFluentHttpResponse EndRequest(IAsyncResult asyncResult);

        /// <summary>
        /// Access http headers.
        /// </summary>
        /// <param name="headers">
        /// The headers.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest Headers(Action<FluentHttpHeaders> headers);

        /// <summary>
        /// Access querystrings.
        /// </summary>
        /// <param name="queryStrings">
        /// The query strings.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest QueryStrings(Action<FluentQueryStrings> queryStrings);

        /// <summary>
        /// Access the body.
        /// </summary>
        /// <param name="body">
        /// The request body.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest Body(Action<FluentHttpRequestBody> body);

        /// <summary>
        /// Access cookies.
        /// </summary>
        /// <param name="cookies">
        /// The cookies.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest Cookies(Action<FluentCookies> cookies);

        /// <summary>
        /// Sets the proxy.
        /// </summary>
        /// <param name="proxy">
        /// The proxy.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest Proxy(System.Net.IWebProxy proxy);

        /// <summary>
        /// Sets the credentials.
        /// </summary>
        /// <param name="credentials">
        /// The credentials.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest Credentials(System.Net.ICredentials credentials);

        /// <summary>
        /// Sets the timeout.
        /// </summary>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest Timeout(int timeout);

        /// <summary>
        /// Sets the buffer size.
        /// </summary>
        /// <param name="bufferSize">
        /// The buffer size.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest BufferSize(int bufferSize);

        /// <summary>
        /// Sets the authenticator.
        /// </summary>
        /// <param name="authenticator">
        /// The authenticator.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest AuthenticateUsing(IFluentAuthenticator authenticator);

        /// <summary>
        /// Sets the authenticator.
        /// </summary>
        /// <param name="authenticator">
        /// The authenticator.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest AuthenticateUsing(Func<IFluentAuthenticator> authenticator);

        /// <summary>
        /// Sets the stream where the response is saved.
        /// </summary>
        /// <param name="saveStream">
        /// The save stream.
        /// </param>
        /// <param name="seekToBeginingWhenDone">
        /// Indicates whether to seek to begining when done.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest SaveTo(Stream saveStream, bool seekToBeginingWhenDone);

        /// <summary>
        /// Sets the stream where the response is saved.
        /// </summary>
        /// <param name="saveStream">
        /// The save stream.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        /// <remarks>
        /// Sets the seek to begining when done.
        /// </remarks>
        IFluentHttpRequest SaveTo(Stream saveStream);

#if !(NET35 || NET20)

        /// <summary>
        /// Converts the <see cref="IFluentHttpRequest"/> to Task.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="taskCreationOptions">
        /// The task creation options.
        /// </param>
        /// <returns>
        /// Returns the task of <see cref="IFluentHttpResponse"/>.
        /// </returns>
        System.Threading.Tasks.Task<IFluentHttpResponse> ToTask(object state, System.Threading.Tasks.TaskCreationOptions taskCreationOptions);

        /// <summary>
        /// Converts the <see cref="IFluentHttpRequest"/> to Task.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// Returns the task of <see cref="IFluentHttpResponse"/>.
        /// </returns>
        System.Threading.Tasks.Task<IFluentHttpResponse> ToTask(object state);

        /// <summary>
        /// Converts the <see cref="IFluentHttpRequest"/> to Task.
        /// </summary>
        /// <returns>
        /// Returns the task of <see cref="IFluentHttpResponse"/>.
        /// </returns>
        System.Threading.Tasks.Task<IFluentHttpResponse> ToTask();

#endif
    }
}