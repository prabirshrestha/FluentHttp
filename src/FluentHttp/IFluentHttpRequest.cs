namespace FluentHttp
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.IO;

    /// <summary>
    /// Represent a Fluent Http Request.
    /// </summary>
    [ContractClass(typeof(FluentHttpRequestContracts))]
    public interface IFluentHttpRequest
    {
        /// <summary>
        /// Occurs when the response headers are received.
        /// </summary>
        event EventHandler<ResponseHeadersReceivedEventArgs> ResponseHeadersReceived;

        /// <summary>
        /// Occurs when the reponse stream buffer was read.
        /// </summary>
        event EventHandler<ResponseReadEventArgs> Read;

        /// <summary>
        /// Occurs when the request has been completed without critical errors.
        /// </summary>
        event EventHandler<CompletedEventArgs> Completed;

        /// <summary>
        /// Gets the base url.
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        /// Gets a value indicating whether to seek the save stream to beginning when completed.
        /// </summary>
        bool SeekSaveStreamToBeginning { get; }

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
        /// Gets the resource path.
        /// </summary>
        /// <returns>
        /// The resource path.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        string GetResourcePath();

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
        /// Gets the http method.
        /// </summary>
        /// <returns>
        /// The http method.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        string GetMethod();


        /// <summary>
        /// Create an instnace of new <see cref="System.Net.HttpWebRequest"/>.
        /// </summary>
        /// <param name="url">
        /// The request url.
        /// </param>
        /// <returns>
        /// Returns <see cref="System.Net.HttpWebRequest"/>.
        /// </returns>
        /// <remarks>
        /// This class can be useful when mocking HttpWebRequest.
        /// </remarks>
        System.Net.HttpWebRequest CreateHttpWebRequest(string url);

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
        /// Gets the http headers.
        /// </summary>
        /// <returns>
        /// The http headers.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        FluentHttpHeaders GetHeaders();

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
        /// Gets the querystrings.
        /// </summary>
        /// <returns>
        /// The querystrings.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        FluentQueryStrings GetQueryStrings();

        /// <summary>
        /// Access the request body.
        /// </summary>
        /// <param name="body">
        /// The request body.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest Body(Action<FluentHttpRequestBody> body);

        /// <summary>
        /// Gets the request body.
        /// </summary>
        /// <returns>
        /// The resquest body.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        FluentHttpRequestBody GetBody();

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
        /// Gets the cookies.
        /// </summary>
        /// <returns>
        /// The cookies.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        FluentCookies GetCookies();

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
        /// Gets the proxy.
        /// </summary>
        /// <returns>
        /// The proxy.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        System.Net.IWebProxy GetProxy();

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
        /// Gets the credentials.
        /// </summary>
        /// <returns>
        /// The credentials.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        System.Net.ICredentials GetCredentials();

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
        /// Gets the timeout.
        /// </summary>
        /// <returns>
        /// The timeout.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetTimeout();

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
        /// Gets the buffer size.
        /// </summary>
        /// <returns>
        /// The buffer size.
        /// </returns>
        int GetBufferSize();

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
        /// Gets the authenticator.
        /// </summary>
        /// <returns>
        /// The authenticator.
        /// </returns>
        IFluentAuthenticator GetAuthenticator();

        /// <summary>
        /// Sets the stream where the response is saved.
        /// </summary>
        /// <param name="saveStream">
        /// The save stream.
        /// </param>
        /// <param name="seekSaveStreamToBeginningWhenDone">
        /// Indicates whether to seek to begining when done.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest SaveTo(Stream saveStream, bool seekSaveStreamToBeginningWhenDone);

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

        /// <summary>
        /// Gets the save stream.
        /// </summary>
        /// <returns>
        /// The save stream.
        /// </returns>
        Stream GetSaveStream();

        /// <summary>
        /// Occurs when http response headers are received.
        /// </summary>
        /// <param name="onResponseHeadersReceived">
        /// On response headers received.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest OnResponseHeadersReceived(EventHandler<ResponseHeadersReceivedEventArgs> onResponseHeadersReceived);

        /// <summary>
        /// Occurrs when http response is completed.
        /// </summary>
        /// <param name="onCompleted">
        /// The on completed.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest OnCompleted(EventHandler<CompletedEventArgs> onCompleted);

        /// <summary>
        /// Occurs when response buffer was read.
        /// </summary>
        /// <param name="onBufferRead">
        /// The on buffer read.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        IFluentHttpRequest OnRead(EventHandler<ResponseReadEventArgs> onBufferRead);

        #region Hide defualt object methods

        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);

#pragma warning disable 0108
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();
#pragma warning restore 0108

        #endregion

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