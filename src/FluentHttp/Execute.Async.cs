namespace FluentHttp
{
    using System;
    using System.Net;

    /// <summary>
    /// Fluent Http Wrapper
    /// </summary>
    public partial class FluentHttpRequest
    {
        public IAsyncResult BeginRequest(FluentHttpRequest fluentHttpRequest, FluentHttpCallback callback, object userState)
        {
            if (Executing != null)
                Executing(this, new ExecutingEventArgs(this));

            AuthenticateIfRequried();

            var request = CreateHttpWebRequest(this);
            var requestState = new HttpRequestState(1448)
                                   {
                                       Request = this,
                                       HttpWebRequest = request,
                                       FluentHttpCallback = callback
                                   };

            // TODO check if http method is supported

            return request.BeginGetResponse(ResponseCallback, requestState);
        }

        private static void ResponseCallback(IAsyncResult asyncResult)
        {
            try
            {
                var requestState = (HttpRequestState)asyncResult.AsyncState;
                var fluentRequest = requestState.Request;

                var response = (HttpWebResponse)requestState.HttpWebRequest.EndGetResponse(asyncResult);
                requestState.Response = ToFluentHttpResponseAsync(response);

                // TODO notify that the header has been received.

                // now download the actual data
                var responseStream = response.GetResponseStream();
                requestState.ResponseStream = responseStream;

                // begin reading contents of the response data
                var result = responseStream.BeginRead(requestState.BufferRead, 0, requestState.BufferSize, ReadCallback,
                                                      requestState);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void ReadCallback(IAsyncResult asyncResult)
        {
            // start reading the acutal contents data
            try
            {
                var requestState = (HttpRequestState)asyncResult.AsyncState;

                var responseStream = requestState.ResponseStream;

                // get get results of read operation

                int bytesRead = responseStream.EndRead(asyncResult);

                // got some data, need to read more
                if (bytesRead > 0)
                {
                    requestState.BytesRead += bytesRead;
                    double percentageComplete = ((double)requestState.BytesRead / requestState.TotalBytes) * 100.0f;

                    var totalTime = DateTime.Now - requestState.TransferStartedDate;

                    // TODO: report progress

                    // kick of another read
                    var result = responseStream.BeginRead(requestState.BufferRead, 0, requestState.BufferSize,
                                                          ReadCallback, requestState);
                    return;
                }
                else
                {
                    // EndRead returned 0, so no more data to read
                    responseStream.Close();
                    requestState.Response.HttpWebResponse.Close();
                    
                    // TODO notify completed.
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}