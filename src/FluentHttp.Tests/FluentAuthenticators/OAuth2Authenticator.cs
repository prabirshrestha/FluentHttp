
namespace FluentHttp.Authenticators
{
    /// <summary>
    /// Base class for OAuth2 Authenticators.
    /// </summary>
    abstract class OAuth2Authenticator : IFluentAuthenticator
    {
        /// <summary>
        /// The oauth 2 token.
        /// </summary>
        private readonly string _oauthToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Authenticator"/> class.
        /// </summary>
        /// <param name="oauthToken">The oauth 2 token.</param>
        protected OAuth2Authenticator(string oauthToken)
        {
            _oauthToken = oauthToken;
        }

        /// <summary>
        /// The oauth 2 token.
        /// </summary>
        public string OAuthToken
        {
            get { return _oauthToken; }
        }

        /// <summary>
        /// Authenticates the fluent http request using OAuth2.
        /// </summary>
        /// <param name="fluentHttpRequest">The fluent http request.</param>
        public abstract void Authenticate(FluentHttpRequest fluentHttpRequest);
    }

    /// <summary>
    /// Authenticate the fluent http request using OAuth2 uri querystring parameter.
    /// </summary>
    class OAuth2UriQueryParameterAuthenticator : OAuth2Authenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2UriQueryParameterAuthenticator"/> class.
        /// </summary>
        /// <param name="oauthToken">The oauth 2 token.</param>
        public OAuth2UriQueryParameterAuthenticator(string oauthToken)
            : base(oauthToken)
        {
        }

        /// <summary>
        /// Authenticate the fluent http request using OAuth2 uri querystring parameter.
        /// </summary>
        /// <param name="fluentHttpRequest">The fluent http request.</param>
        public override void Authenticate(FluentHttpRequest fluentHttpRequest)
        {
            fluentHttpRequest.QueryStrings(qs => qs.Add("oauth_token", OAuthToken));
        }
    }
}