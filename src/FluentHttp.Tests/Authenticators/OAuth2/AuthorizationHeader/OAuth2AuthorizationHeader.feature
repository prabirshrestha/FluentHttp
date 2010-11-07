Feature: OAuth2 Authorization Header
	check if OAuth2AuthorizationRequestHeaderAuthenticator's Authenticate method works

@important
Scenario: Create a valid OAuth2AuthorizationRequestHeaderAuthenticator 
	Given a new FluentHttpRequest 
	When AuthenticateUsing OAuth2AuthorizationRequestHeaderAuthenticator with access token 123
	Then header should contain Authorization
		And its value should be "OAuth 123"