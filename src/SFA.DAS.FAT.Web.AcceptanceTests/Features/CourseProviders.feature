Feature: CourseProviders
	As an employer
	I want a list of providers for a particular course
	So that I can compare providers and choose

Scenario: Navigate to providers for course page
Given I have an http client
When I navigate to the following url: /courses/123/providers
Then an http status code of 200 is returned
And the page content includes the following: Training providers for
#and row for each provider
