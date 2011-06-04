// #define FLUENTHTTP_OAuth2UriQueryParameterBearerAuthenticator

namespace FluentHttp.Authenticators
{
    using System;
    using global::FluentHttp;

    /// <summary>
    /// Base class for OAuth2 Authenticators.
    /// </summary>
    abstract class OAuth2Authenticator : IFluentAuthenticator
    {
        /// <summary>
        /// Authenticates the fluent http request using OAuth2.
        /// </summary>
        /// <param name="fluentHttpRequest">The fluent http request.</param>
        public abstract void Authenticate(FluentHttpRequest fluentHttpRequest);
    }

    /// <remarks>http://tools.ietf.org/html/draft-ietf-oauth-v2-bearer-05</remarks>
    abstract class OAuth2BearerAuthenticator : OAuth2Authenticator
    {
        private readonly string _bearerToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2BearerAuthenticator"/> class.
        /// </summary>
        /// <param name="bearerToken">The oauth 2 bearer_token.</param>
        protected OAuth2BearerAuthenticator(string bearerToken)
        {
            if (string.IsNullOrEmpty(bearerToken))
                throw new ArgumentNullException("bearerToken");
            _bearerToken = bearerToken;
        }

        /// <summary>
        /// Gets the bearer_token.
        /// </summary>
        public string BearerToken
        {
            get { return _bearerToken; }
        }
    }

    /// <remarks>http://tools.ietf.org/html/draft-ietf-oauth-v2-bearer-05#section-2.1</remarks>
    class OAuth2AuthorizationRequestHeaderBearerAuthenticator : OAuth2BearerAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2AuthorizationRequestHeaderBearerAuthenticator"/> class.
        /// </summary>
        /// <param name="bearerToken">The oauth 2 bearer_token.</param>
        public OAuth2AuthorizationRequestHeaderBearerAuthenticator(string bearerToken)
            : base(bearerToken)
        {
        }

        /// <summary>
        /// Authenticate the fluent http request using OAuth2 authorization header bearer_token.
        /// </summary>
        /// <param name="fluentHttpRequest">The fluent http request.</param>
        public override void Authenticate(FluentHttpRequest fluentHttpRequest)
        {
            fluentHttpRequest.Headers(h => h.Add("Authorization", string.Concat("Bearer ", BearerToken)));
        }
    }

#if FLUENTHTTP_OAuth2UriQueryParameterBearerAuthenticator

    /// <remarks>http://tools.ietf.org/html/draft-ietf-oauth-v2-bearer-05#section-2.3</remarks>
    class OAuth2UriQueryParameterBearerAuthenticator : OAuth2BearerAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2UriQueryParameterBearerAuthenticator"/> class.
        /// </summary>
        /// <param name="bearerToken">The oauth 2 bearer_token.</param>
        public OAuth2UriQueryParameterBearerAuthenticator(string bearerToken)
            : base(bearerToken)
        {
        }

        /// <summary>
        /// Authenticate the fluent http request using OAuth2 uri querystring parameter bearer_token.
        /// </summary>
        /// <param name="fluentHttpRequest">The fluent http request.</param>
        public override void Authenticate(FluentHttpRequest fluentHttpRequest)
        {
            fluentHttpRequest.QueryStrings(qs => qs.Add("bearer_token", BearerToken));
        }
    }
#endif
}