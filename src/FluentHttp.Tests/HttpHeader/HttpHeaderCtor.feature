Feature: FluentHttpHeader ctor
	Check if ctor assings value correctly
	and FluentHttpHeader is constructed correctly.

@important
Scenario: Pass header name and header value as ctor params
	Given a null fluent http header
	When I create a new fluent http header with ctor params ("header-name" and "header-value")
		And I get name
		And I get value
	Then name should be "header-name"
		And value should be "header-value"

# if aggressive check
#Scenario: Pass header name as null
#	Given a null fluent http header
#	When I create a new fluent http header with http header as null
#	Then it should throw ArgumentNullException
#
#Scenario: Pass header name as string.Empty
#	Given a null fluent http header
#	When I create a new fluent http header with http header as string.Empty
#	Then it should throw ArgumentNullException
#
#Scenario: Pass header name as ""
#	Given a null fluent http header
#	When I create a new fluent http header with http header as ""
#	Then it should throw ArgumentNullException