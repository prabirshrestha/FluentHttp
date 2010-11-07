Feature: FluentQueryString ctor
	Check if ctor assigns value correctly
	and FluentQueryString is constructed correctly.

@important
Scenario: Pass querystring name and value as ctor params
	Given a null fluent querystring
	When I create a new fluent querystring with ctor params ("qs-name" and "qs-value")
		And I get name
		And I get value
	Then there should be no exception thrown
		And name should be "qs-name"
		And value should be "qs-value"

# if aggressive check
Scenario: Pass querystring name as null
	Given a null fluent querystring
	When I create a new fluent querystring with querystring name as null
	Then it should throw ArgumentOutOfRangeException

Scenario: Pass querystring as string.Empty
	Given a null fluent querystring
	When I create a new fluent querystring with querystring name as string.Empty
	Then it should throw ArgumentOutOfRangeException

Scenario: Pass querystring as ""
	Given a null fluent querystring
	When I create a new fluent querystring with querystring name as ""
	Then it should throw ArgumentOutOfRangeException

Scenario: Pass querystring as white space.
	Given a null fluent querystring
	When I create a new fluent querystring with querystring name as " "
	Then it should throw ArgumentOutOfRangeException