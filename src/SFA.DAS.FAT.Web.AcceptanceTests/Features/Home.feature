Feature: Home
	As a FAT user
	I want a clear home page
	So that it is clear what actions I can take

@mytag
Scenario: Navigate to start page
Given I have an http client
When I navigate to the following url: /
Then an http status code of 200 is returned
