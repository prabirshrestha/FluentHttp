
namespace FluentHttp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.IO;
#if !SILVERLIGHT
    using System.IO.Compression;
#endif
    using System.Net;
    using System.Text;

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

#if !SILVERLIGHT

        /// <summary>
        /// The timeout.
        /// </summary>
        private int timeout;

        /// <summary>
        /// The proxy.
        /// </summary>
        private IWebProxy proxy;

#endif

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
        /// The current async result.
        /// </summary>
        private FluentHttpAsyncResult asyncResult;

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
        /// Occurs when the response headers are received.
        /// </summary>
        public event EventHandler<ResponseHeadersReceivedEventArgs> ResponseHeadersReceived;

        /// <summary>
        /// Occurs when the response stream buffer was read.
        /// </summary>
        public event EventHandler<ResponseReadEventArgs> Read;

        /// <summary>
        /// Occurs when the request has been completed without critical errors.
        /// </summary>
        public event EventHandler<CompletedEventArgs> Completed;

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
        /// Create an instnace of new <see cref="System.Net.HttpWebRequest"/>.
        /// </summary>
        /// <param name="url">
        /// The request url.
        /// </param>
        /// <returns>
        /// Returns <see cref="System.Net.HttpWebRequest"/>.
        /// </returns>
        public HttpWebRequest CreateHttpWebRequest(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            Contract.Assume(httpWebRequest != null);
            return httpWebRequest;
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
            if (this.asyncResult != null)
            {
                throw new InvalidOperationException("Request has already started.");
            }

            AuthenticateIfRequried();

            var httpWebRequest = this.CreateHttpWebRequest(this.BuildRequestUrl());
            PrepareHttpWebRequest(httpWebRequest);

            var internalState = new InternalState(this, httpWebRequest, callback, state);
            this.asyncResult = new FluentHttpAsyncResult(internalState);

            ExecuteAsync(internalState);

            Contract.Assume(this.asyncResult != null);
            return this.asyncResult;
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
            if (asyncResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }

            var ar = asyncResult as FluentHttpAsyncResult;

            if (ar == null || !ReferenceEquals(this.asyncResult, ar))
            {
                throw new ArgumentException("asyncResult");
            }

            // wait for the request to end
            ar.AsyncWaitHandle.WaitOne();

            // propagate the exception to the one who calls EndRequest.
            if (ar.InternalState.Exception != null)
            {
                throw ar.InternalState.Exception;
            }

            return ar.InternalState.Response;
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
#if !SILVERLIGHT
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
#endif

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

#if !SILVERLIGHT

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
#endif

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

        /// <summary>
        /// Occurs when http response headers are received.
        /// </summary>
        /// <param name="onResponseHeadersReceived">
        /// On response headers received.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest OnResponseHeadersReceived(EventHandler<ResponseHeadersReceivedEventArgs> onResponseHeadersReceived)
        {
            if (onResponseHeadersReceived != null)
            {
                this.ResponseHeadersReceived += onResponseHeadersReceived;
            }

            return this;
        }

        /// <summary>
        /// Occurrs when http response is completed.
        /// </summary>
        /// <param name="onCompleted">
        /// The on completed.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest OnCompleted(EventHandler<CompletedEventArgs> onCompleted)
        {
            if (onCompleted != null)
            {
                this.Completed += onCompleted;
            }

            return this;
        }

        /// <summary>
        /// Occurs when response buffer was read.
        /// </summary>
        /// <param name="onBufferRead">
        /// The on buffer read.
        /// </param>
        /// <returns>
        /// Returns the <see cref="IFluentHttpRequest"/>.
        /// </returns>
        public IFluentHttpRequest OnRead(EventHandler<ResponseReadEventArgs> onBufferRead)
        {
            if (onBufferRead != null)
            {
                this.Read += onBufferRead;
            }

            return this;
        }

#if !(SILVERLIGHT || NET35 || NET20)

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

        /// <summary>
        /// Authenticates the <see cref="FluentHttpRequest"/> if it has an authenticator.
        /// </summary>
        internal void AuthenticateIfRequried()
        {
            var authenticator = this.GetAuthenticator();
            if (authenticator != null)
            {
                authenticator.Authenticate(this);
            }
        }

        /// <summary>
        /// Builds the request url.
        /// </summary>
        /// <returns>
        /// The request url.
        /// </returns>
        internal string BuildRequestUrl()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            var sb = new StringBuilder();

            sb.Append(this.BaseUrl);
            sb.Append(this.GetResourcePath());
            sb.Append("?");

            foreach (var qs in this.GetQueryStrings())
            {
                // these querystrings are already url encoded.
                sb.AppendFormat("{0}={1}&", qs.Name, qs.Value);
            }

            // remove the last & or ?
            --sb.Length;

            return sb.ToString();
        }

        /// <summary>
        /// Prepare <see cref="HttpWebRequest"/> by copying necessary http headers, cookies etc. to HttpWebRequest.
        /// </summary>
        /// <param name="httpWebRequest">
        /// The http web request.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        internal void PrepareHttpWebRequest(HttpWebRequest httpWebRequest)
        {
            Contract.Requires(httpWebRequest != null);

            httpWebRequest.Method = this.GetMethod();
            httpWebRequest.Credentials = this.GetCredentials();

            SetHttpWebRequestHeaders(httpWebRequest);
            SetHttpWebRequestCookies(httpWebRequest);

#if !SILVERLIGHT
            httpWebRequest.Timeout = this.GetTimeout();
            httpWebRequest.Proxy = this.GetProxy();

            // decompression methods set by accept-encoding header.
            httpWebRequest.AutomaticDecompression = DecompressionMethods.None;
#endif
        }

        /// <summary>
        /// Copy fluent http request headers to <see cref="HttpWebRequest"/>.
        /// </summary>
        /// <param name="httpWebRequest">
        /// The http web request.
        /// </param>
        internal void SetHttpWebRequestHeaders(HttpWebRequest httpWebRequest)
        {
            Contract.Requires(httpWebRequest != null);

            // set default content-length to 0 if it is not GET.
            if (!this.GetMethod().Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
#if !WINDOWS_PHONE
                httpWebRequest.ContentLength = 0;
#endif
            }

            foreach (var header in this.GetHeaders())
            {
                if (FluentHttpHeaders.IsSpecialHeader(header.Name) == -1)
                {
#if SILVERLIGHT
                    httpWebRequest.Headers[header.Name] = header.Value;
#else
                    httpWebRequest.Headers.Add(header.Name, header.Value);
#endif
                }
                else
                {
                    // todo: need to reorder so the performance is better
                    if (header.Name.Equals("accept", StringComparison.OrdinalIgnoreCase))
                    {
                        httpWebRequest.Accept = header.Value;
                    }
#if !SILVERLIGHT
                    else if (header.Name.Equals("connection", StringComparison.OrdinalIgnoreCase))
                    {
                        httpWebRequest.Connection = header.Value;
                    }
#endif
#if !WINDOWS_PHONE
                    else if (header.Name.Equals("content-length", StringComparison.OrdinalIgnoreCase))
                    {
                        httpWebRequest.ContentLength = long.Parse(header.Value);
                    }
#endif
                    else if (header.Name.Equals("content-type", StringComparison.OrdinalIgnoreCase))
                    {
                        httpWebRequest.ContentType = header.Value;
                    }
#if !SILVERLIGHT
                    else if (header.Name.Equals("expect", StringComparison.OrdinalIgnoreCase))
                    {
                        httpWebRequest.Expect = header.Value;
                    }
                    // date set by the system
                    // host set by the system
                    else if (header.Name.Equals("if-modified-since", StringComparison.OrdinalIgnoreCase))
                    {
                        httpWebRequest.IfModifiedSince = DateTime.Parse(header.Value);
                    }
                    // todo range?
                    else if (header.Name.Equals("referer", StringComparison.OrdinalIgnoreCase))
                    {
                        httpWebRequest.Referer = header.Value;
                    }
                    else if (header.Name.Equals("transfer-encoding", StringComparison.OrdinalIgnoreCase))
                    {
                        httpWebRequest.TransferEncoding = header.Value;
                    }
#endif
                    else if (header.Name.Equals("user-agent", StringComparison.OrdinalIgnoreCase))
                    {
                        httpWebRequest.UserAgent = header.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Copy fluent http request cookies to <see cref="httpWebRequest"/>.
        /// </summary>
        /// <param name="httpWebRequest">
        /// The http web request.
        /// </param>
        internal void SetHttpWebRequestCookies(HttpWebRequest httpWebRequest)
        {
            Contract.Requires(httpWebRequest != null);

            httpWebRequest.CookieContainer = new CookieContainer();

            foreach (var cookie in this.GetCookies())
            {
#if SILVERLIGHT
                httpWebRequest.CookieContainer.Add(httpWebRequest.RequestUri, new Cookie(cookie.Name, cookie.Value));
#else
                httpWebRequest.CookieContainer.Add(new Cookie(cookie.Name, cookie.Value) { Domain = httpWebRequest.RequestUri.Host });
#endif
            }
        }

        /// <summary>
        /// Executes asynchronously.
        /// </summary>
        /// <param name="internalState">
        /// The internal state.
        /// </param>
        internal void ExecuteAsync(InternalState internalState)
        {
            Contract.Requires(internalState != null);

            var httpWebRequest = internalState.HttpWebRequest;

            var requestBody = this.GetBody();
            var requestStream = requestBody.GetStream();

            if (requestStream == null || requestStream.Length == 0)
            {
                // if we don't have a body, then just read asynchronously.
                ReadResponseAsync(internalState);
            }
            else
            {
                // we have a request body.
                if (httpWebRequest.ContentType == null)
                {
                    // set appropriate content-type if it is not yet specified.
                    if (requestBody.IsMultipartFormData())
                    {
                        httpWebRequest.ContentType = "multipart/form-data; boundary=" + requestBody.GetMultipartFormDataBoundary();
                    }
                    else
                    {
                        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    }
                }
#if !WINDOWS_PHONE
                httpWebRequest.ContentLength = requestStream.Length;
#endif

                // write body asynchronously and then start reading asynchronously.
                WriteBodyAndReadResponseAsync(internalState);
            }
        }

        /// <summary>
        /// Start reading the response asynchronously.
        /// </summary>
        /// <param name="internalState">
        /// The internal state.
        /// </param>
        internal void ReadResponseAsync(InternalState internalState)
        {
            Contract.Requires(internalState != null);

            var httpWebRequest = internalState.HttpWebRequest;
            httpWebRequest.BeginGetResponse(
                ar =>
                {
                    var internalAsyncState = (InternalState)ar.AsyncState;

                    // EndGetResponse might throw error.
                    HttpWebResponse httpWebResponse = null;
                    Exception exception = null;

                    try
                    {
                        httpWebResponse = (HttpWebResponse)httpWebRequest.EndGetResponse(ar);
                    }
                    catch (WebException ex)
                    {
                        exception = ex;
                        httpWebResponse = (HttpWebResponse)ex.Response;

                        // don't state the internalAsyncState.Exception here,
                        // coz might be this response could had been successful.
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    if (exception != null && !(exception is WebException))
                    {
                        // critical error occurred.
                        internalAsyncState.Exception = exception;
                        this.asyncResult.Complete();
                    }
                    else
                    {
                        // we have got the response here so create an instance of FluentHttpResponse.
                        internalAsyncState.HttpWebResponse = httpWebResponse;
                        internalAsyncState.Response = new FluentHttpResponse(internalAsyncState.Request, httpWebResponse);

                        if (httpWebResponse == null)
                        {
                            // most likely no internet connection.
                            // some devs might find it usefull to extract more details by looking at webexception.
                            internalAsyncState.Response.Exception = exception;
                            internalAsyncState.Response.ResponseStatus = ResponseStatus.Error;

                            if (NotifyComplete(internalAsyncState))
                            {
                                this.asyncResult.Complete();
                            }
                        }
                        else
                        {
                            // we got the response headers successfully.
                            if (NotifyHeadersReceived(internalAsyncState))
                            {
                                // start reading the response stream asynchronously.
                                ReadResponseStreamAsync(internalAsyncState);
                            }
                        }
                    }
                },
                internalState);
        }

        /// <summary>
        /// Read response stream asynchronously.
        /// </summary>
        /// <param name="internalAsyncState">
        /// The internal async state.
        /// </param>
        internal void ReadResponseStreamAsync(InternalState internalAsyncState)
        {
            Contract.Requires(internalAsyncState != null);
            Contract.Requires(internalAsyncState.HttpWebResponse != null);

            var httpWebResponse = internalAsyncState.HttpWebResponse;

            var responseStream = httpWebResponse.GetResponseStream();

#if !SILVERLIGHT
            
            // TODO: need to get DotNetZip for SL4 (http://dotnetzip.codeplex.com/)
            // hopefully gzip/deflate will be supported in future version 
            // https://connect.microsoft.com/VisualStudio/feedback/details/575037/provide-support-for-gzip-deflate-compression-when-using-silverlight-client-network-stack

            var contentEncoding = httpWebResponse.ContentEncoding;

            if (!string.IsNullOrEmpty(contentEncoding))
            {
                // might have to do case insensitive search here.
                if (contentEncoding.Contains("gzip"))
                {
                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                }
                else if (contentEncoding.Contains("deflate"))
                {
                    responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
                }
            }
#endif

            var destinationStream = this.GetSaveStream();

            var streamCopier = new StreamCopier(responseStream, destinationStream, this.GetBufferSize());

            if (this.Read != null)
            {
                streamCopier.OnRead +=
                    (o, e) =>
                    {
                        if (!NotifyRead(internalAsyncState, e))
                        {
                            // error occured in notify read,
                            // so cancel further read.
                            e.Cancel = true;
                        }
                    };
            }

            streamCopier.OnCompleted +=
                (o, e) =>
                {
                    if (e.Exception != null)
                    {
                        internalAsyncState.Exception = e.Exception;
                    }
                    else if (e.IsCanceled)
                    {
                        // if cancelled
                        internalAsyncState.Response.ResponseStatus = ResponseStatus.Cancelled;
                    }
                    else
                    {
                        // copy completed successfully.
                        internalAsyncState.Response.ResponseStatus = ResponseStatus.Completed;
                    }

                    // web response read completed.
                    if (NotifyComplete(internalAsyncState))
                    {
                        if (destinationStream != null && this.SeekSaveStreamToBeginning)
                        {
                            destinationStream.Seek(0, SeekOrigin.Begin);
                        }

                        this.asyncResult.Complete();
                    }
                };

            streamCopier.BeginCopy(
                ar =>
                {
                    var innerStreamCopierAsyncState = (InternalState)ar.AsyncState;

                    try
                    {
                        streamCopier.EndCopy(ar);
                    }
                    catch (Exception ex)
                    {
                        // critical error occurred.
                        innerStreamCopierAsyncState.Exception = ex;
                        this.asyncResult.Complete();
                    }
                },
                internalAsyncState);
        }

        /// <summary>
        /// Write request body asynchronously and then start reading the response asynchronously.
        /// </summary>
        /// <param name="internalState">
        /// The internal state.
        /// </param>
        internal void WriteBodyAndReadResponseAsync(InternalState internalState)
        {
            Contract.Requires(internalState != null);

            var httpWebRequest = internalState.HttpWebRequest;

            httpWebRequest.BeginGetRequestStream(
                ar =>
                {
                    var destinationStream = httpWebRequest.EndGetRequestStream(ar);

                    var streamCopier = new StreamCopier(this.GetBody().GetStream(), destinationStream, this.GetBufferSize());

                    // we need to notify the stream copier on body read and write.
                    streamCopier.OnCompleted +=
                        (o, e) =>
                        {
                            if (e.Exception != null)
                            {
                                internalState.Exception = e.Exception;
                                this.asyncResult.Complete();
                            }
                            else if (e.IsCancelled)
                            {
                                this.asyncResult.Complete();
                            }
                            else
                            {
                                // copy completed

                                // signal request body write complete.

                                // start receiving response.
                                ReadResponseAsync(internalState);
                            }
                        };

                    streamCopier.BeginCopy(
                        arStreamCopier =>
                        {
                            try
                            {
                                streamCopier.EndCopy(arStreamCopier);
                            }
                            catch (Exception ex)
                            {
                                internalState.Exception = ex;
                                this.asyncResult.Complete();
                            }
                        },
                        null);
                },
                null);
        }

        /// <summary>
        /// Notify that the headers have been received.
        /// </summary>
        /// <param name="internalAsyncState">
        /// The internal async state.
        /// </param>
        /// <returns>
        /// Returns true if Notified headers successfully.
        /// </returns>
        internal bool NotifyHeadersReceived(InternalState internalAsyncState)
        {
            Contract.Requires(internalAsyncState != null);
            Contract.Requires(internalAsyncState.Response != null);

            var responseHeadersReceived = this.ResponseHeadersReceived;

            if (responseHeadersReceived == null)
            {
                return true;
            }

            try
            {
                var responseHeaderReceivedEventArgs = new ResponseHeadersReceivedEventArgs(internalAsyncState.Response, internalAsyncState.State);
                responseHeadersReceived(this, responseHeaderReceivedEventArgs);
                return true;
            }
            catch (Exception ex)
            {
                // we need to catch the user exception so that we can end the request.
                internalAsyncState.Exception = ex;

                // critical exception occurred so end request.
                // note: i think it might be better to have an exception called
                // OnHeaderReceivedException so that we can set the actual expcetion as
                // inner exception. So devs can know that this exception occurred somewhere
                // in OnResponseHeadersReceived event handler.
                this.asyncResult.Complete();
            }

            return false;
        }

        /// <summary>
        /// Notifies when a buffer is read.
        /// </summary>
        /// <param name="internalAsyncState">
        /// The internal async state.
        /// </param>
        /// <param name="e">
        /// The stream copy event argument.
        /// </param>
        /// <returns>
        /// Returns true if successful otherwise false.
        /// </returns>
        internal bool NotifyRead(InternalState internalAsyncState, StreamCopyEventArgs e)
        {
            Contract.Requires(e != null);
            Contract.Requires(internalAsyncState != null);
            Contract.Requires(internalAsyncState.Response != null);

            var onRead = this.Read;

            try
            {
                var responseReadEventArgs = new ResponseReadEventArgs(internalAsyncState.Response, e.Buffer, e.ActualBufferSize, e.BytesRead) { UserState = internalAsyncState.State };
                onRead(this, responseReadEventArgs);
                return true;
            }
            catch (Exception ex)
            {
                // we need to catch the user exception so that we can end the request.
                internalAsyncState.Exception = ex;

                // critical exception occurred so end request.
                // note: i think it might be better to have an exception called
                // OnReadException so that we can set the actual expcetion as
                // inner exception. So devs can know that this exception occurred somewhere
                // in OnRead event handler.
                this.asyncResult.Complete();
            }

            return false;
        }

        /// <summary>
        /// Notify that the request has ended.
        /// </summary>
        /// <param name="internalAsyncState">
        /// The internal async state.
        /// </param>
        /// <returns>
        /// Returns true if successful otherwise false.
        /// </returns>
        internal bool NotifyComplete(InternalState internalAsyncState)
        {
            Contract.Requires(internalAsyncState != null);
            Contract.Requires(internalAsyncState.Response != null);

            var completed = this.Completed;

            if (completed == null)
            {
                return true;
            }

            try
            {
                var responseHeaderReceivedEventArgs = new CompletedEventArgs(internalAsyncState.Response, internalAsyncState.State);
                completed(this, responseHeaderReceivedEventArgs);
                return true;
            }
            catch (Exception ex)
            {
                // we need to catch the user exception so that we can end the request.
                internalAsyncState.Exception = ex;

                // critical exception occurred so end request.
                // note: i think it might be better to have an exception called
                // OnCompletedException so that we can set the actual expcetion as
                // inner exception. So devs can know that this exception occurred somewhere
                // in OnCompleted event handler.
                this.asyncResult.Complete();
            }

            return false;
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here."), ContractInvariantMethod]
        private void InvarianObject()
        {
            Contract.Invariant(!string.IsNullOrEmpty(this.baseUrl));
            Contract.Invariant(!string.IsNullOrEmpty(this.method));
            Contract.Invariant(this.bufferSize >= 1);
#if !SILVERLIGHT
            Contract.Invariant(this.timeout >= 0);
#endif

            Contract.Invariant(this.headers != null);
            Contract.Invariant(this.queryStrings != null);
            Contract.Invariant(this.body != null);
            Contract.Invariant(this.cookies != null);
        }
    }
}