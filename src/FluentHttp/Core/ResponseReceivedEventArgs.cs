namespace FluentHttp
{
    using System;
    using System.IO;

    internal class ResponseReceivedEventArgs : EventArgs
    {
        private readonly IHttpWebResponse _response;
        private readonly Exception _exception;
        private readonly object _asyncState;

        public ResponseReceivedEventArgs(IHttpWebResponse response, Exception exception, object asyncState)
        {
            _response = response;
            _exception = exception;
            _asyncState = asyncState;
        }

        public object AsyncState
        {
            get { return _asyncState; }
        }

        public Exception Exception
        {
            get { return _exception; }
        }

        public IHttpWebResponse Response
        {
            get { return _response; }
        }

        public Stream ResponseSaveStream { get; set; }
    }
}