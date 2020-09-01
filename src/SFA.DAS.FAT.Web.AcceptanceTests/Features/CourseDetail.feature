Feature: CourseDetail
	As an employer
	I want to know if the start date has passed for a bookmarked course
	So that I can know it is no longer available
	
@WireMockServer
Scenario: Course detail page
	Given I navigate to the following url: /courses/14
	Then an http status code of 200 is returned
	And the page content includes the following: Apprenticeship training course
	And the expired course content is not displayed
	And the last start date alert is not displayed

@WireMockServer
Scenario: Course with last start date
	Given I navigate to the following url: /courses/24 
	Then an http status code of 200 is returned
	And the last start date alert is displayed
	
@WireMockServer	
Scenario: Expired course
	Given I navigate to the following url: /courses/101
	Then an http status code of 200 is returned
	And the expired course content is displayed
	And the last start date alert is not displayed

@WireMockServer
Scenario: Regulated course
	Given I navigate to the following url: /courses/333
	Then an http status code of 200 is returned
	And the regulated occupation header and message is displayed