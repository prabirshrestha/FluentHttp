namespace FluentHttp
{
    using System;
    using System.IO;
    using System.Net;

    public delegate void FluentHttpCallback(
        FluentHttpRequest fluentHttpRequest, FluentHttpResponse fluentHttpResponse, object userState);

    class HttpRequestState
    {
        public int BufferSize { get; private set; }

        public HttpRequestState(int bufferSize)
        {
            BufferSize = bufferSize;
        }

        public long TotalBytes { get; set; }

        /// <summary>
        /// Gets or sets the total bytes read during current transfer.
        /// </summary>
        public long BytesRead { get; set; }

        /// <summary>
        /// Gets or sets buffer to read data into.
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// Gets or sets the date and time the transfer started.
        /// </summary>
        public DateTime TransferStartedDate { get; set; }

        public FluentHttpRequest Request { get; set; }

        public FluentHttpResponse Response { get; set; }

        public Stream Stream { get; set; }

        public HttpWebRequest HttpWebRequest { get; set; }

        public AsyncCallback AsynCallback { get; set; }

        public object UserState { get; set; }
    }
}