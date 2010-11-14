namespace FluentHttp
{
    using System;
    using System.IO;
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
                var executingEventArgs = new ExecutingEventArgs(this) { UserState = state };
                Executing(this, executingEventArgs);
                _asyncResult.AsyncState = executingEventArgs.UserState;
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

            ar.AsyncWaitHandle.WaitOne();
            _asyncResult = null;

            return ar.HttpRequestState.Response;
        }

        private void GetResponseCallback(IAsyncResult asyncResult)
        {
            var requestState = (HttpRequestState)asyncResult.AsyncState;
            var request = requestState.HttpWebRequest;
            var fluentHttpRequest = requestState.Request;

            WebResponse response;
            Stream stream;

            try
            {
                using (response = request.EndGetResponse(asyncResult))
                {
                    var fluentHttpResponse = new FluentHttpResponse
                                                 {
                                                     ContentLength = response.ContentLength,
                                                     ContentType = response.ContentType,
                                                     Headers = response.Headers
                                                 };
                    requestState.Response = fluentHttpResponse;
                    requestState.TotalBytes = response.ContentLength;

                    // decompress if compressed.
                    if (fluentHttpRequest.ResponseHeadersReceived != null)
                    {
                        var responseHeadersReceivedEventArgs = new ResponseHeadersReceivedEventArgs(fluentHttpResponse) { UserState = requestState.AsyncResult.AsyncState };
                        fluentHttpRequest.ResponseHeadersReceived(request, responseHeadersReceivedEventArgs);
                        requestState.AsyncResult.AsyncState = responseHeadersReceivedEventArgs.UserState;
                    }

                    using (stream = response.GetResponseStream())
                    {
                        if (stream == null)
                            return;
                        requestState.Stream = stream;

                        Read(requestState);
                    }
                }
            }
            catch (WebException ex)
            {
                // handle web exception
                throw;
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
            try
            {
                if (asyncResult.CompletedSynchronously)
                    return;
                if (EndRead(asyncResult))
                {
                    var requestState = (HttpRequestState)asyncResult.AsyncState;
                    Read(requestState);
                }
            }
            catch (Exception)
            {
                throw;
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
            var canRead = chunkSize > 0 && requestState.BytesRead < requestState.TotalBytes;

            var fluentRequest = requestState.Request;

            if (fluentRequest.ResponseRead != null)
            {
                var responseReadEventArgs = new ResponseReadEventArgs(requestState.Response, requestState.Buffer,
                                                                      chunkSize, requestState.BytesRead, canRead) { UserState = requestState.AsyncResult.AsyncState };
                fluentRequest.ResponseRead(requestState.Stream, responseReadEventArgs);
                requestState.AsyncResult.AsyncState = responseReadEventArgs.UserState;
            }

            if (!canRead)
            {
                if (fluentRequest.Completed != null)
                {
                    var completedEventArgs = new CompletedEventArgs(requestState.Response) { UserState = requestState.AsyncResult.AsyncState };
                    fluentRequest.Completed(null, completedEventArgs);
                    requestState.AsyncResult.AsyncState = completedEventArgs.UserState;
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