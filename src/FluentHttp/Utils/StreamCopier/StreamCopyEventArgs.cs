namespace FluentHttp
{
    using System;

    public class StreamCopyEventArgs : EventArgs
    {
        private readonly StreamCopier _streamCopier;
        private readonly Exception _exception;
        private readonly bool _isCancelled;

        public StreamCopyEventArgs(StreamCopier streamCopier, Exception exception, bool isCancelled)
        {
            _streamCopier = streamCopier;
            _exception = exception;
            _isCancelled = isCancelled;
        }

        public bool IsCanceled
        {
            get { return _isCancelled; }
        }

        public Exception Exception
        {
            get { return _exception; }
        }

        public StreamCopier StreamCopier
        {
            get { return _streamCopier; }
        }

        public byte[] Buffer;
        public int ActualBufferSize;
        public long BytesRead;

        public bool Cancel;
        public bool IsCancelled;

    }
}