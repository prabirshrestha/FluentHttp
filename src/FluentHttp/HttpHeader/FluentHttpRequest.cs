namespace FluentHttp
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;

    public partial class FluentHttpRequest
    {
        // make it HttpHeaders so that it doesn't conflict with Headers method
        private FluentHttpHeaders httpHeaders;

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
        /// Access http headers
        /// </summary>
        /// <param name="headers">
        /// The headers.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest Headers(Action<FluentHttpHeaders> headers)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            if (headers != null)
                headers(HttpHeaders);

            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ContractVerification(true)]
        public FluentHttpHeaders GetHeaders()
        {
            Contract.Ensures(Contract.Result<FluentHttpHeaders>() != null);

            return HttpHeaders;
        }
    }
}