// Copyright 2010 Prabir Shrestha
// 
// https://github.com/prabirshrestha/FluentHttp
//   
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//    http://www.apache.org/licenses/LICENSE-2.0
//   
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace FluentHttp.Authenticators
{
    using System;
    using System.Text;
    using global::FluentHttp;

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