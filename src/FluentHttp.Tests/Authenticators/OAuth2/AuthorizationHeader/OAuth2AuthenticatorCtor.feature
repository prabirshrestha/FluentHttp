Feature: OAuth2Authenticator ctor
	check if ctor assigns value correctly

@important
Scenario: Create a new OAuth2AuthorizationRequestHeaderAuthenticator with value "123"
	Given a new FluentHttpRequest
	When I create OAuth2AuthorizationRequestHeaderAuthenticator with value as "123"
		And I get the OAuthToken
	Then there should be no exception thrown
		And OAuthToken should be 123

# if aggressive check
Scenario: Create a new OAuth2AuthorizationRequestHeaderAuthenticator with value as null
	Given a new FluentHttpRequest
	When I create OAuth2AuthorizationRequestHeaderAuthenticator with value as null
	Then it should throw ArgumentOutOfRangeException

Scenario: Create a new OAuth2AuthorizationRequestHeaderAuthenticator with value as string.Empty
	Given a new FluentHttpRequest
	When I create OAuth2AuthorizationRequestHeaderAuthenticator with value as string.Empty
	Then it should throw ArgumentOutOfRangeException

Scenario: Create a new OAuth2AuthorizationRequestHeaderAuthenticator with value as empty string ""
	Given a new FluentHttpRequest
	When I create OAuth2AuthorizationRequestHeaderAuthenticator with value as  empty string ""
	Then it should throw ArgumentOutOfRangeException

Scenario: Create a new OAuth2AuthorizationRequestHeaderAuthenticator with value as whitespace
	Given a new FluentHttpRequest
	When I create OAuth2AuthorizationRequestHeaderAuthenticator with value as " "
	Then it should throw ArgumentOutOfRangeException