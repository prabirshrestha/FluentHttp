namespace FluentHttp
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Interface for Authenticator
    /// </summary>
    [ContractClass(typeof(FluentAuthenticatorContracts))]
    public interface IFluentAuthenticator
    {
        /// <summary>
        /// Authenticate the fluent http request.
        /// </summary>
        /// <param name="fluentHttpRequest">
        /// The fluent http request.
        /// </param>
        void Authenticate(IFluentHttpRequest fluentHttpRequest);
    }

    [ContractClassFor(typeof(IFluentAuthenticator))]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Reviewed. Suppression is OK here.")]
    internal abstract class FluentAuthenticatorContracts : IFluentAuthenticator
    {
        public void Authenticate(IFluentHttpRequest fluentHttpRequest)
        {
            Contract.Requires(fluentHttpRequest != null);
        }
    }
}