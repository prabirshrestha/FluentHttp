namespace FluentHttp
{
    using System;
    using System.Net;
    using System.Text;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseReadEventArgs"/> class.
        /// </summary>
        /// <param name="fluentHttpResponse">
        /// The fluent http response.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public ResponseReadEventArgs(FluentHttpResponse fluentHttpResponse, byte[] buffer, int bufferSize, long totalBytesRead)
        {
            if (fluentHttpResponse == null)
                throw new ArgumentNullException("fluentHttpResponse");

            _fluentHttpResponse = fluentHttpResponse;
            _buffer = buffer;
            _bufferSize = bufferSize;
            _totalBytesRead = totalBytesRead;
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

        public string GetString(Encoding encoding, int index, int count)
        {
            return encoding.GetString(Buffer, index, count);
        }

        public string GetString(Encoding encoding)
        {
            return GetString(encoding, 0, BufferSize);
        }

        public string GetString(int index, int count)
        {
            var encoding = Encoding.GetEncoding(CharacterSet);
            return GetString(encoding, index, count);
        }

        public string GetString()
        {
            return GetString(0, BufferSize);
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

        public string CharacterSet
        {
            get { return _fluentHttpResponse.CharacterSet; }
        }

        public HttpStatusCode StatusCode
        {
            get { return _fluentHttpResponse.StatusCode; }
        }

    }
}