namespace FluentHttp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.IO;

    public class StreamCopier
    {
        private StreamCopierAsyncResult _asyncResult;

        private readonly Stream source;
        private readonly Stream destination;
        private readonly int bufferSize;

        public StreamCopier(Stream source, Stream destination, int bufferSize)
        {
            Contract.Requires(bufferSize >= 1);

            // TODO: asset buffer size and source stream.
            // note: allow destination to be null, incase the user doesn't want to write
            // and just read.
            this.source = source;
            this.destination = destination;
            this.bufferSize = bufferSize;
        }

        public int BufferSize
        {
            get
            {
                return bufferSize;
            }
        }

        public Stream Destination
        {
            get { return destination; }
        }

        public Stream Source
        {
            get { return source; }
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
                    _asyncResult.StreamCopierState.Exception = ex;
                    _asyncResult.Complete();
                    _asyncResult = null;
                    //}

                    if (ex == null)
                    {
                        var onCompleted = OnCompleted;
                        if (onCompleted != null)
                        {
                            onCompleted(this, new StreamCopyEventArgs(this, ex, isCancelled));
                        }
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
                            e.BytesRead += read;
                            onRead(this, e);
                            if (e.Cancel)
                            {
                                done(null, true);
                                return;
                            }
                        }

                        // if destination is null, just read
                        if (Destination == null)
                        {
                            try
                            {
                                Source.BeginRead(buffer, 0, buffer.Length, rc, streamState);
                            }
                            catch (Exception ex)
                            {
                                done(ex, false);
                            }
                        }
                        else
                        {
                            Destination.BeginWrite(
                                buffer, 0, read,
                                writeResult =>
                                {
                                    try
                                    {
                                        Destination.EndWrite(writeResult);
                                        Destination.Flush();
                                        // Will block until data is sent: http://efreedom.com/Question/1-2529558/Silverlight-RC-File-Upload-Upload-Progress 
                                        Source.BeginRead(buffer, 0, buffer.Length, rc, streamState);
                                    }
                                    catch (Exception ex)
                                    {
                                        done(ex, false);
                                    }
                                },
                                streamState);
                        }
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

        /// <summary>
        /// On copy start.
        /// </summary>
        public event EventHandler<StreamCopyEventArgs> OnStart;

        /// <summary>
        /// On copy completed.
        /// </summary>
        public event EventHandler<StreamCopyEventArgs> OnCompleted;

        /// <summary>
        /// On read.
        /// </summary>
        public event EventHandler<StreamCopyEventArgs> OnRead;

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
           Justification = "Reviewed. Suppression is OK here."), ContractInvariantMethod]
        private void InvarianObject()
        {
            Contract.Invariant(this.bufferSize >= 1);
        }
    }
}