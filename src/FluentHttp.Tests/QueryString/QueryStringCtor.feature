Feature: FluentQueryString ctor
	Check if ctor assigns value correctly
	and FluentQueryString is constructed correctly.

@important
Scenario: Pass querystring name and value as ctor params
	Given a null fluent querystring header
	When I create a new fluent querystring with ctor params ("qs-name" and "qs-value")
		And I get name
		And I get value
	Then name should be "qs-name"
		And value should be "qs-value"