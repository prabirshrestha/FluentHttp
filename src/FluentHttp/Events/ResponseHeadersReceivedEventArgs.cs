namespace FluentHttp
{
    using System;
    using System.Diagnostics.Contracts;
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
        private readonly FluentHttpResponse fluentHttpResponse;

        private readonly byte[] buffer;

        private readonly int bufferSize;

        private readonly long totalBytesRead;

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

            this.fluentHttpResponse = fluentHttpResponse;
            this.buffer = buffer;
            this.bufferSize = bufferSize;
            this.totalBytesRead = totalBytesRead;
        }

        public object UserState { get; set; }

        public int BufferSize
        {
            get { return this.bufferSize; }
        }

        /// <summary>
        /// Gets the buffer read.
        /// </summary>
        public byte[] Buffer
        {
            get { return this.buffer; }
        }

        public string GetString(Encoding encoding, int index, int count)
        {
            Contract.Requires(encoding != null);

            return encoding.GetString(Buffer, index, count);
        }

        public string GetString(Encoding encoding)
        {
            Contract.Requires(encoding != null);
            return GetString(encoding, 0, BufferSize);
        }



        public string GetString(int index, int count)
        {
#if !SILVERLIGHT
            var encoding = Encoding.GetEncoding(CharacterSet);
#else
            var encoding = Encoding.UTF8;
#endif
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
            get { return this.fluentHttpResponse; }
        }

        public long ContentLength
        {
            get { return this.fluentHttpResponse.ContentLength; }
        }

        public string ContentType
        {
            get { return this.fluentHttpResponse.ContentType; }
        }

#if !SILVERLIGHT

        public string CharacterSet
        {
            get { return this.fluentHttpResponse.CharacterSet; }
        }

#endif

        public HttpStatusCode StatusCode
        {
            get { return this.fluentHttpResponse.StatusCode; }
        }

    }
}