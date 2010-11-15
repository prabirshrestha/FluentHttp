namespace FluentHttp
{
    using System;

    public class StreamCopyEventArgs : EventArgs
    {
        private readonly StreamCopier _streamCopier;
        private readonly Exception _exception;

        public StreamCopyEventArgs(StreamCopier streamCopier, Exception exception)
        {
            _streamCopier = streamCopier;
            _exception = exception;
        }

        public Exception Exception
        {
            get { return _exception; }
        }

        public StreamCopier StreamCopier
        {
            get { return _streamCopier; }
        }

    }
}