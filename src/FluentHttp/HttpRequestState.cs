namespace FluentHttp
{
    using System;
    using System.IO;

    public delegate void FluentHttpResponseHeaderRecieved(FluentHttpResponse fluentHttpResponse);

    public delegate void FluentHttpResponseProgressChanged(
        FluentHttpResponse fluentHttpResponse, int totalBytes, double percentageComplete, double transferRate);

    public delegate void FluentHttpResponseCompleted(FluentHttpResponse fluentHttpResponse);

    public class HttpRequestState
    {
        public HttpRequestState(int bufferSize)
        {
            BufferRead = new byte[bufferSize];
        }

        public long TotalBytes { get; set; }

        /// <summary>
        /// Gets or sets the total bytes read during current transfer.
        /// </summary>
        public long BytesRead { get; set; }

        /// <summary>
        /// Gets or sets delta % for each buffer read.
        /// </summary>
        public double ProgressIncrement { get; set; }

        /// <summary>
        /// Gets or sets buffer to read data into.
        /// </summary>
        public byte[] BufferRead { get; set; }

        /// <summary>
        /// Gets or sets the date and time the transfer started.
        /// </summary>
        public DateTime TransferStartedDate { get; set; }

        public FluentHttpRequest Request { get; set; }

        public FluentHttpResponse Response { get; set; }

        public Stream StreamResponse { get; set; }

        public FluentHttpResponseHeaderRecieved FluentHttpResponseHeaderRecieved;
        public FluentHttpResponseProgressChanged FluentHttpResponseProgressChanged;
        public FluentHttpResponseCompleted FluentHttpResponseCompleted;


    }
}