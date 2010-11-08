namespace FluentHttp
{
    using System;
    using System.Net;

    public delegate void FluentHttpCallback();
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
            HttpWebResponse response;

            switch (request.Method.ToUpperInvariant())
            {
                case "GET":
                    response = Get(this, request);
                    break;
                case "POST":
                case "PUT":
                case "DELETE":
                default:
                    throw new NotSupportedException(string.Format("'{0}' http method is not supported"));
            }

            return null;

            return null;
        }
    }
}