Feature: CourseDetail
	As an employer
	I want to know if the start date has passed for a bookmarked course
	So that I can know it is no longer available

Scenario: Course detail page
When I navigate to the following url: /courses/14
Then an http status code of 200 is returned
And the page content includes the following: Apprenticeship training course
#todo: and course details are displayed
And the expired course content is not displayed
And the last start date alert is not displayed

Scenario: Course with last start date
When I navigate to the following url: /courses/24 
Then an http status code of 200 is returned
And the last start date alert is displayed

Scenario: Expired course
When I navigate to the following url: /courses/101
Then an http status code of 200 is returned
And the expired course content is displayed
And the last start date alert is not displayed
#todo: and course details are not displayed

Scenario: Regulated course
When I navigate to the following url: /courses/333
Then an http status code of 200 is returned
And the regulated occupation header and message is displayed