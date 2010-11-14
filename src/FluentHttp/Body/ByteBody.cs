namespace FluentHttp
{
    using System.Text;

    class HttpByteBody : IHttpBody
    {
        private readonly byte[] _bytes;

        public HttpByteBody(byte[] bytes)
        {
            _bytes = bytes;
        }

        public HttpByteBody(string contents, Encoding encoding)
            : this(encoding.GetBytes(contents))
        {
        }

        public HttpByteBody(string contents)
            : this(contents, Encoding.UTF8)
        {
        }

        public byte[] Bytes
        {
            get { return _bytes; }
        }
    }
}