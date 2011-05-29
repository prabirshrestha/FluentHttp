
namespace FluentHttp.Authenticators
{
    using System;
    using System.Text;

    /// <summary>
    /// Represents a http basic authenticator.
    /// </summary>
    class HttpBasicAuthenticator : IFluentAuthenticator
    {
        /// <summary>
        /// The username.
        /// </summary>
        private readonly string _username;

        /// <summary>
        /// The password.
        /// </summary>
        private readonly string _password;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpBasicAuthenticator"/> class.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public HttpBasicAuthenticator(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(username);
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(password);

            _username = username;
            _password = password;
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username { get { return _username; } }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get { return _password; } }

        /// <summary>
        /// Authenticates the fluent http request using http basic authentication.
        /// </summary>
        /// <param name="fluentHttpRequest">
        /// The fluent http request.
        /// </param>
        public void Authenticate(FluentHttpRequest fluentHttpRequest)
        {
            fluentHttpRequest.Headers(
                headers =>
                headers.Add(
                    "Authorization",
                    string.Concat("Basic ",
                                  Convert.ToBase64String(
                                      Encoding.UTF8.GetBytes(string.Format("{0}:{1}", Username, Password))))));
        }
    }
}