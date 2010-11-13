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
        public IAsyncResult BeginRequest(AsyncCallback callback, object userState)
        {
            if (Executing != null)
            {
                var executingEventArgs = new ExecutingEventArgs(this) { UserState = userState };
                Executing(this, executingEventArgs);
                userState = executingEventArgs.UserState;
            }

            AuthenticateIfRequried();

            var request = CreateHttpWebRequest(this);

            var requestState = new HttpRequestState(GetBufferSize())
                                   {
                                       Request = this,
                                       HttpWebRequest = request,
                                       AsynCallback = callback,
                                       UserState = userState
                                   };

            // TODO check if http method is supported

            return request.BeginGetResponse(GetResponseCallback, requestState);
        }

        public FluentHttpResponse EndRequest(IAsyncResult result)
        {
            var requestState = (HttpRequestState) result.AsyncState;
            return requestState.Response;
        }

        private static void GetResponseCallback(IAsyncResult asyncResult)
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
                        var responseHeadersReceivedEventArgs = new ResponseHeadersReceivedEventArgs(fluentHttpResponse) { UserState = requestState.UserState };
                        fluentHttpRequest.ResponseHeadersReceived(request, responseHeadersReceivedEventArgs);
                        requestState.UserState = responseHeadersReceivedEventArgs.UserState;
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
        private static void Read(HttpRequestState requestState)
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
        private static bool EndRead(IAsyncResult asyncResult)
        {
            var requestState = (HttpRequestState)asyncResult.AsyncState;
            int chunkSize = requestState.Stream.EndRead(asyncResult);
            requestState.BytesRead += chunkSize;
            var canRead = chunkSize > 0 && requestState.BytesRead < requestState.TotalBytes;

            var fluentRequest = requestState.Request;

            if (fluentRequest.ResponseRead != null)
            {
                var responseReadEventArgs = new ResponseReadEventArgs(requestState.Response, requestState.Buffer,
                                                                      chunkSize, requestState.BytesRead, canRead) { UserState = requestState.UserState };
                fluentRequest.ResponseRead(requestState.Stream, responseReadEventArgs);
                requestState.UserState = responseReadEventArgs.UserState;
            }

            if (!canRead)
            {
                if (fluentRequest.Completed != null)
                {
                    var completedEventArgs = new CompletedEventArgs(requestState.Response) { UserState = requestState.UserState };
                    fluentRequest.Completed(null, completedEventArgs);
                    requestState.UserState = completedEventArgs.UserState;
                }
            }

            return canRead;
        }

        public static void ReadResponseStreamCallback(IAsyncResult asyncResult)
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

        //private static void ResponseCallback(IAsyncResult asyncResult)
        //{
        //    var requestState = (HttpRequestState)asyncResult.AsyncState;
        //    var request = requestState.HttpWebRequest;

        //    WebResponse response = null;
        //    Stream stream = null;

        //    try
        //    {
        //        using (response = request.EndGetResponse(asyncResult))
        //        {
        //            // decompress if compressed.
        //            // TODO: notify that the header has been received.

        //        }

        //        //    var response = (HttpWebResponse)requestState.HttpWebRequest.EndGetResponse(asyncResult);
        //        //    requestState.Response = ToFluentHttpResponseAsync(response);

        //        //    // TODO notify that the header has been received.

        //        //    // now download the actual data
        //        //    var responseStream = response.GetResponseStream();
        //        //    requestState.ResponseStream = responseStream;

        //        //    // begin reading contents of the response data
        //        //    var result = responseStream.BeginRead(requestState.BufferRead, 0, requestState.BufferSize, ReadCallback,
        //        //                                          requestState);

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //private static void ReadCallback(IAsyncResult asyncResult)
        //{
        //    // start reading the actual contents data
        //    try
        //    {
        //        var requestState = (HttpRequestState)asyncResult.AsyncState;

        //        var responseStream = requestState.ResponseStream;

        //        // get get results of read operation

        //        int bytesRead = responseStream.EndRead(asyncResult);

        //        // got some data, need to read more
        //        if (bytesRead > 0)
        //        {
        //            requestState.BytesRead += bytesRead;
        //            double percentageComplete = ((double)requestState.BytesRead / requestState.TotalBytes) * 100.0f;

        //            var totalTime = DateTime.Now - requestState.TransferStartedDate;

        //            // TODO: report progress

        //            // kick of another read
        //            var result = responseStream.BeginRead(requestState.BufferRead, 0, requestState.BufferSize,
        //                                                  ReadCallback, requestState);
        //            return;
        //        }
        //        else
        //        {
        //            // EndRead returned 0, so no more data to read
        //            responseStream.Close();
        //            requestState.Response.HttpWebResponse.Close();

        //            // TODO notify completed.
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


    }
}