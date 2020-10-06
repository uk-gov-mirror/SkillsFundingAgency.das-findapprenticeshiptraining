Feature: CourseProviderDetail
	As an employer
    I want a list of details for a specific provider
	So that I can compare providers and choose

@WireMockServer
Scenario: Course not offered at location
Given I navigate to the following url: courses/2/providers/1001?location=coventry
Then an http status code of 200 is returned
And the page content includes the following: This training provider does not offer this course at the apprenticeship location
And the page content includes the following: There are 4 training providers for
	
@WireMockServer	
Scenario: No feedback on provider
Given I navigate to the following url: courses/2/providers/1001?location=london
Then an http status code of 200 is returned
And the page content includes the following: Not yet reviewed

@WireMockServer
Scenario: Feedback on provider
Given I navigate to the following url: courses/2/providers/1001
Then an http status code of 200 is returned
And the page content includes the following: (50&#x2B; reviews)	


