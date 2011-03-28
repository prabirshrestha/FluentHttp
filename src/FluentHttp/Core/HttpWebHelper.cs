
namespace FluentHttp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using Prabir.Async;

    internal class HttpWebHelper
    {
        //public virtual string BuildRequestUrl(string baseUrl, string resourcePath, IEnumerable<Pair<string, string>> queryStrings)
        //{
        //    var sb = new System.Text.StringBuilder();

        //    if (string.IsNullOrEmpty(baseUrl))
        //    {
        //        throw new System.ArgumentNullException("baseUrl");
        //    }

        //    sb.Append(baseUrl);
        //    sb.Append(AddStartingSlashIfNotPresent(resourcePath));
        //    sb.Append("?");

        //    if (queryStrings != null)
        //    {
        //        foreach (var queryString in queryStrings)
        //        {
        //            // note: assume queryString is already url encoded.
        //            sb.AppendFormat("{0}={1}&", queryString.Name, queryString.Value);
        //        }
        //    }

        //    // remote the last & or ?
        //    --sb.Length;

        //    return sb.ToString();
        //}

        public virtual IHttpWebRequest CreateHttpWebRequest(string requestUrl, string httpMethod, IEnumerable<Pair<string, string>> requestHeaders, CookieCollection requestCookies)
        {
            var httpWebRequest = CreateNewHttpWebRequest(requestUrl);

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

        public virtual IEnumerable<IAsync> ExecuteAsync(
            IHttpWebRequest httpWebRequest,
            Stream requestBody,
            Action<ResponseReceivedEventArgs> responseReceivedCallback)
        {
            if (httpWebRequest == null)
            {
                throw new ArgumentNullException("httpWebRequest");
            }

            // buffer space for the data to be read and written.
            byte[] buffer = new byte[4096];

            if (requestBody != null && requestBody.Length != 0)
            {
                // we have a request body, so write it asynchronously.

#if !WINDOWS_PHONE
                httpWebRequest.ContentLength = requestBody.Length;
#endif
                // assume content-type and content-lenght is already set.
                var request = Async.FromAsync(httpWebRequest.BeginGetRequestStream, httpWebRequest.EndGetRequestStream);
                yield return request;

                if (request.Exception != null)
                {
                    throw new NotImplementedException();
                }

                // while there is data to be read and written.
                var requestStream = request.Result;
                while (true)
                {
                    // read data asynchronously.
                    // when the operation completes, if no data could be read then we are done.
                    var count = Async.FromAsync<int>(requestBody.BeginRead, requestBody.EndRead, buffer, 0, buffer.Length, null);
                    yield return count;

                    if (count.Exception != null)
                    {
                        throw count.Exception;
                    }

                    if (count.Result == 0)
                    {
                        break;
                    }

                    // write data asynchronously.
                    var write = Async.FromAsync(requestStream.BeginWrite, requestStream.EndWrite, buffer, 0, count.Result, null);
                    yield return write;

                    if (write.Exception != null)
                    {
                        throw write.Exception;
                    }
                }
            }

            // asynchronously get the response from the http server.
            var response = Async.FromAsync(httpWebRequest.BeginGetResponse, httpWebRequest.EndGetResponse);

            yield return response;

            var httpWebResponse = response.Result;
            Exception exception = null;

            if (response.Exception != null)
            {
                // exception occurred
                WebExceptionWrapper webException = null;
                if (response.Exception is WebException)
                {
                    webException = new WebExceptionWrapper((WebException)response.Exception);
                }

                if (webException != null)
                {
                    exception = webException;
                    httpWebResponse = webException.GetResponse();
                }
                else
                {
                    exception = response.Exception;
                }
            }

            if (exception != null && httpWebResponse == null)
            {
                // critical error occurred.
                // most likely no internet connection or silverlight cross domain policy exception.
                throw exception;
            }

            // we have got the response
            Stream responseSaveStream = null;
            if (responseReceivedCallback != null)
            {
                // we have the response headers.
                var responseReceived = new ResponseReceivedEventArgs(httpWebResponse);
                responseReceivedCallback(responseReceived);
                responseSaveStream = responseReceived.ResponseSaveStream;
            }

            // read response stream
            var responseStream = httpWebResponse.GetResponseStream();

            if (responseSaveStream == null)
            {
                // read the response stream asynchronosuly but don't write.
                while (true)
                {
                    var count = Async.FromAsync<int>(responseStream.BeginRead, responseStream.EndRead, buffer, 0, buffer.Length, null);
                    yield return count;

                    if (count.Exception != null)
                    {
                        throw count.Exception;
                    }

                    if (count.Result == 0)
                    {
                        break;
                    }
                }
            }
            else
            {
                if (!responseSaveStream.CanWrite)
                {
                    throw new ArgumentException("responseSaveStream is not writable.");
                }

                // while there is data to be read and written.
                while (true)
                {
                    // read data asynchronously.
                    // when the operation completes, if no data could be read then we are done.
                    var count = Async.FromAsync<int>(responseStream.BeginRead, responseStream.EndRead, buffer, 0, buffer.Length, null);
                    yield return count;

                    if (count.Exception != null)
                    {
                        throw count.Exception;
                    }

                    if (count.Result == 0)
                    {
                        break;
                    }

                    // write data asynchronously.
                    var write = Async.FromAsync(responseSaveStream.BeginWrite, responseSaveStream.EndWrite, buffer, 0, count.Result, null);
                    yield return write;

                    if (write.Exception != null)
                    {
                        throw write.Exception;
                    }
                }
            }
        }

        public virtual void Execute(
            IHttpWebRequest httpWebRequest,
            Stream requestBody,
            Action<ResponseReceivedEventArgs> responseReceivedCallback)
        {
            Async.ExecuteAndWait(ExecuteAsync(httpWebRequest, requestBody, responseReceivedCallback));
        }

        protected virtual IHttpWebRequest CreateNewHttpWebRequest(string requestUrl)
        {
            if (string.IsNullOrEmpty(requestUrl))
            {
                throw new ArgumentException("requestUrl is null or empty", "requestUrl");
            }

            return new HttpWebRequestWrapper((HttpWebRequest)WebRequest.Create(requestUrl));
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
                    httpWebRequest.ContentLength = Convert.ToInt64(value);
                }
                else if (name.Equals("user-agent", StringComparison.OrdinalIgnoreCase))
                {
                    httpWebRequest.UserAgent = value;
                }
                else
                {
                    httpWebRequest.Headers.Add(name, value);
                }
            }
        }

        //internal static IList<Pair<string, string>> ExtractResponseHeaders(WebHeaderCollection headerCollection)
        //{
        //    var responseHeaders = new List<Pair<string, string>>();

        //    for (int i = 0; i < headerCollection.Count; i++)
        //    {
        //        responseHeaders.Add(new Pair<string, string>(headerCollection.GetKey(i), headerCollection[i]));
        //    }

        //    return responseHeaders;
        //}

        //public static string AddStartingSlashIfNotPresent(string input)
        //{
        //    if (string.IsNullOrEmpty(input))
        //    {
        //        return "/";
        //    }

        //    // if not null or empty
        //    if (input[0] != '/')
        //    {
        //        // if doesn't start with / then add /
        //        return "/" + input;
        //    }
        //    else
        //    {
        //        // else return the original input.
        //        return input;
        //    }
        //}
    }
}