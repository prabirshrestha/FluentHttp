namespace FluentHttp
{
    using System;
    using System.Diagnostics.Contracts;
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
            {
                throw new InvalidOperationException("Request has already started.");
            }

            _asyncResult = new FluentHttpAsyncResult(callback, state);

            if (Executing != null)
            {
                var executingEventArgs = new ExecutingEventArgs(this, state);
                Executing(this, executingEventArgs);
            }

            AuthenticateIfRequried();

            var request = CreateHttpWebRequest(this);

            var httpBody = GetRequestBody();
            var requestState = new HttpRequestState(GetBufferSize(), _asyncResult)
                                   {
                                       Request = this,
                                       HttpWebRequest = request,
                                       RequestBody = httpBody
                                   };

            _asyncResult.HttpRequestState = requestState;

            if (httpBody == null)
            {
                ReadResponse(requestState);
            }
            else
            {
                // set the content length, content-type and boundaries 
                // and other necessary stuffs here. this has to be set
                // before the request begins.

                // TODO: support for multipart.
                if (string.IsNullOrEmpty(request.ContentType))
                {
                    request.ContentType = httpBody.ContentType;
                }

                request.ContentLength = httpBody.ContentLength;

                WriteBodyAndReadResponse(httpBody, requestState);
            }

            return _asyncResult;
        }

        private void ReadResponse(HttpRequestState requestState)
        {
            var request = requestState.HttpWebRequest;
            request.BeginGetResponse(
                ar =>
                {
                    requestState = (HttpRequestState)ar.AsyncState;

                    // EndGetResponse might throw error.
                    HttpWebResponse response;

                    try
                    {
                        response = (HttpWebResponse)request.EndGetResponse(ar);
                    }
                    catch (WebException ex)
                    {
                        response = (HttpWebResponse)ex.Response;

                        // don't set the requestState.Exception,
                        // that exception is for something else.
                    }

                    // got the response header, read it and notify
                    requestState.HttpWebResponse = response;
                    var fluentHttpResponse = new FluentHttpResponse(this, response);
                    requestState.Response = fluentHttpResponse;
                    NotifyHeadersReceived(fluentHttpResponse, requestState);

                    ReadResponseStream(requestState);
                },
                requestState);
        }

        private void ReadResponseStream(HttpRequestState requestState)
        {
            var stream = requestState.HttpWebResponse.GetResponseStream();

            if (stream == null)
            {
                return;
            }

            var contentEncoding = requestState.HttpWebResponse.ContentEncoding;

            if (contentEncoding.Contains("gzip"))
            {
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }
            else if (contentEncoding.Contains("deflate"))
            {
                stream = new DeflateStream(stream, CompressionMode.Decompress);
            }

            var destinationStream = GetSaveStream();

            var fluentRequest = requestState.Request;

            var streamCopier = new StreamCopier(stream, destinationStream, requestState.BufferSize);

            if (fluentRequest.ResponseRead != null)
            {
                streamCopier.OnRead +=
                    (o, e) =>
                    {
                        var responseReadEventArgs = new ResponseReadEventArgs(
                                                        requestState.Response,
                                                        e.Buffer,
                                                        e.ActualBufferSize,
                                                        e.BytesRead)
                                                        {
                                                            UserState = requestState.AsyncResult.AsyncState
                                                        };

                        fluentRequest.ResponseRead(requestState.Stream, responseReadEventArgs);
                        requestState.AsyncResult.AsyncState = responseReadEventArgs.UserState;
                    };
            }

            streamCopier.OnCompleted +=
                (o, e) =>
                {
                    if (e.Exception != null)
                    {
                        // if an exception occurred.
                        requestState.Exception = e.Exception;
                    }
                    else if (e.IsCancelled)
                    {
                        // if cancelled
                    }
                    else
                    {
                        // else copy completed.                        
                    }

                    // web response read completed.
                    // signal complete
                    if (fluentRequest.Completed != null)
                    {
                        try
                        {
                            var completedEventArgs = new CompletedEventArgs(
                                                            requestState.Response,
                                                            requestState.AsyncResult.AsyncState);

                            requestState.Response.ResponseStatus = ResponseStatus.Completed;
                            fluentRequest.Completed(this, completedEventArgs);
                        }
                        catch (Exception ex)
                        {
                            // we need to catch the user exception so that we can end the request.
                            requestState.Exception = ex;
                        }
                    }

                    if (this.GetSaveStream() != null && this.saveStreamSeekToBeginning)
                        this.saveStream.Seek(0, System.IO.SeekOrigin.Begin);

                    Complete();
                };

            // start copying stream
            streamCopier.BeginCopy(
                ar =>
                {
                    try
                    {
                        streamCopier.EndCopy(ar);
                    }
                    catch (Exception ex)
                    {
                        requestState.Exception = ex;
                        Complete();
                    }
                }, requestState);
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

        private void Complete()
        {
            _asyncResult.Complete();
            _asyncResult = null;
        }

        [ContractVerification(true)]
        private void WriteBodyAndReadResponse(IHttpRequestBody httpBody, HttpRequestState requestState)
        {
            Contract.Requires(requestState != null);
            Contract.Requires(requestState.HttpWebRequest != null);

            var httpWebRequest = requestState.HttpWebRequest;

            httpWebRequest.BeginGetRequestStream(
                ar =>
                {
                    requestState = (HttpRequestState)ar.AsyncState;
                    var request = requestState.HttpWebRequest;

                    var destinationStream = request.EndGetRequestStream(ar);

                    var streamCopier = new StreamCopier(httpBody.Stream, destinationStream, requestState.BufferSize);

                    // notify stream copier on request body read.

                    streamCopier.OnCompleted +=
                        (o, e) =>
                        {
                            try
                            {
                                // try closing the http request body stream.
                                destinationStream.Close();
                            }
                            catch
                            {
                                // don't do anything.
                            }

                            if (e.Exception != null)
                            {
                                // if an exception occurred.
                                requestState.Exception = e.Exception;
                                Complete();
                            }
                            else if (e.IsCanceled)
                            {
                                // if cancelled
                                Complete();
                            }
                            else
                            {
                                // else copy completed

                                // singal request body write complete.

                                // start receiving the response.
                                ReadResponse(requestState);
                            }
                        };

                    streamCopier.BeginCopy(
                        arStreamCopier =>
                        {
                            try
                            {
                                streamCopier.EndCopy(arStreamCopier);
                            }
                            catch (Exception ex)
                            {
                                requestState.Exception = ex;
                                Complete();
                            }
                        },
                        requestState);

                },
                requestState);
        }

        public FluentHttpResponse EndRequest(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            var ar = asyncResult as FluentHttpAsyncResult;

            if (ar == null || !ReferenceEquals(_asyncResult, ar))
                throw new ArgumentException("asyncResult");

            ar.AsyncWaitHandle.WaitOne();

            // propagate the exception to the one who calls EndRequest.
            if (ar.HttpRequestState.Exception != null)
            {
                ar.HttpRequestState.Response.ResponseStatus = ResponseStatus.Error;
                throw ar.HttpRequestState.Exception;
            }

            return ar.HttpRequestState.Response;
        }
    }
}