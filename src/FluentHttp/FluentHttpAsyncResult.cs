namespace FluentHttp
{
    using System;
    using System.Threading;

    /// <summary>
    /// Represents the fluent http async result.
    /// </summary>
    public class FluentHttpAsyncResult : IAsyncResult
    {
        /// <summary>
        /// The fluent http request
        /// </summary>
        private readonly FluentHttpRequest _request;

        /// <summary>
        /// The wait handle.
        /// </summary>
        private readonly ManualResetEvent _waitHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpAsyncResult"/> class.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        public FluentHttpAsyncResult(FluentHttpRequest request)
        {
            _request = request;
            _waitHandle = new ManualResetEvent(false);
        }

        /// <summary>
        /// Gets the fluent http request.
        /// </summary>
        public FluentHttpRequest Request
        {
            get { return _request; }
        }

        /// <summary>
        /// Gets or sets the fluent http response.
        /// </summary>
        public FluentHttpResponse Response { get; internal set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        internal Exception Exception { get; set; }

        public bool IsCompleted { get; internal set; }

        public WaitHandle AsyncWaitHandle
        {
            get { return _waitHandle; }
        }

        public object AsyncState { get; internal set; }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        internal void SetAsyncWaitHandle()
        {
            _waitHandle.Set();
        }
    }
}