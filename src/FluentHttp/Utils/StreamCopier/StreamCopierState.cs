namespace FluentHttp
{
    using System;

    class StreamCopierState : IDisposable
    {
        private readonly int _bufferSize;
        private readonly StreamCopierAsyncResult _streamCopierAsyncResult;

        public StreamCopierState(int bufferSize, StreamCopierAsyncResult streamCopierAsyncResult)
        {
            _bufferSize = bufferSize;
            _streamCopierAsyncResult = streamCopierAsyncResult;
        }

        public StreamCopierAsyncResult StreamCopierAsyncResult
        {
            get { return _streamCopierAsyncResult; }
        }

        public int BufferSize
        {
            get { return _bufferSize; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
        }

        public Exception Exception { get; set; }
    }
}