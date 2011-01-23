namespace FluentHttp
{
    using System;
    using System.Net;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Event Args for CompletedEventArgs
    /// </summary>
    public class CompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Fluent Http Response.
        /// </summary>
        private readonly IFluentHttpResponse fluentHttpResponse;

        /// <summary>
        /// User state.
        /// </summary>
        private readonly object userState;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompletedEventArgs"/> class.
        /// </summary>
        /// <param name="fluentHttpResponse">
        /// The fluent http response.
        /// </param>
        /// <param name="userState">
        /// The user state.
        /// </param>
        public CompletedEventArgs(IFluentHttpResponse fluentHttpResponse, object userState)
        {
            Contract.Requires(fluentHttpResponse != null);

            this.fluentHttpResponse = fluentHttpResponse;
            this.userState = userState;
        }

        /// <summary>
        /// Gets the user state.
        /// </summary>
        public object UserState
        {
            get { return this.userState; }
        }

        /// <summary>
        /// Gets the <see cref="FluentHttpResponse"/>.
        /// </summary>
        public IFluentHttpResponse FluentHttpResponse
        {
            get { return this.fluentHttpResponse; }
        }

        /// <summary>
        /// Gets the response status.
        /// </summary>
        public ResponseStatus ResponseStatus
        {
            get { return this.fluentHttpResponse.ResponseStatus; }
        }

        /// <summary>
        /// Gets the http status code.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return this.fluentHttpResponse.StatusCode; }
        }
    }
}