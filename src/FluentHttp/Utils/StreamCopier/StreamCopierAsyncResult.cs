namespace FluentHttp
{
    using System;
    using System.Threading;

    class StreamCopierAsyncResult : IAsyncResult
    {
        private AsyncCallback _callback;
        private object _state;

        private ManualResetEvent _waitHandle;

        public StreamCopierAsyncResult(AsyncCallback callback, object state)
        {
            _callback = callback;
            _state = state;
            _waitHandle = new ManualResetEvent(false);
        }

        /// <summary>
        /// Gets a value that indicates whether the asynchronous operation has completed.
        /// </summary>
        /// <returns>
        /// true if the operation is complete; otherwise, false.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public bool IsCompleted
        {
            get
            {
#if SILVERLIGHT
                return _waitHandle.WaitOne(0);
#else
                return _waitHandle.WaitOne(0, false);
#endif
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public WaitHandle AsyncWaitHandle
        {
            get { return _waitHandle; }
        }

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
        /// </summary>
        /// <returns>
        /// A user-defined object that qualifies or contains information about an asynchronous operation.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object AsyncState
        {
            get { return _state; }
            protected internal set { _state = value; }
        }

        /// <summary>
        /// Gets a value that indicates whether the asynchronous operation completed synchronously.
        /// </summary>
        /// <returns>
        /// true if the asynchronous operation completed synchronously; otherwise, false.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public bool CompletedSynchronously
        {
            get { return false; }
        }

        internal void Complete()
        {
            _waitHandle.Set();

            if (_callback != null)
                _callback(this);
        }

        internal StreamCopierState StreamCopierState { get; set; }
    }
}