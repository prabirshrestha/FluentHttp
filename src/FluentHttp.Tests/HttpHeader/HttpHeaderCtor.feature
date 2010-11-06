Feature: FluentHttpHeader ctor
	Check if ctor assings value corrently

@important
Scenario: Pass header name and header value as ctor params
	Given a new fluent http header with ctor params ("header-name" and "header-value")
	When I get name
	And I get value
	Then name should be "header-name"
	And value should be "header-value"