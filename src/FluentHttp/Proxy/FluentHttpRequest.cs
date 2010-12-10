namespace FluentHttp
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Net;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1601:PartialElementsMustBeDocumented",
        Justification = "Reviewed. Suppression is OK here.")]
    public partial class FluentHttpRequest
    {
        /// <summary>
        /// Proxy to use when making http web request.
        /// </summary>
        private IWebProxy proxy;

        /// <summary>
        /// Sets the proxy.
        /// </summary>
        /// <param name="proxy">
        /// The proxy to use.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest Proxy(IWebProxy proxy)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            this.proxy = proxy;

            return this;
        }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <returns>
        /// Returns the proxy.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IWebProxy GetProxy()
        {
            return proxy;
        }
    }
}