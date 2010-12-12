namespace FluentHttp
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Fluent Http Wrapper
    /// </summary>
    public partial class FluentHttpRequest
    {
        /// <summary>
        /// Base url of the request.
        /// </summary>
        private readonly string baseUrl;

        /// <summary>
        /// Name of the method
        /// </summary>
        private string method = "GET";

        /// <summary>
        /// Http timeout.
        /// </summary>
        private int timeout;

        /// <summary>
        /// Resource Path
        /// </summary>
        private string resourcePath = string.Empty;

        /// <summary>
        /// Request body and response buffer size.
        /// </summary>
        private int bufferSize = 4096;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpRequest"/> class.
        /// </summary>
        /// <param name="baseUrl">
        /// The url to make request at.
        /// </param>
        [ContractVerification(true)]
        public FluentHttpRequest(string baseUrl)
        {
            Contract.Requires(!string.IsNullOrEmpty(baseUrl));

            Contract.Ensures(!string.IsNullOrEmpty(this.baseUrl));

            this.baseUrl = baseUrl;
        }

        /// <summary>
        /// Gets the base url to make request at.
        /// </summary>
        public string BaseUrl
        {
            get { return this.baseUrl; }
        }

        /// <summary>
        /// Set resource path
        /// </summary>
        /// <param name="resourcePath">
        /// The resource path.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest ResourcePath(string resourcePath)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            if (!(string.IsNullOrEmpty(resourcePath) || (resourcePath = resourcePath.Trim()).Length == 0))
            {
                if (resourcePath.EndsWith("/"))
                {
                    // if ends with trailing slash remove it.
                    resourcePath = resourcePath.Substring(0, resourcePath.Length - 1);
                }

                // if not null or empty
                if (resourcePath.Length > 0 && resourcePath[0] != '/')
                {
                    // if doesn't start with / then add /
                    resourcePath = "/" + resourcePath;
                }
            }

            this.resourcePath = resourcePath;

            return this;
        }

        /// <summary>
        /// Gets the resource path.
        /// </summary>
        /// <returns>
        /// Returns the resource path.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetResourcePath()
        {
            return resourcePath;
        }

        /// <summary>
        /// Sets the http method.
        /// </summary>
        /// <param name="method">
        /// The http method.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throws <see cref="ArgumentNullException"/> if method is null or empty.
        /// </exception>
        [ContractVerification(true)]
        public FluentHttpRequest Method(string method)
        {
            Contract.Requires(!string.IsNullOrEmpty(method));
            // Contract.Requires(Contract.Exists(new[] { "GET", "POST", "DELETE" }, p => p.Equals(method, StringComparison.OrdinalIgnoreCase)));

            Contract.Ensures(!string.IsNullOrEmpty(method));
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            this.method = method;

            return this;
        }

        /// <summary>
        /// Gets the type of Http Method
        /// </summary>
        /// <returns>
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetMethod()
        {
            return method;
        }

        /// <summary>
        /// Sets the timeout.
        /// </summary>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        /// <returns>
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest Timeout(int timeout)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            this.timeout = timeout;

            return this;
        }

        /// <summary>
        /// Gets the timeout.
        /// </summary>
        /// <returns>
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetTimeout()
        {
            return timeout;
        }

        /// <summary>
        /// Sets the buffer size.
        /// </summary>
        /// <param name="bufferSize">
        /// The buffer size.
        /// </param>
        /// <returns>
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ContractVerification(true)]
        public FluentHttpRequest BufferSize(int bufferSize)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize", "Buffer size must be greater than 0");
            this.bufferSize = bufferSize;

            return this;
        }

        /// <summary>
        /// Gets the buffer size.
        /// </summary>
        /// <returns>
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetBufferSize()
        {
            return bufferSize;
        }

        /// <summary>
        /// Request body.
        /// </summary>
        private IHttpRequestBody httpRequestBody;

        /// <summary>
        /// Sets the http request body.
        /// </summary>
        /// <param name="httpRequestBody">
        /// The http request body.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>.
        /// </returns>
        [ContractVerification(true)]
        private FluentHttpRequest RequestBody(IHttpRequestBody httpRequestBody)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            this.httpRequestBody = httpRequestBody;

            return this;
        }

        /// <summary>
        /// Gets teh http request body.
        /// </summary>
        /// <returns>
        /// Returns <see cref="IHttpRequestBody"/>.
        /// </returns>
        private IHttpRequestBody GetRequestBody()
        {
            return this.httpRequestBody;
        }

        /// <summary>
        /// Sets the contents of http request body.
        /// </summary>
        /// <param name="contents">
        /// The string contents.
        /// </param>
        /// <param name="encoding">
        /// The encoding.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest Body(string contents, Encoding encoding)
        {
            Contract.Requires(!string.IsNullOrEmpty(contents));
            Contract.Requires(encoding != null);
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            return Body(encoding.GetBytes(contents));
        }

        /// <summary>
        /// Sets the sepecified string as http request body using UTF8 encoding.
        /// </summary>
        /// <param name="contents">
        /// The contents.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest Body(string contents)
        {
            Contract.Requires(!string.IsNullOrEmpty(contents));
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            return Body(contents, Encoding.UTF8);
        }

        /// <summary>
        /// Sets the http request body.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest Body(byte[] data)
        {
            if (this.httpRequestBody != null)
            {
                throw new InvalidOperationException(
                    "Body has already been set. Call ClearBody() before setting a new body.");
            }

            return data == null ? ClearBody() : RequestBody(new StreamHttpBody(new MemoryStream(data)));
        }

        /// <summary>
        /// Clears the http request body.
        /// </summary>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest ClearBody()
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            this.httpRequestBody = null;

            return this;
        }

        private Stream saveStream;
        private bool saveStreamSeekToBeginning;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [ContractVerification(true)]
        public FluentHttpRequest SaveTo(Stream stream, bool seekToBeginingWhenDone)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            this.saveStream = stream;

            if (stream == null)
            {
                return this;
            }

            if (!stream.CanWrite)
            {
                throw new ArgumentException("stream is not writable.");
            }

            if (seekToBeginingWhenDone)
            {
                if (!stream.CanSeek)
                {
                    throw new ArgumentException("stream is not seekable");
                }
            }

            this.saveStreamSeekToBeginning = seekToBeginingWhenDone;

            return this;
        }

        [ContractVerification(true)]
        public FluentHttpRequest SaveTo(Stream stream)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            return SaveTo(stream, true);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Stream GetSaveStream()
        {
            return this.saveStream;
        }

        #region Hide defualt object methods

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

#pragma warning disable 0108
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Type GetType()
        {
            return base.GetType();
        }
#pragma warning restore 0108

        #endregion

        [ContractInvariantMethod]
        [ContractVerification(true)]
        void InvariantObject()
        {
            Contract.Invariant(!string.IsNullOrEmpty(method));
        }
    }
}