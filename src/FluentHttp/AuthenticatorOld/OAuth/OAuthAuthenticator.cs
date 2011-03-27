namespace FluentHttp
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents the OAuth Authenticator.
    /// </summary>
    public abstract class OAuthAuthenticator : IFluentAuthenticator
    {
        public abstract void Authenticate(IFluentHttpRequest fluentHttpRequest);
    }

    /// <summary>
    /// Represents the OAuth authenticator for accessing protected resources.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public abstract class OAuthProtectedResourceAuthenticator : OAuthAuthenticator
    {
        /// <summary>
        /// The consumer key.
        /// </summary>
        private readonly string consumerKey;

        /// <summary>
        /// The consumer secret.
        /// </summary>
        private readonly string consumerSecret;

        /// <summary>
        /// The token.
        /// </summary>
        private readonly string token;

        /// <summary>
        /// The token secret.
        /// </summary>
        private readonly string tokenSecret;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthProtectedResourceAuthenticator"/> class.
        /// </summary>
        /// <param name="consumerKey">
        /// The consumer key.
        /// </param>
        /// <param name="consumerSecret">
        /// The consumer secret.
        /// </param>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <param name="tokenSecret">
        /// The token secret.
        /// </param>
        protected OAuthProtectedResourceAuthenticator(string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.token = token;
            this.tokenSecret = tokenSecret;
        }

        /// <summary>
        /// Gets the consumer key.
        /// </summary>
        public string ConsumerKey
        {
            get { return this.consumerKey; }
        }

        /// <summary>
        /// Gets the consumer secret.
        /// </summary>
        public string ConsumerSecret
        {
            get { return this.consumerSecret; }
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        public string Token
        {
            get { return this.token; }
        }

        /// <summary>
        /// Gets the token secret.
        /// </summary>
        public string TokenSecret
        {
            get { return this.tokenSecret; }
        }
    }

    /// <summary>
    /// Represents the oauth authenticator accessing protected resources using the authorization header.
    /// </summary>
    public class OAuthProtectedResourceAuthorizationRequestHeaderAuthenticator : OAuthProtectedResourceAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthProtectedResourceAuthorizationRequestHeaderAuthenticator"/> class.
        /// </summary>
        /// <param name="consumerKey">
        /// The consumer key.
        /// </param>
        /// <param name="consumerSecret">
        /// The consumer secret.
        /// </param>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <param name="tokenSecret">
        /// The token secret.
        /// </param>
        public OAuthProtectedResourceAuthorizationRequestHeaderAuthenticator(string consumerKey, string consumerSecret, string token, string tokenSecret)
            : base(consumerKey, consumerSecret, token, tokenSecret)
        {
        }

        public override void Authenticate(IFluentHttpRequest fluentHttpRequest)
        {
            var requestUrl = new Uri(FluentHttpRequest.BuildRequestUrl(fluentHttpRequest));

            var value = OAuthUtil.GenerateHeader(requestUrl, this.ConsumerKey, this.ConsumerSecret, this.Token, this.TokenSecret, fluentHttpRequest.GetMethod());

            fluentHttpRequest.Headers(h => h.Add("Authorization", value));
        }
    }
}