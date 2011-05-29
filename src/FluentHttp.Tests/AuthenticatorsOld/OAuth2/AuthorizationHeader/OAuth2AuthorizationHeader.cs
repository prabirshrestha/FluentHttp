namespace FluentHttp.Tests.Authenticators.OAuth2.AuthorizationHeader
{
    using System;
    using System.Linq;
    using TechTalk.SpecFlow;
    using Xunit;

    [Binding]
    [StepScope(Feature = "OAuth2 Authorization Header")]
    public class OAuth2AuthorizationHeader
    {
        private FluentHttpRequest _fluentHttpRequest;

        [Given(@"a new FluentHttpRequest")]
        public void GivenANewFluentHttpRequest()
        {
            _fluentHttpRequest = new FluentHttpRequest("https://graph.facebook.com");
        }

        [When(@"AuthenticateUsing OAuth2AuthorizationRequestHeaderAuthenticator with access token (.*)")]
        public void WhenAuthenticateUsingOAuth2AuthorizationRequestHeaderAuthenticatorWithAccessToken123(string oauthToken)
        {
            _fluentHttpRequest.AuthenticateUsing(new OAuth2AuthorizationRequestHeaderAuthenticator(oauthToken));
            _fluentHttpRequest.GetAuthenticator().Authenticate(_fluentHttpRequest);
        }

        [Then(@"header should contain Authorization")]
        public void ThenHeaderShouldContainAuthorization()
        {
            var containsAuthorizationHeader =
                _fluentHttpRequest.GetHeaders().Any(
                    h => h.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase));

            Assert.True(containsAuthorizationHeader);
        }

        [Then(@"its value should be ""(.*)""")]
        public void ThenItsValueShouldBeOAuth123(string value)
        {
            var header = _fluentHttpRequest.GetHeaders().Single(h => h.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase));

            Assert.Equal(value, header.Value);
        }


    }
}