namespace FluentHttp
{
    using System.ComponentModel;
    using System.Net;

    public partial class FluentHttpRequest
    {
        private IWebProxy _proxy;

        public FluentHttpRequest Proxy(IWebProxy proxy)
        {
            _proxy = proxy;
            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IWebProxy GetProxy()
        {
            return _proxy;
        }
    }
}