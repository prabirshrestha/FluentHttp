namespace FluentHttp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net;

    /// <summary>
    /// Represents a Fluent Http Request.
    /// </summary>
    public class FluentHttpRequest : IFluentHttpRequest
    {
        /// <summary>
        /// The base url.
        /// </summary>
        private readonly string baseUrl;

        /// <summary>
        /// The http headers.
        /// </summary>
        private readonly FluentHttpHeaders headers;

        /// <summary>
        /// The querystrings.
        /// </summary>
        private readonly FluentQueryStrings queryStrings;

        /// <summary>
        /// The request body.
        /// </summary>
        private readonly FluentHttpRequestBody body;

        /// <summary>
        /// The cookies.
        /// </summary>
        private readonly FluentCookies cookies;

        /// <summary>
        /// The resource path.
        /// </summary>
        private string resourcePath;

        /// <summary>
        /// The http method.
        /// </summary>
        private string method;

        /// <summary>
        /// The buffer size.
        /// </summary>
        private int bufferSize;

        /// <summary>
        /// The timeout.
        /// </summary>
        private int timeout;

        /// <summary>
        /// The proxy.
        /// </summary>
        private IWebProxy proxy;

        /// <summary>
        /// The credentials.
        /// </summary>
        private ICredentials credentials;

        /// <summary>
        /// The authenticator.
        /// </summary>
        private IFluentAuthenticator authenticator;

        /// <summary>
        /// Destination Stream to save the response.
        /// </summary>
        private Stream saveStream;

        /// <summary>
        /// Seeks to beginning when done.
        /// </summary>
        private bool seekSaveStreamToBeginning;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpRequest"/> class.
        /// </summary>
        /// <param name="baseUrl">
        /// The base url.
        /// </param>
        public FluentHttpRequest(string baseUrl)
        {
            Contract.Requires(!string.IsNullOrEmpty(baseUrl));

            this.baseUrl = baseUrl;

            this.headers = new FluentHttpHeaders();
            this.queryStrings = new FluentQueryStrings();
            this.body = new FluentHttpRequestBody();
            this.cookies = new FluentCookies();

            // set default values here
            this.method = "GET";
            this.bufferSize = 4096;
        }

        /// <summary>
        /// Gets the base url.
        /// </summary>
        public string BaseUrl
        {
            get { return this.baseUrl; }
        }

        /// <summary>
        /// Gets a value indicating whether to seek the save stream to beginning when completed.
        /// </summary>
        public bool SeekSaveStreamToBeginning
        {
            get { return this.seekSaveStreamToBeginning; }
        }

        /// <summary>
        /// Sets the resource path.
        /// </summary>
        /// <param name="resourcePath">
        /// The resource path.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest ResourcePath(string resourcePath)
        {
            this.resourcePath = string.IsNullOrEmpty(resourcePath)
                                    ? string.Empty
                                    : Utils.AddStartingSlashInNotPresent(resourcePath);

            return this;
        }

        /// <summary>
        /// Gets the resource path.
        /// </summary>
        /// <returns>
        /// The resource path.
        /// </returns>
        public string GetResourcePath()
        {
            return this.resourcePath ?? string.Empty;
        }

        /// <summary>
        /// Sets the http method.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <returns>
        /// Returns <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest Method(string method)
        {
            this.method = method;
            return this;
        }

        /// <summary>
        /// Gets the http method.
        /// </summary>
        /// <returns>
        /// The http method.
        /// </returns>
        public string GetMethod()
        {
            return this.method;
        }

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
        public IAsyncResult BeginRequest(AsyncCallback callback, object state)
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
        /// Returns the <see cref="IFluentHttpResponse"/>.
        /// </returns>
        public IFluentHttpResponse EndRequest(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Access http headers.
        /// </summary>
        /// <param name="headers">
        /// The headers.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest Headers(Action<FluentHttpHeaders> headers)
        {
            if (headers != null)
            {
                headers(this.headers);
            }

            return this;
        }

        /// <summary>
        /// Gets the http headers.
        /// </summary>
        /// <returns>
        /// The http headers.
        /// </returns>
        public FluentHttpHeaders GetHeaders()
        {
            return this.headers;
        }

        /// <summary>
        /// Access querystrings.
        /// </summary>
        /// <param name="queryStrings">
        /// The query strings.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest QueryStrings(Action<FluentQueryStrings> queryStrings)
        {
            if (queryStrings != null)
            {
                queryStrings(this.queryStrings);
            }

            return this;
        }

        /// <summary>
        /// Gets the querystrings.
        /// </summary>
        /// <returns>
        /// The querystrings.
        /// </returns>
        public FluentQueryStrings GetQueryStrings()
        {
            return this.queryStrings;
        }

        /// <summary>
        /// Access the request body.
        /// </summary>
        /// <param name="body">
        /// The request body.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest Body(Action<FluentHttpRequestBody> body)
        {
            if (body != null)
            {
                body(this.body);
            }

            return this;
        }

        /// <summary>
        /// Gets the request body.
        /// </summary>
        /// <returns>
        /// The resquest body.
        /// </returns>
        public FluentHttpRequestBody GetBody()
        {
            return this.body;
        }

        /// <summary>
        /// Access cookies.
        /// </summary>
        /// <param name="cookies">
        /// The cookies.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest Cookies(Action<FluentCookies> cookies)
        {
            if (cookies != null)
            {
                cookies(this.cookies);
            }

            return this;
        }

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        /// <returns>
        /// The cookies.
        /// </returns>
        public FluentCookies GetCookies()
        {
            return this.cookies;
        }

        /// <summary>
        /// Sets the proxy.
        /// </summary>
        /// <param name="proxy">
        /// The proxy.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest Proxy(IWebProxy proxy)
        {
            this.proxy = proxy;
            return this;
        }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <returns>
        /// The proxy.
        /// </returns>
        public IWebProxy GetProxy()
        {
            return this.proxy;
        }

        /// <summary>
        /// Sets the credentials.
        /// </summary>
        /// <param name="credentials">
        /// The credentials.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest Credentials(ICredentials credentials)
        {
            this.credentials = credentials;
            return this;
        }

        /// <summary>
        /// Gets the credentials.
        /// </summary>
        /// <returns>
        /// The credentials.
        /// </returns>
        public ICredentials GetCredentials()
        {
            return this.credentials;
        }

        /// <summary>
        /// Sets the timeout.
        /// </summary>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest Timeout(int timeout)
        {
            this.timeout = timeout;
            return this;
        }

        /// <summary>
        /// Gets the timeout.
        /// </summary>
        /// <returns>
        /// The timeout.
        /// </returns>
        public int GetTimeout()
        {
            return this.timeout;
        }

        /// <summary>
        /// Sets the buffer size.
        /// </summary>
        /// <param name="bufferSize">
        /// The buffer size.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest BufferSize(int bufferSize)
        {
            this.bufferSize = bufferSize;
            return this;
        }

        /// <summary>
        /// Gets the buffer size.
        /// </summary>
        /// <returns>
        /// The buffer size.
        /// </returns>
        public int GetBufferSize()
        {
            return this.bufferSize;
        }

        /// <summary>
        /// Sets the authenticator.
        /// </summary>
        /// <param name="authenticator">
        /// The authenticator.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest AuthenticateUsing(IFluentAuthenticator authenticator)
        {
            this.authenticator = authenticator;
            return this;
        }

        /// <summary>
        /// Sets the authenticator.
        /// </summary>
        /// <param name="authenticator">
        /// The authenticator.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest AuthenticateUsing(Func<IFluentAuthenticator> authenticator)
        {
            return authenticator != null ? this.AuthenticateUsing(authenticator()) : this;
        }

        /// <summary>
        /// Gets the authenticator.
        /// </summary>
        /// <returns>
        /// The authenticator.
        /// </returns>
        public IFluentAuthenticator GetAuthenticator()
        {
            return this.authenticator;
        }

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
        public IFluentHttpRequest SaveTo(Stream saveStream, bool seekSaveStreamToBeginningWhenDone)
        {
            this.saveStream = saveStream;

            if (saveStream == null)
            {
                return this;
            }

            if (!saveStream.CanWrite)
            {
                throw new ArgumentException("stream is not writable.");
            }

            if (seekSaveStreamToBeginningWhenDone)
            {
                if (!saveStream.CanSeek)
                {
                    throw new ArgumentException("stream is not seekable");
                }
            }

            this.seekSaveStreamToBeginning = seekSaveStreamToBeginningWhenDone;

            return this;
        }

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
        public IFluentHttpRequest SaveTo(Stream saveStream)
        {
            return this.SaveTo(saveStream, true);
        }

        /// <summary>
        /// Gets the save stream.
        /// </summary>
        /// <returns>
        /// The save stream.
        /// </returns>
        public Stream GetSaveStream()
        {
            return this.saveStream;
        }

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
        public System.Threading.Tasks.Task<IFluentHttpResponse> ToTask(object state, System.Threading.Tasks.TaskCreationOptions taskCreationOptions)
        {
            Contract.Assume(System.Threading.Tasks.Task.Factory != null);
            var task = System.Threading.Tasks.Task.Factory.FromAsync<IFluentHttpResponse>(this.BeginRequest, this.EndRequest, state, taskCreationOptions);
            Contract.Assume(task != null);
            return task;
        }

        /// <summary>
        /// Converts the <see cref="IFluentHttpRequest"/> to Task.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// Returns the task of <see cref="IFluentHttpResponse"/>.
        /// </returns>
        public System.Threading.Tasks.Task<IFluentHttpResponse> ToTask(object state)
        {
            return this.ToTask(state, System.Threading.Tasks.TaskCreationOptions.None);
        }

        /// <summary>
        /// Converts the <see cref="IFluentHttpRequest"/> to Task.
        /// </summary>
        /// <returns>
        /// Returns the task of <see cref="IFluentHttpResponse"/>.
        /// </returns>
        public System.Threading.Tasks.Task<IFluentHttpResponse> ToTask()
        {
            return this.ToTask(null);
        }

#endif

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here."), ContractInvariantMethod]
        private void InvarianObject()
        {
            Contract.Invariant(!string.IsNullOrEmpty(this.baseUrl));
            Contract.Invariant(!string.IsNullOrEmpty(this.method));
            Contract.Invariant(this.bufferSize >= 1);
            Contract.Invariant(this.timeout >= 0);

            Contract.Invariant(this.headers != null);
            Contract.Invariant(this.queryStrings != null);
            Contract.Invariant(this.body != null);
            Contract.Invariant(this.cookies != null);
        }
    }
}