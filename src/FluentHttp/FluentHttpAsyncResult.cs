namespace FluentHttp
{
    using System;
    using System.Threading;

    public class FluentHttpAsyncResult : IAsyncResult
    {
        private AsyncCallback _callback;
        private object _state;

        private ManualResetEvent _waitHandle;

        public FluentHttpAsyncResult(AsyncCallback callback, object state)
        {
            _callback = callback;
            _state = state;
            _waitHandle = new ManualResetEvent(false);
        }

        public bool IsCompleted
        {
            get { return _waitHandle.WaitOne(0, false); }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return _waitHandle; }
        }

        public object AsyncState
        {
            get { return _state; }
            protected internal set { _state = value; }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        internal void Complete()
        {
            _waitHandle.Set();

            if (HttpRequestState != null)
            {
                // don't set the http request state to null.
                // EndRequest method still needs to access it.
                HttpRequestState.Dispose();
            }

            if (_callback != null)
                _callback(this);
        }

        internal HttpRequestState HttpRequestState { get; set; }
    }
}