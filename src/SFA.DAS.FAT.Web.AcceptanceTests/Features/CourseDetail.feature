Feature: CourseDetail
	As an employer
	I want to view the details a particular course
	So that I can see if it is suitable for my needs

Scenario: Navigate to course detail page
When I navigate to the following url: /courses/123
Then an http status code of 200 is returned
And there is a message and button to course list displayed if course last start date has passed
