namespace FluentHttp
{
    using System;
    using System.IO.Compression;
    using System.Net;
    using System.IO;

    /// <summary>
    /// Fluent Http Wrapper
    /// </summary>
    public partial class FluentHttpRequest
    {
        private FluentHttpAsyncResult _asyncResult;

        public IAsyncResult BeginRequest(AsyncCallback callback,object state)
        {
            return null;
        }

        public FluentHttpResponse EndRequest(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            var ar = asyncResult as FluentHttpAsyncResult;

            if (ar == null || !ReferenceEquals(_asyncResult, ar))
                throw new ArgumentException("asyncResult");

            // propagate the exception to the one who calls EndRequest.
            if (ar.HttpRequestState.Exception != null)
            {
                ar.HttpRequestState.Response.ResponseStatus = ResponseStatus.Error;
                throw ar.HttpRequestState.Exception;
            }

            ar.AsyncWaitHandle.WaitOne();

            return ar.HttpRequestState.Response;
        }
    }
}