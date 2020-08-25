Feature: CourseDetail
	As an employer
	I want to see if providers delivering a course need to be regulated 
	So that I know that I should choose a provider with the correct authorisation

@mytag
Scenario: Navigate to course detail page for regulated course
	When I navigate to the following url: /courses/333
	Then an http status code of 200 is returned
    And the regulated occupation header and message is displayed