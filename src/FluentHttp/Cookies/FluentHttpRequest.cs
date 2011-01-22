namespace FluentHttp
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1601:PartialElementsMustBeDocumented",
        Justification = "Reviewed. Suppression is OK here.")]
    public partial class FluentHttpRequestOld
    {
        /// <summary>
        /// Internal http cookies.
        /// </summary>
        private FluentCookies httpCookies;

        /// <summary>
        /// Gets the internal HttpCookies.
        /// </summary>
        [ContractVerification(true)]
        private FluentCookies HttpCookies
        {
            get
            {
                Contract.Ensures(Contract.Result<FluentCookies>() != null);

                return this.httpCookies ?? (this.httpCookies = new FluentCookies());
            }
        }

        /// <summary>
        /// Access cookies.
        /// </summary>
        /// <param name="cookies">
        /// The cookies.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequestOld"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequestOld Cookies(Action<FluentCookies> cookies)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestOld>() != null);

            if (cookies != null)
                cookies(HttpCookies);

            return this;
        }

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        /// <returns>
        /// Returns <see cref="FluentCookies"/>.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ContractVerification(true)]
        public FluentCookies GetCookies()
        {
            Contract.Ensures(Contract.Result<FluentCookies>() != null);

            return HttpCookies;
        }
    }
}