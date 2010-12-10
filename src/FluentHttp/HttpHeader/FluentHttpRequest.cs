namespace FluentHttp
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1601:PartialElementsMustBeDocumented",
        Justification = "Reviewed. Suppression is OK here.")]
    public partial class FluentHttpRequest
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

                return httpHeaders ?? (this.httpHeaders = new FluentHttpHeaders());
            }
        }

        /// <summary>
        /// Access http headers.
        /// </summary>
        /// <param name="headers">
        /// The http headers.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest Headers(Action<FluentHttpHeaders> headers)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            if (headers != null)
                headers(HttpHeaders);

            return this;
        }

        /// <summary>
        /// Gets the http headers for the <see cref="FluentHttpRequest"/>.
        /// </summary>
        /// <returns>
        /// Returns the http headers.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ContractVerification(true)]
        public FluentHttpHeaders GetHeaders()
        {
            Contract.Ensures(Contract.Result<FluentHttpHeaders>() != null);

            return HttpHeaders;
        }
    }
}