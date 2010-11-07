namespace FluentHttp
{
    using System.ComponentModel;

    public partial class FluentHttpRequest
    {
        private IFluentAuthenticator _fluentAuthenticator;

        /// <summary>
        /// Sets the authenticator
        /// </summary>
        /// <param name="fluentAuthenticator">
        /// The fluent authenticator.
        /// </param>
        /// <returns>
        /// </returns>
        public FluentHttpRequest AuthenticateUsing(IFluentAuthenticator fluentAuthenticator)
        {
            _fluentAuthenticator = fluentAuthenticator;
            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IFluentAuthenticator GetAuthenticator()
        {
            return _fluentAuthenticator;
        }
    }
}