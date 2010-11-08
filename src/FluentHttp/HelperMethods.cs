namespace FluentHttp
{
    using System;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Fluent Http Wrapper
    /// </summary>
    public partial class FluentHttpRequest
    {
        /// <summary>
        /// Adds necessary authentication stuffs if required.
        /// </summary>
        internal void AuthenticateIfRequried()
        {
            var authenticator = GetAuthenticator();
            if (authenticator != null)
                authenticator.Authenticate(this);
        }

        /// <summary>
        /// Builds the url, ie. sums up the baseUrl + resourePath + querystring
        /// </summary>
        internal static string BuildUrl(FluentHttpRequest fluentHttpRequest)
        {
            var sb = new StringBuilder();

            sb.Append(fluentHttpRequest.BaseUrl);
            sb.Append(fluentHttpRequest.GetResourcePath());

            // append querystrings

            return sb.ToString();
        }

        /// <summary>
        /// Creates the HttpWebRequest
        /// </summary>
        /// <param name="fluentHttpRequest">
        /// The fluent Http Request.
        /// </param>
        /// <returns>
        /// </returns>
        internal static HttpWebRequest CreateHttpWebRequest(FluentHttpRequest fluentHttpRequest)
        {
            var requestUrl = BuildUrl(fluentHttpRequest);
            var webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

            // todo append querystring

            webRequest.Method = fluentHttpRequest.GetMethod();
            webRequest = SetHeaders(fluentHttpRequest, webRequest);

            // this is required in case the file was added.
            webRequest = ResetContentLengthIfNeeded(fluentHttpRequest, webRequest);

            return webRequest;
        }

        internal static HttpWebRequest SetHeaders(FluentHttpRequest fluentHttpRequest, HttpWebRequest webRequest)
        {
            // default content-length to 0 if it is not GET.
            if (!fluentHttpRequest.GetMethod().Equals("GET", StringComparison.OrdinalIgnoreCase))
                webRequest.ContentLength = 0;

            var headers = fluentHttpRequest.GetHeaders();

            foreach (var fluentHttpHeader in headers)
            {
                if (FluentHttpHeaders.IsSpecialHeader(fluentHttpHeader.Name) == -1)
                    webRequest.Headers.Add(fluentHttpHeader.Name, fluentHttpHeader.Value);
                else
                {
                    // todo: need to reorder so the performance is better
                    if (fluentHttpHeader.Name.Equals("accept", StringComparison.OrdinalIgnoreCase))
                        webRequest.Accept = fluentHttpHeader.Value;
                    else if (fluentHttpHeader.Name.Equals("connection", StringComparison.OrdinalIgnoreCase))
                        webRequest.Connection = fluentHttpHeader.Value;
                    else if (fluentHttpHeader.Name.Equals("content-length", StringComparison.OrdinalIgnoreCase))
                        webRequest.ContentLength = long.Parse(fluentHttpHeader.Value);
                    else if (fluentHttpHeader.Name.Equals("content-type", StringComparison.OrdinalIgnoreCase))
                        webRequest.ContentType = fluentHttpHeader.Value;
                    else if (fluentHttpHeader.Name.Equals("expect", StringComparison.OrdinalIgnoreCase))
                        webRequest.Expect = fluentHttpHeader.Value;
                    // date set by the system
                    // host set by the system
                    else if (fluentHttpHeader.Name.Equals("if-modified-since", StringComparison.OrdinalIgnoreCase))
                        webRequest.IfModifiedSince = DateTime.Parse(fluentHttpHeader.Value);
                    // todo range?
                    else if (fluentHttpHeader.Name.Equals("referer", StringComparison.OrdinalIgnoreCase))
                        webRequest.Referer = fluentHttpHeader.Value;
                    else if (fluentHttpHeader.Name.Equals("transfer-encoding", StringComparison.OrdinalIgnoreCase))
                        webRequest.TransferEncoding = fluentHttpHeader.Value;
                    else if (fluentHttpHeader.Name.Equals("user-agent", StringComparison.OrdinalIgnoreCase))
                        webRequest.UserAgent = fluentHttpHeader.Value;
                }
            }

            return webRequest;
        }

        /// <summary>
        /// Resets content length, if file was added or something else.
        /// </summary>
        /// <param name="fluentHttpRequest">
        /// The fluent http request.
        /// </param>
        /// <param name="webRequest">
        /// The web request.
        /// </param>
        /// <returns>
        /// </returns>
        internal static HttpWebRequest ResetContentLengthIfNeeded(FluentHttpRequest fluentHttpRequest, HttpWebRequest webRequest)
        {
            // todo
            return webRequest;
        }
    }
}