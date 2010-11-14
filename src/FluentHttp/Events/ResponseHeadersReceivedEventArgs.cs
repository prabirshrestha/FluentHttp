namespace FluentHttp
{
    using System;
    using System.Net;

    /// <summary>
    /// Event Args for ResponseReadEventArgs
    /// </summary>
    public class ResponseReadEventArgs : EventArgs
    {
        /// <summary>
        /// Fluent Http Response.
        /// </summary>
        private readonly FluentHttpResponse _fluentHttpResponse;

        private readonly byte[] _buffer;
        private readonly int _bufferSize;

        private readonly long _totalBytesRead;
        private readonly bool _hasMore;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseReadEventArgs"/> class.
        /// </summary>
        /// <param name="fluentHttpResponse">
        /// The fluent http response.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public ResponseReadEventArgs(FluentHttpResponse fluentHttpResponse, byte[] buffer, int bufferSize, long totalBytesRead, bool hasMore)
        {
            if (fluentHttpResponse == null)
                throw new ArgumentNullException("fluentHttpResponse");

            _fluentHttpResponse = fluentHttpResponse;
            _buffer = buffer;
            _bufferSize = bufferSize;
            _totalBytesRead = totalBytesRead;
            _hasMore = hasMore;
        }

        public object UserState { get; set; }

        public int BufferSize
        {
            get { return _bufferSize; }
        }

        /// <summary>
        /// Gets the buffer read.
        /// </summary>
        public byte[] Buffer
        {
            get { return _buffer; }
        }

        public bool HasMore
        {
            get { return _hasMore; }
        }

        /// <summary>
        /// Gets number of bytes read.
        /// </summary>
        public long TotalBytesRead
        {
            get { return _totalBytesRead; }
        }

        /// <summary>
        /// Gets the FluentHttpResponse.
        /// </summary>
        public FluentHttpResponse FluentHttpResponse
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

        public HttpStatusCode StatusCode
        {
            get { return _fluentHttpResponse.StatusCode; }
        }

    }
}