Feature: CourseProviderDetail
	As an employer
    I want a list of details for a specific provider
	So that I can compare providers and choose

@WireMockServer
Scenario: Navigate to details for a course provider's page
When I navigate to the following url: courses/2/providers/1001?location=coventry
Then an http status code of 200 is returned
And the page content includes the following: This training provider does not offer this course at the apprenticeship location
And the page content includes the following: There are 0 training providers for

