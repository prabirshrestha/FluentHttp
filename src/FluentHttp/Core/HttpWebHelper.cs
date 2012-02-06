﻿
namespace FluentHttp
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.IO;

    internal delegate IHttpWebRequest HttpWebRequestFactoryDelegate(string url);

    internal delegate void StreamCopyCompletedDelegate(Stream input, Stream output, Exception exception);

    internal class HttpWebHelper
    {
        protected readonly HttpWebRequestFactoryDelegate HttpWebRequestFactory;

        public event EventHandler<ResponseReceivedEventArgs> ResponseReceived;

        public bool FlushResponseStream { get; set; }
        public bool FlushResponseSaveStream { get; set; }
        public bool FlushRequestStream { get; set; }
        public bool FlushInputRequestBody { get; set; }

        public bool AsyncRequestStream { get; set; }
        public bool AsyncResponseStream { get; set; }

        public HttpWebHelper()
        {
            Init();
            HttpWebRequestFactory =
                requestUrl =>
                {
                    if (string.IsNullOrEmpty(requestUrl))
                    {
                        throw new ArgumentException("requestUrl is null or empty", "requestUrl");
                    }

                    return new HttpWebRequestWrapper((HttpWebRequest)WebRequest.Create(requestUrl));
                };
        }

        public HttpWebHelper(HttpWebRequestFactoryDelegate httpWebRequestFactory)
        {
            if (httpWebRequestFactory == null)
            {
                throw new ArgumentNullException("httpWebRequestFactory");
            }

            Init();
            HttpWebRequestFactory = httpWebRequestFactory;
        }

        private void Init()
        {
            FlushResponseStream = true;
            FlushResponseSaveStream = true;
        }

        public static string BuildRequestUrl(string baseUrl, string resourcePath, IEnumerable<Pair<string, string>> queryStrings)
        {
            var sb = new System.Text.StringBuilder();

            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException("baseUrl");
            }

            sb.Append(baseUrl);
            if (!string.IsNullOrEmpty(resourcePath))
                sb.Append(AddStartingSlashIfNotPresent(resourcePath));
            sb.Append("?");

            if (queryStrings != null)
            {
                foreach (var queryString in queryStrings)
                {
                    // note: assume queryString is already url encoded.
                    sb.AppendFormat("{0}={1}&", queryString.Name, queryString.Value);
                }
            }

            // remote the last & or ?
            --sb.Length;

            return sb.ToString();
        }

        public static string AddStartingSlashIfNotPresent(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "/";
            }

            // if not null or empty
            if (input[0] != '/')
            {
                // if doesn't start with / then add /
                return "/" + input;
            }
            else
            {
                // else return the original input.
                return input;
            }
        }

        public virtual IHttpWebRequest CreateHttpWebRequest(string requestUrl, string httpMethod, IEnumerable<Pair<string, string>> requestHeaders, CookieCollection requestCookies)
        {
            var httpWebRequest = HttpWebRequestFactory(requestUrl);

            if (httpWebRequest == null)
            {
                throw new Exception("HttpWebRequest factory returned null.");
            }

            httpWebRequest.Method = httpMethod ?? "GET";

#if !SILVERLIGHT

            if (requestCookies != null)
            {
                foreach (Cookie requestCookie in requestCookies)
                {
                    httpWebRequest.CookieContainer.Add(requestCookie);
                }
            }

#endif

#if !WINDOWS_PHONE

            if (!httpWebRequest.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                // set default content-length to 0 if it is not GET.
                httpWebRequest.ContentLength = 0;
            }

#endif

            if (requestHeaders != null)
            {
                SetHttpWebRequestHeaders(httpWebRequest, requestHeaders);
            }

            return httpWebRequest;
        }

        protected virtual void SetHttpWebRequestHeaders(IHttpWebRequest httpWebRequest, IEnumerable<Pair<string, string>> requestHeaders)
        {
            foreach (var requestHeader in requestHeaders)
            {
                var name = requestHeader.Name;
                var value = requestHeader.Value;

                // todo: add more special headers
                if (name.Equals("content-type", StringComparison.OrdinalIgnoreCase))
                {
                    httpWebRequest.ContentType = value;
                }
                else if (name.Equals("content-length", StringComparison.OrdinalIgnoreCase))
                {
#if WINDOWS_PHONE
                    throw new Exception("Cannot set content-length.");
#else
                    httpWebRequest.ContentLength = Convert.ToInt64(value);
#endif
                }
                else if(name.Equals("accept", StringComparison.OrdinalIgnoreCase))
                {
                    httpWebRequest.Accept = value;
                }
                else if (name.Equals("user-agent", StringComparison.OrdinalIgnoreCase))
                {
                    httpWebRequest.UserAgent = value;
                }
                else
                {
#if SILVERLIGHT
                    httpWebRequest.Headers[name] = value;
#else
                    httpWebRequest.Headers.Add(name, value);
#endif
                }
            }
        }

        public virtual void ExecuteAsync(IHttpWebRequest httpWebRequest, Stream requestBody, AsyncCallback callback, object state)
        {
            if (httpWebRequest == null)
            {
                throw new ArgumentNullException("httpWebRequest");
            }

            if (requestBody != null && requestBody.Length != 0)
            {
                // we have a request body, so write it asynchronously.
#if !WINDOWS_PHONE
                httpWebRequest.ContentLength = requestBody.Length;
#endif
                BeginGetRequestStream(httpWebRequest, requestBody, callback, state);
            }
            else
            {
                // asynchronously get the response from the http server.  
                BeginGetResponse(httpWebRequest, requestBody, callback, state);
            }
        }

#if !SILVERLIGHT

        public virtual HttpWebHelperResult Execute(IHttpWebRequest httpWebRequest, Stream requestBody)
        {
            if (httpWebRequest == null)
            {
                throw new ArgumentNullException("httpWebRequest");
            }

            if (requestBody != null && requestBody.Length != 0)
            {
                // we have a request body, so write it synchronously.
#if !WINDOWS_PHONE
                httpWebRequest.ContentLength = requestBody.Length;
#endif
                return WriteAndGetReponse(httpWebRequest, requestBody);
            }
            else
            {
                // synchronously get the response from the http server.
                return GetResponse(httpWebRequest, requestBody);
            }
        }

        private HttpWebHelperResult WriteAndGetReponse(IHttpWebRequest httpWebRequest, Stream requestBody)
        {
            Stream requestStream = null;
            Exception exception = null;
            IHttpWebResponse httpWebResponse = null;
            HttpWebHelperResult result;

            try
            {
                requestStream = httpWebRequest.GetRequestStream();
            }
            catch (WebException ex)
            {
                var webException = new WebExceptionWrapper(ex);
                httpWebResponse = webException.GetResponse();
                exception = webException;
            }
            finally
            {
                if (exception == null)
                {
                    // we got the stream, so copy to the stream
                    result = CopyRequestStream(httpWebRequest, requestBody, requestStream);
                }
                else
                {
                    // there was an error
                    if (httpWebResponse == null)
                    {
                        result = new HttpWebHelperResult(httpWebRequest, null, exception, null, false, true, null, null);
                    }
                    else
                    {
                        var args = new ResponseReceivedEventArgs(httpWebResponse, exception, null);
                        OnResponseReceived(args);
                        result = ReadResponseStream(httpWebRequest, httpWebResponse, exception, args.ResponseSaveStream);
                    }
                }
            }

            return result;
        }

        private HttpWebHelperResult CopyRequestStream(IHttpWebRequest httpWebRequest, Stream requestBody, Stream requestStream)
        {
            try
            {
                CopyStream(requestBody, requestStream, FlushInputRequestBody, FlushRequestStream);
                requestStream.Close();
            }
            catch (Exception ex)
            {
                return new HttpWebHelperResult(httpWebRequest, null, ex, null, false, false, null, null);
            }

            return GetResponse(httpWebRequest, requestBody);
        }

        private HttpWebHelperResult GetResponse(IHttpWebRequest httpWebRequest, Stream requestBody)
        {
            IHttpWebResponse httpWebResponse = null;
            Exception exception = null;
            HttpWebHelperResult httpWebHelperAsyncResult = null;

            try
            {
                httpWebResponse = httpWebRequest.GetResponse();
            }
            catch (WebException ex)
            {
                var webException = new WebExceptionWrapper(ex);
                httpWebResponse = webException.GetResponse();
                exception = webException;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (httpWebResponse != null)
                {
                    var args = new ResponseReceivedEventArgs(httpWebResponse, exception, null);
                    OnResponseReceived(args);
                    httpWebHelperAsyncResult = ReadResponseStream(httpWebRequest, httpWebResponse, exception, args.ResponseSaveStream);
                }
                else
                {
                    httpWebHelperAsyncResult = new HttpWebHelperResult(httpWebRequest, httpWebResponse, exception, null, false, true, null, null);
                }
            }

            return httpWebHelperAsyncResult;
        }

        private HttpWebHelperResult ReadResponseStream(IHttpWebRequest httpWebRequest, IHttpWebResponse httpWebResponse, Exception innerException, Stream responseSaveStream)
        {
            Stream responseStream = null;
            Exception exception = null;

            try
            {
                responseStream = httpWebResponse.GetResponseStream();
            }
            catch (WebException ex)
            {
                exception = new WebExceptionWrapper(ex);
            }

            return exception == null ? CopyResponseStream(httpWebRequest, httpWebResponse, innerException, responseStream, responseSaveStream) : new HttpWebHelperResult(httpWebRequest, httpWebResponse, exception, innerException, false, true, responseSaveStream, null);
        }

        private HttpWebHelperResult CopyResponseStream(IHttpWebRequest httpWebRequest, IHttpWebResponse httpWebResponse, Exception innerException, Stream responseStream, Stream responseSaveStream)
        {
            try
            {
                CopyStream(responseStream, responseSaveStream, false, false);
                responseStream.Close();

                return new HttpWebHelperResult(httpWebRequest, httpWebResponse, null, innerException, false, true, responseSaveStream, null);
            }
            catch (Exception ex)
            {
                return new HttpWebHelperResult(httpWebRequest, httpWebResponse, ex, innerException, false, true, responseSaveStream, null);
            }
        }

#endif

        protected void BeginGetRequestStream(IHttpWebRequest httpWebRequest, Stream requestBody, AsyncCallback callback, object state)
        {
            httpWebRequest.BeginGetRequestStream(ar => RequestCallback(ar, httpWebRequest, requestBody, callback, state), null);
        }

        protected virtual void BeginGetResponse(IHttpWebRequest httpWebRequest, Stream requestBody, AsyncCallback callback, object state)
        {
            httpWebRequest.BeginGetResponse(ar => ResponseCallback(ar, httpWebRequest, requestBody, callback, state), null);
        }

        private void RequestCallback(IAsyncResult asyncResult, IHttpWebRequest httpWebRequest, Stream requestBody, AsyncCallback callback, object state)
        {
            Stream requestStream = null;
            Exception exception = null;
            IHttpWebResponse httpWebResponse = null;

            try
            {
                requestStream = httpWebRequest.EndGetRequestStream(asyncResult);
            }
            catch (WebException ex)
            {
                var webException = new WebExceptionWrapper(ex);
                httpWebResponse = webException.GetResponse();
                exception = webException;
            }
            finally
            {
                if (exception == null)
                {
                    // we got the stream, so copy to the stream
                    CopyRequestStream(httpWebRequest, requestBody, requestStream, callback, state);
                }
                else
                {
                    // there was an error
                    if (httpWebResponse == null)
                    {
                        if (callback != null)
                            callback(new HttpWebHelperResult(httpWebRequest, null, exception, null, false, false, null, state));
                    }
                    else
                    {
                        var args = new ResponseReceivedEventArgs(httpWebResponse, exception, state);
                        OnResponseReceived(args);
                        ReadResponseStream(httpWebRequest, httpWebResponse, exception, args.ResponseSaveStream, callback, state);
                    }
                }
            }
        }

        private void CopyRequestStream(IHttpWebRequest httpWebRequest, Stream requestBody, Stream requestStream, AsyncCallback callback, object state)
        {
            if (AsyncRequestStream)
            {
                // pure read/write async
                CopyStreamAsync(requestBody, requestStream, FlushInputRequestBody, FlushRequestStream,
                    (source, destination, exception) =>
                    {
                        source.Close();

                        if (exception == null)
                        {
                            BeginGetResponse(httpWebRequest, requestBody, callback, state);
                        }
                        else
                        {
                            if (callback != null)
                                callback(new HttpWebHelperResult(httpWebRequest, null, exception, null, false, false, null, state));
                        }
                    });
            }
            else
            {
                // Read requestBody then write synchronously.
                try
                {
                    CopyStream(requestBody, requestStream, FlushInputRequestBody, FlushRequestStream);
                    requestStream.Close();
                    BeginGetResponse(httpWebRequest, requestBody, callback, state);
                }
                catch (Exception ex)
                {
                    if (callback != null)
                        callback(new HttpWebHelperResult(httpWebRequest, null, ex, null, false, false, null, state));
                }
            }
        }

        private void ResponseCallback(IAsyncResult asyncResult, IHttpWebRequest httpWebRequest, Stream requestBody, AsyncCallback callback, object state)
        {
            IHttpWebResponse httpWebResponse = null;
            Exception exception = null;

            try
            {
                httpWebResponse = httpWebRequest.EndGetResponse(asyncResult);
            }
            catch (WebException ex)
            {
                var webException = new WebExceptionWrapper(ex);
                httpWebResponse = webException.GetResponse();
                exception = webException;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (httpWebResponse != null)
                {
                    var args = new ResponseReceivedEventArgs(httpWebResponse, exception, state);
                    OnResponseReceived(args);
                    ReadResponseStream(httpWebRequest, httpWebResponse, exception, args.ResponseSaveStream, callback, state);
                }
                else
                {
                    if (callback != null)
                        callback(new HttpWebHelperResult(httpWebRequest, httpWebResponse, exception, null, false, false, null, state));
                }
            }
        }

        private void ReadResponseStream(IHttpWebRequest httpWebRequest, IHttpWebResponse httpWebResponse, Exception innerException, Stream responseSaveStream, AsyncCallback callback, object state)
        {
            Stream responseStream = null;
            Exception exception = null;

            try
            {
                responseStream = httpWebResponse.GetResponseStream();
            }
            catch (WebException ex)
            {
                exception = new WebExceptionWrapper(ex);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (exception == null)
                {
                    CopyResponseStream(httpWebRequest, httpWebResponse, innerException, responseStream, responseSaveStream, callback, state);
                }
                else
                {
                    if (callback != null)
                        callback(new HttpWebHelperResult(httpWebRequest, httpWebResponse, exception, null, false, false, responseSaveStream, state));
                }
            }
        }

        private void CopyResponseStream(IHttpWebRequest httpWebRequest, IHttpWebResponse httpWebResponse, Exception innerException, Stream responseStream, Stream responseSaveStream, AsyncCallback callback, object state)
        {
            if (AsyncResponseStream && responseSaveStream != null)
            {
                // pure read/write async
                CopyStreamAsync(responseStream, responseSaveStream, FlushResponseStream, FlushResponseSaveStream,
                                (source, destination, exception) =>
                                {
                                    source.Close();
                                    callback(new HttpWebHelperResult(httpWebRequest, httpWebResponse, exception, null, false, false, responseSaveStream, state));
                                });
            }
            else
            {
                try
                {
                    if (responseSaveStream == null)
                        ReadStream(responseStream, FlushResponseStream);
                    else
                        CopyStream(responseStream, responseSaveStream, FlushResponseStream, FlushResponseSaveStream);

                    responseStream.Close();
                    if (callback != null)
                        callback(new HttpWebHelperResult(httpWebRequest, httpWebResponse, null, innerException, false, false, responseSaveStream, state));
                }
                catch (Exception ex)
                {
                    if (callback != null)
                        callback(new HttpWebHelperResult(httpWebRequest, httpWebResponse, ex, null, false, false, responseSaveStream, state));
                }
            }
        }

        protected void OnResponseReceived(ResponseReceivedEventArgs args)
        {
            if (ResponseReceived != null)
            {
                ResponseReceived(this, args);
            }
        }

        private void CopyStream(Stream input, Stream output, bool flushInput, bool flushOutput)
        {
            byte[] buffer = new byte[1024 * 4]; // 4 kb
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (flushInput)
                    input.Flush();
                if (read <= 0)
                    return;
                output.Write(buffer, 0, read);
                if (flushOutput)
                    output.Flush();
            }
        }

        private void ReadStream(Stream input, bool flushInput)
        {
            byte[] buffer = new byte[1024 * 4]; // 4 kb
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (flushInput)
                    input.Flush();
                if (read <= 0)
                    return;
            }
        }

        private void CopyStreamAsync(Stream input, Stream output, bool flushInput, bool flushOutput, StreamCopyCompletedDelegate completed)
        {
            byte[] buffer = new byte[1024 * 4];
            var asyncOp = System.ComponentModel.AsyncOperationManager.CreateOperation(null);

            Action<Exception> done = e =>
            {
                if (completed != null) asyncOp.Post(delegate
                    {
                        completed(input, output, e);
                    }, null);
            };

            AsyncCallback rc = null;
            rc = readResult =>
            {
                try
                {
                    int read = input.EndRead(readResult);
                    if (read > 0)
                    {
                        if (flushInput) input.Flush();
                        output.BeginWrite(buffer, 0, read, writeResult =>
                        {
                            try
                            {
                                output.EndWrite(writeResult);
                                if (flushOutput) output.Flush();
                                input.BeginRead(buffer, 0, buffer.Length, rc, null);
                            }
                            catch (Exception exc) { done(exc); }
                        }, null);
                    }
                    else done(null);
                }
                catch (Exception exc) { done(exc); }
            };

            input.BeginRead(buffer, 0, buffer.Length, rc, null);
        }

        public static string ToString(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

#if HTTPWEBHELPER_TPL

        public System.Threading.Tasks.Task<HttpWebHelperAsyncResult> ExecuteTaskAsync(IHttpWebRequest httpWebRequest, Stream requestBody, object state)
        {
            var tcs = new System.Threading.Tasks.TaskCompletionSource<HttpWebHelperAsyncResult>(state);

            try
            {
                ExecuteAsync(httpWebRequest, requestBody,
                             ar =>
                             {
                                 var asyncResult = (HttpWebHelperAsyncResult)ar;
                                 if (asyncResult.IsCancelled) tcs.TrySetCanceled();
                                 if (asyncResult.Exception != null) tcs.TrySetException(asyncResult.Exception);
                                 else tcs.TrySetResult(asyncResult);
                             }, state);

            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return tcs.Task;
        }

        public System.Threading.Tasks.Task<HttpWebHelperAsyncResult> ExecuteTaskAsync(IHttpWebRequest httpWebRequest, Stream requestBody)
        {
            return ExecuteTaskAsync(httpWebRequest, requestBody, null);
        }

#endif

    }
}