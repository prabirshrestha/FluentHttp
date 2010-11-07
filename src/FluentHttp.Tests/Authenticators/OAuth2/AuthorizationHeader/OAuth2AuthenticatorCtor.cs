namespace FluentHttp.Tests.Authenticators.OAuth2.AuthorizationHeader
{
    using System;
    using TechTalk.SpecFlow;
    using Xunit;

    [Binding]
    [StepScope(Feature = "OAuth2Authenticator ctor")]
    public class OAuth2AuthenticatorCtor
    {
        private FluentHttpRequest _fluentHttpRequest;
        private string _oauthToken;

        [Given(@"a new FluentHttpRequest")]
        public void GivenANewFluentHttpRequest()
        {
            _fluentHttpRequest = new FluentHttpRequest("https://graph.facebook.com");
        }

        [When(@"I create OAuth2AuthorizationRequestHeaderAuthenticator with value as ""(.*)""")]
        public void WhenICreateOAuth2AuthorizationRequestHeaderAuthenticatorWithValue123(string oauthToken)
        {
            try
            {
                _fluentHttpRequest.AuthenticateUsing(new OAuth2AuthorizationRequestHeaderAuthenticator(oauthToken));
                _fluentHttpRequest.GetAuthenticator().Authenticate(_fluentHttpRequest);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"I get the OAuthToken")]
        public void WhenIGetTheOAuthToken()
        {
            _oauthToken = (_fluentHttpRequest.GetAuthenticator() as OAuth2Authenticator).OAuthToken;
        }

        [Then(@"there should be no exception thrown")]
        public void ThenThereShouldBeNoExceptionThrown()
        {
            Assert.Null(_exception);
        }


        [Then(@"OAuthToken should be (.*)")]
        public void ThenOAuthTokenShouldBe123(string oauthToken)
        {
            Assert.Equal(oauthToken, _oauthToken);
        }

#if AGGRESSIVE_CHECK

        private Exception _exception;

        [Then(@"it should throw ArgumentOutOfRangeException")]
        public void ThenItShouldThrowArgumentNullException()
        {
            Assert.NotNull(_exception);
            Assert.Equal(typeof(ArgumentOutOfRangeException), _exception.GetType());
        }

        [When(@"I create OAuth2AuthorizationRequestHeaderAuthenticator with value as null")]
        public void WhenICreateOAuth2AuthorizationRequestHeaderAuthenticatorWithValueAsNull()
        {
            try
            {
                _fluentHttpRequest.AuthenticateUsing(new OAuth2AuthorizationRequestHeaderAuthenticator(null));
                _fluentHttpRequest.GetAuthenticator().Authenticate(_fluentHttpRequest);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"I create OAuth2AuthorizationRequestHeaderAuthenticator with value as string\.Empty")]
        public void WhenICreateOAuth2AuthorizationRequestHeaderAuthenticatorWithValueAsString_Empty()
        {
            try
            {
                _fluentHttpRequest.AuthenticateUsing(new OAuth2AuthorizationRequestHeaderAuthenticator(string.Empty));
                _fluentHttpRequest.GetAuthenticator().Authenticate(_fluentHttpRequest);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"I create OAuth2AuthorizationRequestHeaderAuthenticator with value as  empty string """"")]
        public void WhenICreateOAuth2AuthorizationRequestHeaderAuthenticatorWithValueAsEmptyString()
        {
            try
            {
                _fluentHttpRequest.AuthenticateUsing(new OAuth2AuthorizationRequestHeaderAuthenticator(""));
                _fluentHttpRequest.GetAuthenticator().Authenticate(_fluentHttpRequest);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

#endif
    }
}