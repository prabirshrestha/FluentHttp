
namespace FluentHttp
{
    using System;
    using System.Threading;

    public delegate void FluentHttpCallback(FluentHttpAsyncResult asyncResult);

    /// <summary>
    /// Represents the fluent http async result.
    /// </summary>
    public class FluentHttpAsyncResult : IAsyncResult
    {
        private readonly FluentHttpRequest _request;
        private readonly FluentHttpResponse _response;
        private readonly object _asyncState;
        private readonly WaitHandle _asyncWaitHandle;
        private readonly bool _completedSynchronously;
        private readonly bool _isCompleted;
        private readonly bool _isCancelled;
        private readonly Exception _exception;
        private readonly Exception _innerException;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpAsyncResult"/> class.
        /// </summary>
        /// <param name="request">The <see cref="FluentHttpRequest"/>.</param>
        /// <param name="response">The <see cref="FluentHttpResponse"/>.</param>
        /// <param name="asyncState">The async state.</param>
        /// <param name="asyncWaitHandle">The async wait handle.</param>
        /// <param name="completedSynchronously">Indicates whether the async operation completed synchronously.</param>
        /// <param name="isCompleted">Indicates whether the async operation completed.</param>
        /// <param name="isCancelled">Indicates whether the async operation was cancelled.</param>
        /// <param name="exception">The exception during the http web request.</param>
        /// <param name="innerException">The inner exception during the http web request.</param>
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

        /// <summary>
        /// Gets the inner exception.
        /// </summary>
        public Exception InnerException
        {
            get { return _innerException; }
        }
        
        /// <summary>
        /// Gets the <see cref="FluentHttpResponse"/>.
        /// </summary>
        public FluentHttpResponse Response
        {
            get { return _response; }
        }

        /// <summary>
        /// Gets the <see cref="FluentHttpRequest"/>.
        /// </summary>
        public FluentHttpRequest Request
        {
            get { return _request; }
        }

        /// <summary>
        /// Indicates whether the request was cancelled.
        /// </summary>
        public bool IsCancelled
        {
            get { return _isCancelled; }
        }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get { return _exception; } }

        /// <summary>
        /// Indicates whether the async operation is completed.
        /// </summary>
        public bool IsCompleted { get { return _isCompleted; } }

        /// <summary>
        /// Gets the async wait handle.
        /// </summary>
        public WaitHandle AsyncWaitHandle
        {
            get { return _asyncWaitHandle; }
        }

        /// <summary>
        /// Gets the async state.
        /// </summary>
        public object AsyncState
        {
            get { return _asyncState; }
        }

        /// <summary>
        /// Indicates whether the async operation completed synchronously.
        /// </summary>
        public bool CompletedSynchronously
        {
            get { return _completedSynchronously; }
        }
    }
}