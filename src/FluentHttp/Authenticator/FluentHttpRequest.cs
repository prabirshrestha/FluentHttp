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
        /// <summary>
        /// Internal authenticator.
        /// </summary>
        private IFluentAuthenticator fluentAuthenticator;

        /// <summary>
        /// Sets the authenticator.
        /// </summary>
        /// <param name="fluentAuthenticator">
        /// The fluent authenticator.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequest"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequest AuthenticateUsing(IFluentAuthenticator fluentAuthenticator)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequest>() != null);

            this.fluentAuthenticator = fluentAuthenticator;

            return this;
        }

        public FluentHttpRequest AuthenticateUsing(Func<IFluentAuthenticator> authenticator)
        {
            if (authenticator != null)
            {
                this.fluentAuthenticator = authenticator();
            }

            return this;
        }

        /// <summary>
        /// Gets the authenticator.
        /// </summary>
        /// <returns>
        /// Retruns <see cref="IFluentAuthenticator"/>.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IFluentAuthenticator GetAuthenticator()
        {
            return fluentAuthenticator;
        }
    }
}