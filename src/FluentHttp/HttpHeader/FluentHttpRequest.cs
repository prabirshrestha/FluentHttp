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
        // make it HttpHeaders so that it doesn't conflict with Headers method

        /// <summary>
        /// Internal collection of http headers.
        /// </summary>
        private FluentHttpHeaders httpHeaders;

        /// <summary>
        /// Gets the http headers.
        /// </summary>
        [ContractVerification(true)]
        private FluentHttpHeaders HttpHeaders
        {
            get
            {
                Contract.Ensures(Contract.Result<FluentHttpHeaders>() != null);

                return this.httpHeaders ?? (this.httpHeaders = new FluentHttpHeaders());
            }
        }

        /// <summary>
        /// Access http headers.
        /// </summary>
        /// <param name="headers">
        /// The http headers.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequestOld"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequestOld Headers(Action<FluentHttpHeaders> headers)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestOld>() != null);

            if (headers != null)
                headers(HttpHeaders);

            return this;
        }

        /// <summary>
        /// Gets the http headers for the <see cref="FluentHttpRequestOld"/>.
        /// </summary>
        /// <returns>
        /// Returns the http headers.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ContractVerification(true)]
        [Pure]
        public FluentHttpHeaders GetHeaders()
        {
            Contract.Ensures(Contract.Result<FluentHttpHeaders>() != null);

            return HttpHeaders;
        }
    }
}