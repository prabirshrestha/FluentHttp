namespace FluentHttp
{
    using System;
    using System.IO;

    internal class ResponseReceivedEventArgs : EventArgs
    {
        private readonly IHttpWebResponse _response;

        public ResponseReceivedEventArgs(IHttpWebResponse response)
        {
            _response = response;
        }

        public IHttpWebResponse Response
        {
            get { return _response; }
        }

        public Stream ResponseSaveStream { get; set; }
    }
}