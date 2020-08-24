Feature: CourseDetail
	As an employer
	I want to know if the start date has passed for a bookmarked course
	So that I can know it is no longer available

Scenario: Navigate to course detail page for expired course
When I navigate to the following url: /courses/101
Then an http status code of 200 is returned
And there is a message and button to go to course list displayed
