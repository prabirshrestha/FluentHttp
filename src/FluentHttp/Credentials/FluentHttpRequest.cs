namespace FluentHttp
{
    using System.ComponentModel;
    using System.Net;

    public partial class FluentHttpRequest
    {
        private ICredentials _credentials;

        /// <summary>
        /// Set credentials
        /// </summary>
        /// <param name="credentials">
        /// The credentials.
        /// </param>
        /// <returns>
        /// FluentHttpRequest
        /// </returns>
        public FluentHttpRequest Credentials(ICredentials credentials)
        {
            _credentials = credentials;
            return this;
        }

        /// <summary>
        /// Gets the Credentials
        /// </summary>
        /// <returns>
        /// Returns ICredentials
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICredentials GetCredentials()
        {
            return _credentials;
        }
    }
}