namespace FluentHttp
{
    using System;
    using System.ComponentModel;

    public partial class FluentHttpRequest
    {
        // make it HttpHeaders so that it doesn't conflict with Headers method
        private FluentHttpHeaders _httpHeaders;
        private FluentHttpHeaders HttpHeaders
        {
            get { return _httpHeaders ?? (_httpHeaders = new FluentHttpHeaders()); }
        }

        /// <summary>
        /// Access http headers
        /// </summary>
        /// <param name="headers">
        /// The headers.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>
        /// </returns>
        public FluentHttpRequest Headers(Action<FluentHttpHeaders> headers)
        {
            if (headers != null)
                headers(HttpHeaders);
            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentHttpHeaders GetHeaders()
        {
            return HttpHeaders;
        }
    }
}