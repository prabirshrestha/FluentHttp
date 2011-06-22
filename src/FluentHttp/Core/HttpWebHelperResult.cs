using System;
using System.Threading;
using System.IO;

namespace FluentHttp
{
    internal class HttpWebHelperResult : IAsyncResult
    {
        private readonly IHttpWebRequest _httpWebRequest;
        private readonly IHttpWebResponse _httpWebResponse;
        private readonly Exception _exception;
        private readonly Exception _innerException;
        private readonly bool _isCancelled;
        private readonly bool _completeSynchronsouly;
        private readonly Stream _responseSaveStream;
        private readonly object _state;

        public HttpWebHelperResult(IHttpWebRequest httpWebRequest, IHttpWebResponse httpWebResponse, Exception exception, Exception innerException, bool isCancelled, bool completeSynchronsouly, Stream responseSaveStream, object state)
        {
            _httpWebRequest = httpWebRequest;
            _httpWebResponse = httpWebResponse;
            _exception = exception;
            _innerException = innerException;
            _isCancelled = isCancelled;
            _completeSynchronsouly = completeSynchronsouly;
            _responseSaveStream = responseSaveStream;
            _state = state;
        }

        public Exception InnerException
        {
            get { return _innerException; }
        }

        public bool IsCancelled
        {
            get { return _isCancelled; }
        }

        public IHttpWebRequest HttpWebRequest
        {
            get { return _httpWebRequest; }
        }

        public IHttpWebResponse HttpWebResponse
        {
            get { return _httpWebResponse; }
        }

        public Stream ResponseSaveStream
        {
            get { return _responseSaveStream; }
        }

        public Exception Exception
        {
            get { return _exception; }
        }

        public bool IsCompleted
        {
            get { return true; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return null; }
        }

        public object AsyncState
        {
            get { return _state; }
        }

        public bool CompletedSynchronously
        {
            get { return _completeSynchronsouly; }
        }
    }
}