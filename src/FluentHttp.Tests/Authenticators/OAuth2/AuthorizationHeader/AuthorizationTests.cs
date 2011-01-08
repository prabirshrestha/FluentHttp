namespace FluentHttp.Tests.Authenticators.OAuth2.AuthorizationHeader
{
    using Xunit;

    public class AuthorizationTests
    {
        [Fact]
        public void Authenticate_WhenAuthenticate_ShouldContainAuthorizationHeader()
        {
            var request = new FluentHttpRequest("http://www.prabir.me")
                .AuthenticateUsing(new OAuth2AuthorizationRequestHeaderAuthenticator("oauthtoken"));

            request.GetAuthenticator().Authenticate(request);

            var headers = request.GetHeaders();

            var containsAuthorizationHeader = false;

            foreach (var fluentHttpHeader in headers)
            {
                if (fluentHttpHeader.Name.Equals("Authorization"))
                {
                    containsAuthorizationHeader = true;
                    break;
                }
            }

            Assert.True(containsAuthorizationHeader);
        }

        [Fact]
        public void Authenticate_WhenAuthenticate_ShouldContainAuthorizationHeaderWithOAuthToken()
        {
            var oauthToken = "oauthtoken";
            var request = new FluentHttpRequest("http://www.prabir.me")
                .AuthenticateUsing(new OAuth2AuthorizationRequestHeaderAuthenticator(oauthToken));

            request.GetAuthenticator().Authenticate(request);

            var headers = request.GetHeaders();

            string authorizationHeaderValue = string.Empty;
            foreach (var fluentHttpHeader in headers)
            {
                if (fluentHttpHeader.Name.Equals("Authorization"))
                {
                    authorizationHeaderValue = fluentHttpHeader.Value;
                    break;
                }
            }

            Assert.Equal("OAuth " + oauthToken, authorizationHeaderValue);
        }
    }
}