namespace FluentHttp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents OAuth1 authenticator.
    /// </summary>
    public abstract class OAuthAuthenticator : IFluentAuthenticator
    {
        private const string Digit = "1234567890";
        private const string Lower = "abcdefghijklmnopqrstuvwxyz";

        private static readonly Random _random;
        private static readonly object _randomLock = new object();

        static OAuthAuthenticator()
        {
            _random = new Random();
        }

        /// <summary>
        /// Authenticate the fluent http request using oauth.
        /// </summary>
        /// <param name="fluentHttpRequest">
        /// The fluent http request.
        /// </param>
        public abstract void Authenticate(IFluentHttpRequest fluentHttpRequest);

        /// <summary>
        /// Generates the oauth time stamp for the specified time..
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// Returns the oauth time stamp.
        /// </returns>
        internal static string OAuthTimeStamp(DateTime dateTime)
        {
            return Utils.ToUnixTime(dateTime).ToString("#");
        }

        /// <summary>
        /// Generates the oauth time stamp for the current time.
        /// </summary>
        /// <returns>
        /// Returns the oauth time stamp.
        /// </returns>
        internal static string OAuthTimeStamp()
        {
            return OAuthTimeStamp(DateTime.UtcNow);
        }

        public static string GenerateNonce()
        {
            const string chars = (Lower + Digit);

            var nonce = new char[16];
            lock (_randomLock)
            {
                for (var i = 0; i < nonce.Length; i++)
                {
                    nonce[i] = chars[_random.Next(0, chars.Length)];
                }
            }
            return new string(nonce);
        }

        /// <summary>
        /// Creates a request URL suitable for making OAuth requests.
        /// Resulting URLs must exclude port 80 or port 443 when accompanied by HTTP and HTTPS, respectively.
        /// Resulting URLs must be lower case.
        /// </summary>
        /// <seealso cref="http://oauth.net/core/1.0#rfc.section.9.1.2"/>
        /// <param name="url">The original request URL</param>
        /// <returns></returns>
        internal static string ConstructRequestUrl(Uri url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            var sb = new StringBuilder();

            var requestUrl = string.Format("{0}://{1}", url.Scheme, url.Host);
            var qualified = string.Format(":{0}", url.Port);
            var basic = url.Scheme == "http" && url.Port == 80;
            var secure = url.Scheme == "https" && url.Port == 443;

            sb.Append(requestUrl);
            sb.Append(!basic && !secure ? qualified : string.Empty);
            sb.Append(url.AbsolutePath);

            return sb.ToString(); //.ToLower();
        }

        internal static string ConcatenateRequestElements(string method, string requestTokenUrl, IDictionary<string, string> parameters)
        {
            Contract.Requires(!string.IsNullOrEmpty(method));
            Contract.Requires(!string.IsNullOrEmpty(requestTokenUrl));
            Contract.Requires(parameters != null);

            var sb = new StringBuilder();

            sb.AppendFormat("{0}&{1}&", method.ToUpper(), Uri.EscapeDataString(ConstructRequestUrl(new Uri(requestTokenUrl))));
            sb.Append(Uri.EscapeDataString(NormalizeRequestParameters(parameters)));

            return sb.ToString();
        }

        internal static string NormalizeRequestParameters(IDictionary<string, string> parameters)
        {
            Contract.Requires(parameters != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            var sortedParameters = SortParametersExcludingSignature(parameters);

            var sb = new StringBuilder();
            foreach (var parameter in sortedParameters)
            {
                sb.AppendFormat("{0}={1}&", parameter.Key, parameter.Value);
            }

            if (sortedParameters.Count > 0)
            {
                --sb.Length;
            }

            return sb.ToString();
        }

        internal static IDictionary<string, string> SortParametersExcludingSignature(IDictionary<string, string> parameters)
        {
            Contract.Requires(parameters != null);

            var copy = new Dictionary<string, string>();

            foreach (var parameter in parameters)
            {
                if (!parameter.Key.Equals("oauth_signature", StringComparison.OrdinalIgnoreCase))
                {
                    copy.Add(Utils.UrlEncode(parameter.Key), Utils.UrlEncode(parameter.Value));
                }
            }

            return copy.OrderBy(p => p.Key).ThenBy(p => p.Value).ToDictionary(p => p.Key, p => p.Value);
        }

        internal static string GenerateSignature(string signatureMethod, bool isEscaped, string signatureBase, string consumerSecret, string tokenSecret)
        {
            if (string.IsNullOrEmpty(tokenSecret))
            {
                tokenSecret = string.Empty;
            }

            consumerSecret = Uri.EscapeDataString(consumerSecret);
            tokenSecret = Uri.EscapeDataString(tokenSecret);

            string signature;
            switch (signatureMethod.ToUpper())
            {
                case "HMAC-SHA1":
                    var key = string.Format("{0}&{1}", consumerSecret, tokenSecret);
                    signature = Convert.ToBase64String(Utils.ComputeHmacSha1Hash(Encoding.UTF8.GetBytes(signatureBase), Encoding.UTF8.GetBytes(key)));
                    break;
                default:
                    throw new NotImplementedException("Only HMAC-SHA1 is supported.");
            }

            return isEscaped ? Uri.EscapeDataString(signature) : signature;
        }

        internal static string BuildAuthorizationHeaderValue(IDictionary<string, string> oauthInfo)
        {
            var sb = new StringBuilder("OAuth ");

            foreach (var parameter in oauthInfo)
            {
                sb.AppendFormat("{0}=\"{1}\",", parameter.Key, parameter.Value);
            }

            if (oauthInfo.Count > 0)
            {
                // remove the last comma
                --sb.Length;
            }

            return sb.ToString();
        }
    }

    public abstract class OAuthTemporaryCredentialsAuthenticator : OAuthAuthenticator
    {
        private readonly string consumerKey;
        private readonly string consumerSecret;
        private readonly string signatureMethod;

        public OAuthTemporaryCredentialsAuthenticator(string consumerKey, string consumerSecret)
            : this(consumerKey, consumerSecret, null)
        {
        }

        public OAuthTemporaryCredentialsAuthenticator(string consumerKey, string consumerSecret, string signatureMethod)
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.signatureMethod = string.IsNullOrEmpty(signatureMethod) ? "HMAC-SHA1" : signatureMethod;
        }

        public string SignatureMethod
        {
            get { return signatureMethod; }
        }

        public string ConsumerSecret
        {
            get { return consumerSecret; }
        }

        public string ConsumerKey
        {
            get { return consumerKey; }
        }

        internal protected IDictionary<string, string> BuildRequestTokenInfo(IFluentHttpRequest fluentHttpRequest, string timeStamp, string nonce)
        {
            if (string.IsNullOrEmpty(timeStamp))
            {
                timeStamp = OAuthTimeStamp();
            }

            if (string.IsNullOrEmpty(nonce))
            {
                nonce = GenerateNonce();
            }

            var url = FluentHttpRequest.BuildRequestUrl(fluentHttpRequest);
            var method = fluentHttpRequest.GetMethod().ToUpper();

            var parameters = new Dictionary<string, string>
                                 {
                                     { "oauth_consumer_key", this.ConsumerKey },
                                     { "oauth_nonce", nonce },
                                     { "oauth_signature_method", this.SignatureMethod },
                                     { "oauth_timestamp", timeStamp },
                                     { "oauth_version", "1.0" }
                                 };

            // todo: add other values.

            var signatureBase = ConcatenateRequestElements(method, url, parameters);
            var signature = GenerateSignature(this.SignatureMethod, true, signatureBase, this.ConsumerSecret, null);

            parameters.Add("oauth_signature", signature);

            return parameters;
        }
    }

    public class OAuthTemporaryCredentialsAuthorizationRequestHeaderAuthenticator : OAuthTemporaryCredentialsAuthenticator
    {
        public OAuthTemporaryCredentialsAuthorizationRequestHeaderAuthenticator(string consumerKey, string consumerSecret)
            : base(consumerKey, consumerSecret)
        {
        }

        public override void Authenticate(IFluentHttpRequest fluentHttpRequest)
        {
            var oauthInfo = BuildRequestTokenInfo(fluentHttpRequest, null, null);

            var authorizationValue = BuildAuthorizationHeaderValue(oauthInfo);

            fluentHttpRequest.Headers(h => h.Add("Authorization", authorizationValue));
        }
    }
}