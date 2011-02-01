namespace FluentHttp
{
    using System;

    /// <summary>
    /// Represents OAuth1 authenticator.
    /// </summary>
    public abstract class OAuthAuthenticator : IFluentAuthenticator
    {
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
        internal string OAuthTimeStamp(DateTime dateTime)
        {
            return Utils.ToUnixTime(dateTime).ToString("#");
        }

        /// <summary>
        /// Generates the oauth time stamp for the current time.
        /// </summary>
        /// <returns>
        /// Returns the oauth time stamp.
        /// </returns>
        internal string OAuthTimeStamp()
        {
            return this.OAuthTimeStamp(DateTime.UtcNow);
        }
    }
}