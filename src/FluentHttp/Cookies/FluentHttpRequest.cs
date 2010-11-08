namespace FluentHttp
{
    using System;
    using System.ComponentModel;

    public partial class FluentHttpRequest
    {
        private FluentCookies _httpCookies;
        private FluentCookies HttpCookies
        {
            get { return _httpCookies ?? (_httpCookies = new FluentCookies()); }
        }

        public FluentHttpRequest Cookies(Action<FluentCookies> cookies)
        {
            if (cookies != null)
                cookies(HttpCookies);
            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentCookies GetCookies()
        {
            return HttpCookies;
        }
    }
}