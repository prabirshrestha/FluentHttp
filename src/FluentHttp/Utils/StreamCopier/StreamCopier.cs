namespace FluentHttp
{
    using System;
    using System.IO;

    public class StreamCopier
    {
        private StreamCopierAsyncResult _asyncResult;

        private readonly Stream _source;
        private readonly Stream _destination;
        private readonly int _bufferSize;

        public StreamCopier(Stream source, Stream destination, int bufferSize)
        {
            // TODO: asset buffer size and streams
            _source = source;
            _destination = destination;
            _bufferSize = bufferSize;
        }

        public int BufferSize
        {
            get { return _bufferSize; }
        }

        public Stream Destination
        {
            get { return _destination; }
        }

        public Stream Source
        {
            get { return _source; }
        }

        public IAsyncResult BeginCopy(AsyncCallback callback, object state)
        {
            //lock (this)
            //{
            if (_asyncResult != null)
                throw new InvalidOperationException("Stream copying has already started.");

            _asyncResult = new StreamCopierAsyncResult(callback, state);
            var streamState = new StreamCopierState(BufferSize, _asyncResult);
            _asyncResult.StreamCopierState = streamState;

            //}

            var onStart = OnStart;
            if (onStart != null)
                onStart(this, new StreamCopyEventArgs(this, null, false));

            // http://msdn.microsoft.com/en-us/magazine/cc337900.aspx
            var buffer = new byte[BufferSize];

            Action<Exception, bool> done =
                (ex, isCancelled) =>
                {
                    //lock (this)
                    //{
                    _asyncResult.Complete();
                    _asyncResult = null;
                    //}

                    var onCompleted = OnCompleted;
                    if (onCompleted != null)
                    {
                        onCompleted(this, new StreamCopyEventArgs(this, ex, isCancelled));
                    }
                };

            AsyncCallback rc = null;
            rc = readResult =>
            {
                try
                {
                    int read = Source.EndRead(readResult);
                    if (read > 0)
                    {
                        var onRead = OnRead;
                        if (onRead != null)
                        {
                            var e = new StreamCopyEventArgs(this, null, false) { Buffer = buffer, ActualBufferSize = read };
                            onRead(this, e);
                            if (e.Cancel)
                            {
                                done(null, true);
                                return;
                            }
                        }

                        Destination.BeginWrite(
                            buffer, 0, read,
                            writeResult =>
                            {
                                try
                                {
                                    Destination.EndWrite(writeResult);
                                    Destination.Flush(); // Will block until data is sent: http://efreedom.com/Question/1-2529558/Silverlight-RC-File-Upload-Upload-Progress 
                                    Source.BeginRead(buffer, 0, buffer.Length, rc, streamState);
                                }
                                catch (Exception ex)
                                {
                                    done(ex, false);
                                }
                            },
                            streamState);
                    }
                    else
                    {
                        done(null, false);
                    }
                }
                catch (Exception ex)
                {
                    done(ex, false);
                }
            };

            Source.BeginRead(buffer, 0, buffer.Length, rc, streamState);

            return _asyncResult;
        }

        public void EndCopy(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            var ar = asyncResult as StreamCopierAsyncResult;

            if (ar == null || !ReferenceEquals(_asyncResult, ar))
                throw new ArgumentException("asyncResult");

            // propagate the exception to the one who calls EndRequest
            if (ar.StreamCopierState.Exception != null)
            {
                throw ar.StreamCopierState.Exception;
            }

            ar.AsyncWaitHandle.WaitOne();
        }

        public event EventHandler<StreamCopyEventArgs> OnStart;

        public event EventHandler<StreamCopyEventArgs> OnCompleted;

        public event EventHandler<StreamCopyEventArgs> OnRead;
    }
}