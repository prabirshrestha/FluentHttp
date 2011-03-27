namespace FluentHttp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Text;

    /// <summary>
    /// Represents a http basic authenticator.
    /// </summary>
    public class HttpBasicAuthenticatior : IFluentAuthenticator
    {
        /// <summary>
        /// The username.
        /// </summary>
        private readonly string username;

        /// <summary>
        /// The password.
        /// </summary>
        private readonly string password;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpBasicAuthenticatior"/> class.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        public HttpBasicAuthenticatior(string username, string password)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));

            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return this.username;
            }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return this.password;
            }
        }

        /// <summary>
        /// Authenticate the fluent http request using http basic authentication.
        /// </summary>
        /// <param name="fluentHttpRequest">
        /// The fluent http request.
        /// </param>
        public void Authenticate(IFluentHttpRequest fluentHttpRequest)
        {
            fluentHttpRequest.Headers(headers =>
                headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", this.Username, this.Password)))));
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here."), ContractInvariantMethod]
        private void InvariantObject()
        {
            Contract.Invariant(!string.IsNullOrEmpty(this.username));
            Contract.Invariant(!string.IsNullOrEmpty(this.password));
        }
    }
}