namespace FluentHttp
{
    using System.IO;

    class StreamHttpBody : IHttpRequestBody
    {
        private readonly Stream stream;

        public StreamHttpBody(Stream stream)
        {
            this.stream = stream;
        }

        public Stream Stream
        {
            get { return this.stream; }
        }

        public long ContentLength
        {
            get { return this.stream.Length; }
        }

        public bool AutoDisposeStream { get; set; }

        public string ContentType
        {
            get { return "application/x-www-form-urlencoded"; }
        }
    }
}