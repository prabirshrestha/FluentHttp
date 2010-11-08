namespace FluentHttp
{
    using System;
    using System.Net;

    /// <summary>
    /// Fluent Http Wrapper
    /// </summary>
    public partial class FluentHttpRequest
    {

        /// <summary>
        /// Executes the request synchronously.
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// if http method is not supported.
        /// </exception>
        public FluentHttpResponse Execute()
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

            // convert resonse to FluentHttpResponse and return response.
            return ToFluentHttpResponse(response);
        }

        private static HttpWebResponse Get(FluentHttpRequest fluentHttpRequest, HttpWebRequest request)
        {
            var fluentHtpResponse = new FluentHttpResponse();

            throw new NotImplementedException();
        }
    }
}