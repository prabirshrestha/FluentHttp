namespace FluentHttp.Authenticators
{
    /// <summary>
    /// Interface for Authenticator
    /// </summary>
    public interface IFluentAuthenticator
    {
        /// <summary>
        /// Authenticate the fluent http request.
        /// </summary>
        /// <param name="fluentHttpRequest">
        /// The fluent http request.
        /// </param>
        void Authenticate(FluentHttpRequest fluentHttpRequest);
    }
}