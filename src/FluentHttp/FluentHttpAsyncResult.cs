
namespace FluentHttp
{
    using System;
    using System.Threading;
    using System.IO;

    public delegate void FluentHttpCallback(FluentHttpAsyncResult asyncResult);

    /// <summary>
    /// Represents the fluent http async result.
    /// </summary>
    public class FluentHttpAsyncResult : IAsyncResult
    {
        /// <summary>
        /// The fluent http request
        /// </summary>
        private readonly FluentHttpRequest _request;
        private readonly FluentHttpResponse _response;
        private readonly object _asyncState;
        private readonly WaitHandle _asyncWaitHandle;
        private readonly bool _completedSynchronously;
        private readonly bool _isCompleted;
        private readonly bool _isCancelled;
        private readonly Exception _exception;
        private readonly Exception _innerException;

        public FluentHttpAsyncResult(FluentHttpRequest request, FluentHttpResponse response, object asyncState, WaitHandle asyncWaitHandle, bool completedSynchronously, bool isCompleted, bool isCancelled, Exception exception, Exception innerException)
        {
            _request = request;
            _response = response;
            _asyncState = asyncState;
            _asyncWaitHandle = asyncWaitHandle;
            _completedSynchronously = completedSynchronously;
            _isCompleted = isCompleted;
            _isCancelled = isCancelled;
            _exception = exception;
            _innerException = innerException;
        }

        public Exception InnerException
        {
            get { return _innerException; }
        }
            
        public FluentHttpResponse Response
        {
            get { return _response; }
        }

        public FluentHttpRequest Request
        {
            get { return _request; }
        }

        public bool IsCancelled
        {
            get { return _isCancelled; }
        }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get { return _exception; } }

        public bool IsCompleted { get { return _isCompleted; } }

        public WaitHandle AsyncWaitHandle
        {
            get { return _asyncWaitHandle; }
        }

        public object AsyncState
        {
            get { return _asyncState; }
        }

        public bool CompletedSynchronously
        {
            get { return _completedSynchronously; }
        }
    }
}