namespace FluentHttp
{
    using System;
    using System.IO.Compression;
    using System.Net;

    /// <summary>
    /// Fluent Http Wrapper
    /// </summary>
    public partial class FluentHttpRequest
    {
        private FluentHttpAsyncResult _asyncResult;

        public IAsyncResult BeginRequest(AsyncCallback callback, object state)
        {
            if (_asyncResult != null)
                throw new InvalidOperationException("Request has already started.");

            _asyncResult = new FluentHttpAsyncResult(callback, state);

            if (Executing != null)
            {
                var executingEventArgs = new ExecutingEventArgs(this, state);
                Executing(this, executingEventArgs);
            }

            AuthenticateIfRequried();

            var request = CreateHttpWebRequest(this);

            var requestState = new HttpRequestState(GetBufferSize(), _asyncResult)
                                   {
                                       Request = this,
                                       HttpWebRequest = request
                                   };

            _asyncResult.HttpRequestState = requestState;

            request.BeginGetResponse(GetResponseCallback, requestState);

            return _asyncResult;
        }

        public FluentHttpResponse EndRequest(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            var ar = asyncResult as FluentHttpAsyncResult;

            if (ar == null || !object.ReferenceEquals(_asyncResult, ar))
                throw new ArgumentException("asyncResult");

            _asyncResult = null;

            // propagate the exception to the one who calls EndRequest.
            if (ar.HttpRequestState.Exception != null)
            {
                ar.HttpRequestState.Response.ResponseStatus = ResponseStatus.Error;
                throw ar.HttpRequestState.Exception;
            }

            ar.AsyncWaitHandle.WaitOne();

            return ar.HttpRequestState.Response;
        }

        private void GetResponseCallback(IAsyncResult asyncResult)
        {
            var requestState = (HttpRequestState)asyncResult.AsyncState;
            var request = requestState.HttpWebRequest;

            WebResponse response;

            try
            {
                response = request.EndGetResponse(asyncResult);

                // TODO: better to do this in different method
                requestState.HttpWebResponse = (HttpWebResponse)response;
                var fluentHttpResponse = new FluentHttpResponse(this, requestState.HttpWebResponse);

                requestState.Response = fluentHttpResponse;
                requestState.TotalBytes = response.ContentLength;

                NotifyHeadersReceived(fluentHttpResponse, requestState);

                ReadResponseStream(requestState);

            }
            catch (WebException ex)
            {
                // handle web exception
                requestState.HttpWebResponse = (HttpWebResponse)ex.Response;

                var fluentHttpResponse = new FluentHttpResponse(this, requestState.HttpWebResponse);
                requestState.Response = fluentHttpResponse;
                // don't set the requestState.Exception, that exception is for something else besides WebException.

                requestState.Response = fluentHttpResponse;
                requestState.TotalBytes = fluentHttpResponse.ContentLength;

                NotifyHeadersReceived(fluentHttpResponse, requestState);
                ReadResponseStream(requestState);
            }
        }

        private void ReadResponseStream(HttpRequestState requestState)
        {
            var stream = requestState.HttpWebResponse.GetResponseStream();

            if (stream == null)
                return;

            requestState.Stream = stream;

            if (requestState.HttpWebResponse.ContentEncoding.Contains("gzip"))
                requestState.Stream = new GZipStream(stream, CompressionMode.Decompress);
            else if (requestState.HttpWebResponse.ContentEncoding.Contains("deflate"))
                requestState.Stream = new DeflateStream(stream, CompressionMode.Decompress);

            Read(requestState);
        }

        private void NotifyHeadersReceived(FluentHttpResponse fluentHttpResponse, HttpRequestState requestState)
        {
            var fluentHttpRequest = fluentHttpResponse.Request;

            if (fluentHttpRequest.ResponseHeadersReceived == null)
                return;

            try
            {
                var responseHeadersReceivedEventArgs = new ResponseHeadersReceivedEventArgs(fluentHttpResponse,
                                                                                            requestState.AsyncResult.
                                                                                                AsyncState);
                fluentHttpRequest.ResponseHeadersReceived(this, responseHeadersReceivedEventArgs);
            }
            catch (Exception ex)
            {
                // we need to catch the user exception so that we can end the request.
                requestState.Exception = ex;
                Complete();
            }
        }

        /// <summary>
        /// Read stream asynchronously.
        /// </summary>
        /// <param name="requestState">
        /// The request state.
        /// </param>
        private void Read(HttpRequestState requestState)
        {
            var stream = requestState.Stream;
            while (true)
            {
                requestState.Buffer = new byte[requestState.BufferSize];
                var result = stream.BeginRead(requestState.Buffer, 0, requestState.BufferSize,
                                              ReadResponseStreamCallback, requestState);

                if (!result.CompletedSynchronously)
                    return; // handled by callback
                if (!EndRead(result))
                    break;
            }
        }

        public void ReadResponseStreamCallback(IAsyncResult asyncResult)
        {
            if (asyncResult.CompletedSynchronously)
                return;
            if (EndRead(asyncResult))
            {
                var requestState = (HttpRequestState)asyncResult.AsyncState;
                Read(requestState);
            }
        }

        /// <summary>
        /// Returns false if there is no more data to read.
        /// </summary>
        /// <param name="asyncResult">
        /// The async result.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private bool EndRead(IAsyncResult asyncResult)
        {
            var requestState = (HttpRequestState)asyncResult.AsyncState;
            int chunkSize = requestState.Stream.EndRead(asyncResult);
            requestState.BytesRead += chunkSize;
            var canRead = chunkSize > 0 && requestState.Stream.CanRead;

            var fluentRequest = requestState.Request;

            if (fluentRequest.ResponseRead != null)
            {
                try
                {
                    var responseReadEventArgs = new ResponseReadEventArgs(requestState.Response, requestState.Buffer,
                                                                          chunkSize, requestState.BytesRead, canRead) { UserState = requestState.AsyncResult.AsyncState };
                    fluentRequest.ResponseRead(requestState.Stream, responseReadEventArgs);
                    requestState.AsyncResult.AsyncState = responseReadEventArgs.UserState;
                }
                catch (Exception ex)
                {
                    // we need to catch the user exception so that we can end the request.
                    requestState.Exception = ex;
                    Complete();
                }
            }

            if (!canRead)
            {
                requestState.Response.ResponseStatus = ResponseStatus.Completed;

                if (fluentRequest.Completed != null)
                {
                    try
                    {
                        var completedEventArgs = new CompletedEventArgs(requestState.Response,
                                                                        requestState.AsyncResult.AsyncState);
                        fluentRequest.Completed(null, completedEventArgs);
                    }
                    catch (Exception ex)
                    {
                        // we need to catch the user exception so that we can end the request.
                        requestState.Exception = ex;
                        Complete();
                    }
                }
                Complete();
            }

            return canRead;
        }

        private void Complete()
        {
            _asyncResult.Complete();
            _asyncResult = null;
            // more cleanups
        }

    }
}