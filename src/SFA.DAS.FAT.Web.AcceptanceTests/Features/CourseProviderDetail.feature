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
And the page content includes the following: View other training providers
	
@WireMockServer	
Scenario: No feedback on provider
Given I navigate to the following url: courses/2/providers/1001?location=london
Then an http status code of 200 is returned
And the page content includes the following: Not yet reviewed

@WireMockServer
Scenario: Feedback on provider
Given I navigate to the following url: courses/2/providers/1001
Then an http status code of 200 is returned
And the page content includes the following: (51 reviews)	

@WireMockServer
Scenario: Course offered at location
When I navigate to the following url: courses/14/providers/1001?location=Camden
Then an http status code of 200 is returned
And the page content includes the following: is 1 of
And the page content includes the following: in the apprenticeship location

@WireMockServer
Scenario: No location does not show apprenticeship location message
	When I navigate to the following url: courses/14/providers/1001
	Then an http status code of 200 is returned
	And the page content does not include the following: in the apprenticeship location
 


