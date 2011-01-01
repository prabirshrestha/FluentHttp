namespace FluentHttp
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Muti Read Stream
    /// </summary>
    /// <remarks>
    /// http://www.c-sharpcorner.com/uploadfile/sergey%20shirokov/multistreamcs11232005012800am/multistreamcs.aspx
    /// TODO: add auto dispose streams funtionality
    /// </remarks>
    class MultiReadStream : Stream
    {
        private List<Stream> streamList = new List<Stream>();

        private long position;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <returns>
        /// true if the stream supports reading; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <returns>
        /// true if the stream supports seeking; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <returns>
        /// true if the stream supports writing; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <returns>
        /// A long value representing the length of the stream in bytes.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. 
        ///                 </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. 
        /// </exception><filterpriority>1</filterpriority>
        public override long Length
        {
            get
            {
                long result = 0;
                foreach (Stream stream in streamList)
                {
                    result += stream.Length;
                }
                return result;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <returns>
        /// The current position within the stream.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. 
        ///                 </exception><exception cref="T:System.NotSupportedException">The stream does not support seeking. 
        ///                 </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. 
        /// </exception><filterpriority>1</filterpriority>
        public override long Position
        {
            get { return position; }
            set { Seek(value, SeekOrigin.Begin); }
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. 
        /// </exception><filterpriority>2</filterpriority>
        public override void Flush()
        {
            // TODO: need to implement flush.
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter. 
        /// </param><param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position. 
        /// </param><exception cref="T:System.IO.IOException">An I/O error occurs. 
        /// </exception><exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output. 
        /// </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. 
        /// </exception><filterpriority>1</filterpriority>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long len = Length;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    position = offset;
                    break;
                case SeekOrigin.Current:
                    position += offset;
                    break;
                case SeekOrigin.End:
                    position = len + offset;
                    break;
            }
            if (position > len)
            {
                position = len;
            }
            else if (position < 0)
            {
                position = 0;
            }
            return position;
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes. 
        /// </param><exception cref="T:System.IO.IOException">An I/O error occurs. 
        /// </exception><exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output. 
        /// </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. 
        /// </exception><filterpriority>2</filterpriority>
        public override void SetLength(long value) { }

        /// <summary>
        /// Adds the stream
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        public void AddStream(Stream stream)
        {
            streamList.Add(stream);
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source. 
        /// </param><param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream. 
        /// </param><param name="count">The maximum number of bytes to be read from the current stream. 
        /// </param><exception cref="T:System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length. 
        /// </exception><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null. 
        /// </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative. 
        /// </exception><exception cref="T:System.IO.IOException">An I/O error occurs. 
        /// </exception><exception cref="T:System.NotSupportedException">The stream does not support reading. 
        /// </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. 
        /// </exception><filterpriority>1</filterpriority>
        public override int Read(byte[] buffer, int offset, int count)
        {
            long len = 0;
            int result = 0;
            int buf_pos = offset;
            int bytesRead;
            foreach (Stream stream in streamList)
            {
                if (position < (len + stream.Length))
                {
                    stream.Position = position - len;
                    bytesRead = stream.Read(buffer, buf_pos, count);
                    result += bytesRead;
                    buf_pos += bytesRead;
                    position += bytesRead;
                    if (bytesRead < count)
                    {
                        count -= bytesRead;
                    }
                    else
                    {
                        break;
                    }
                }
                len += stream.Length;
            }
            return result;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new System.InvalidOperationException("Stream is not writable");
        }
    }
}