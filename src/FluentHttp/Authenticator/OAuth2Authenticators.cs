
namespace FluentHttp
{
    /// <summary>
    /// Base class for OAuth2 Authenticators.
    /// </summary>
    public abstract class OAuth2Authenticator : IFluentAuthenticator
    {
        /// <summary>
        /// The oauth_token
        /// </summary>
        private readonly string _accessToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Authenticator"/> class.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        public OAuth2Authenticator(string accessToken)
        {
            _accessToken = accessToken;
        }

        /// <summary>
        /// Gets the OAuth2 token.
        /// </summary>
        public string OAuthToken
        {
            get { return _accessToken; }
        }

        #region Implementation of IFluentAuthenticator

        /// <summary>
        /// Authenticate the fluent http request using OAuth2.
        /// </summary>
        /// <param name="fluentHttpRequest">
        /// The fluent http request.
        /// </param>
        public abstract void Authenticate(FluentHttpRequest fluentHttpRequest);

        #endregion
    }
}