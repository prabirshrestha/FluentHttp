namespace FluentHttp
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net;

    /// <summary>
    /// Event Args for FluentHttpResponseEventArgs
    /// </summary>
    public class ResponseHeadersReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Fluent Http Response.
        /// </summary>
        private readonly IFluentHttpResponse _fluentHttpResponse;

        private readonly object _userState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseHeadersReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="fluentHttpResponse">
        /// The fluent http response.
        /// </param>
        public ResponseHeadersReceivedEventArgs(IFluentHttpResponse fluentHttpResponse, object userState)
        {
            Contract.Requires(fluentHttpResponse != null);

            _fluentHttpResponse = fluentHttpResponse;
            _userState = userState;
        }

        public object UserState
        {
            get { return _userState; }
        }

        /// <summary>
        /// Gets the FluentHttpResponse.
        /// </summary>
        public IFluentHttpResponse FluentHttpResponse
        {
            get { return _fluentHttpResponse; }
        }

        public long ContentLength
        {
            get { return _fluentHttpResponse.ContentLength; }
        }

        public string ContentType
        {
            get { return _fluentHttpResponse.ContentType; }
        }

#if !SILVERLIGHT

        public string ContentEncoding
        {
            get { return _fluentHttpResponse.ContentEncoding; }
        }
#endif

        public HttpStatusCode StatusCode
        {
            get { return _fluentHttpResponse.StatusCode; }
        }


    }
}