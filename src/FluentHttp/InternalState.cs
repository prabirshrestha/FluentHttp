namespace FluentHttp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Net;

    /// <summary>
    /// Represents an internal state class necessary to perform async operation.
    /// </summary>
    internal class InternalState
    {
        /// <summary>
        /// The request.
        /// </summary>
        private readonly IFluentHttpRequest request;

        /// <summary>
        /// The http web request.
        /// </summary>
        private readonly HttpWebRequest httpWebRequest;

        /// <summary>
        /// The user callback.
        /// </summary>
        private readonly AsyncCallback callback;

        /// <summary>
        /// The user state.
        /// </summary>
        private readonly object state;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalState"/> class.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="httpWebRequest">
        /// The http web request.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public InternalState(IFluentHttpRequest request, HttpWebRequest httpWebRequest, AsyncCallback callback, object state)
        {
            Contract.Requires(request != null);
            Contract.Requires(httpWebRequest != null);

            this.request = request;
            this.httpWebRequest = httpWebRequest;
            this.callback = callback;
            this.state = state;
        }

        /// <summary>
        /// Gets the user state.
        /// </summary>
        public object State
        {
            get { return this.state; }
        }

        /// <summary>
        /// Gets the user callback.
        /// </summary>
        public AsyncCallback Callback
        {
            get { return this.callback; }
        }

        /// <summary>
        /// Gets the <see cref="HttpWebRequest"/>.
        /// </summary>
        public HttpWebRequest HttpWebRequest
        {
            get
            {
                Contract.Ensures(Contract.Result<HttpWebRequest>() != null);
                return this.httpWebRequest;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="HttpWebResponse"/>.
        /// </summary>
        public HttpWebResponse HttpWebResponse { get; set; }

        /// <summary>
        /// Gets or sets the fluent http response.
        /// </summary>
        public FluentHttpResponse Response { get; set; }

        /// <summary>
        /// Gets or sets the exception to be thrown when we call EndRequest.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets the fluent http request.
        /// </summary>
        public IFluentHttpRequest Request
        {
            get
            {
                Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);
                return this.request;
            }
        }

        /// <summary>
        /// Gets or sets the totaly bytes read during current transfer.
        /// </summary>
        public long BytesRead { get; set; }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here."), ContractInvariantMethod]
        private void InvarianObject()
        {
            Contract.Invariant(this.request != null);
            Contract.Invariant(this.httpWebRequest != null);
        }
    }
}