namespace FluentHttp
{
    using System;
    using System.IO;

    class HttpRequestBody
    {
        private readonly Stream _stream;
        private readonly long _length;
        private readonly bool _autoDispose;

        public HttpRequestBody(Stream stream, long length, bool autoDispose)
        {
            if (!stream.CanRead)
                throw new ArgumentNullException("Stream is not readable");

            _stream = stream;

            if (length == 0)
                length = stream.Length;

            _length = length;
            _autoDispose = autoDispose;
        }

        public HttpRequestBody(Stream stream, long lenght)
            : this(stream, lenght, true)
        {
        }

        public HttpRequestBody(Stream stream)
            : this(stream, 0)
        {

        }

        public bool AutoDispose
        {
            get { return _autoDispose; }
        }

        public long Length
        {
            get { return _length; }
        }

        public Stream Stream
        {
            get { return _stream; }
        }
    }
}