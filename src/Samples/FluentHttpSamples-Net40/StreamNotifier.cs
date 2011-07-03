using System;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace FluentHttpSamples
{
    public class WriteProgressChangedEventArgs : ProgressChangedEventArgs
    {
        // https://raw.github.com/mono/mono/master/mcs/class/System/System.Net/DownloadProgressChangedEventArgs.cs
        internal WriteProgressChangedEventArgs(long bytesReceived, long totalBytesToReceive, object userState)
            : base(totalBytesToReceive != -1 ? ((int)(bytesReceived * 100.0 / totalBytesToReceive)) : 0, userState)
        {
            this.received = bytesReceived;
            this.total = totalBytesToReceive;
        }

        long received, total;

        public long BytesReceived
        {
            get { return received; }
        }

        public long TotalBytesToReceive
        {
            get { return total; }
        }

    }

    public delegate void WriteProgressChangedEventHandler(object sender, WriteProgressChangedEventArgs e);

    public class StreamNotifier : Stream
    {
        private readonly Stream _stream;
        private readonly long _length;
        private readonly object _userToken;

        public StreamNotifier(Stream stream, long length, object userToken)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            _stream = stream;
            _length = length;
            _userToken = userToken;
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        private long notify_total;
        public override void Write(byte[] buffer, int offset, int count)
        {
            notify_total += count;
            OnWriteProgressChanged(new WriteProgressChangedEventArgs(notify_total, _length, _userToken));
            _stream.Write(buffer, offset, count);
        }

        public event WriteProgressChangedEventHandler WriteProgressChanged;

        protected virtual void OnWriteProgressChanged(WriteProgressChangedEventArgs e)
        {
            if (WriteProgressChanged != null)
                WriteProgressChanged(this, e);
        }

        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

        public override long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }
    }
}