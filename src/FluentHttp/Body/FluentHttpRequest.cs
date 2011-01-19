namespace FluentHttp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1601:PartialElementsMustBeDocumented",
        Justification = "Reviewed. Suppression is OK here.")]
    public partial class FluentHttpRequest
    {
        private FluentHttpRequestBody fluentHttprequestBody;

        [ContractVerification(true)]
        private FluentHttpRequestBody FluentHttpRequestBody
        {
            get
            {
                Contract.Ensures(Contract.Result<FluentHttpRequestBody>() != null);

                return this.fluentHttprequestBody ?? (this.fluentHttprequestBody = new FluentHttpRequestBody());
            }
        }

        [ContractVerification(true)]
        public FluentHttpRequest Body(Action<FluentHttpRequestBody> requestBody)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            if (requestBody != null)
            {
                requestBody(this.FluentHttpRequestBody);
            }

            return this;
        }

        [ContractVerification(true)]
        public FluentHttpRequestBody GetRequestBody()
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestBody>() != null);
            
            return this.FluentHttpRequestBody;
        }

    }
}