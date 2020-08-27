using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.MockServer;
using SFA.DAS.FAT.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

namespace SFA.DAS.FAT.Web.AcceptanceTests.Steps
{
    [Binding]
    public sealed class CourseDetailSteps
    {
        private readonly ScenarioContext _context;

        public CourseDetailSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Then("the course details are (not )?displayed")]
        public async Task ThenTheCourseDetailsAreDisplayed(string not)
        {
            var json = DataFileManager.GetFile("course-lastdatestarts.json");
            var expectedApiResponse = JsonConvert.DeserializeObject<TrainingCourse>(json);

            var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);
            var actualContent = await response.Content.ReadAsStringAsync();

            //todo: similar to below, but for each property displayed
            //actualContent.Should().Contain($"{expectedApiResponse.Course.Title} (level {expectedApiResponse.Course.Level}) is available for new starts until {expectedApiResponse.Course.StandardDates.LastDateStarts.GetValueOrDefault():d MMM yyyy}");
        }

        [Then("the last start date alert is (not )?displayed")]
        public async Task ThenTheLastStartDateAlertIsDisplayed(string not)
        {
            var json = DataFileManager.GetFile("course-lastdatestarts.json");
            var expectedApiResponse = JsonConvert.DeserializeObject<TrainingCourse>(json);
            var expectedAlertMessage =
                $"{expectedApiResponse.Course.Title} (level {expectedApiResponse.Course.Level}) " +
                "is available for new starts until " +
                $"{expectedApiResponse.Course.StandardDates.LastDateStarts.GetValueOrDefault():d MMM yyyy}";

            var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);
            var actualContent = await response.Content.ReadAsStringAsync();

            if (not == string.Empty)
            {
                actualContent.Should().Contain(expectedAlertMessage);
            }
            else
            {
                actualContent.Should().NotContain(expectedAlertMessage);
            }
        }

        [Then("the expired course content is (not )?displayed")]
        public async Task ThenTheExpiredCourseContentIsDisplayed(string not) //todo feature can be refactored to just use existing content steps for this
        {
            var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);
            var actualContent = await response.Content.ReadAsStringAsync();

            if (not == String.Empty)
            {
                actualContent.Should().Contain("This apprenticeship training course is no longer available for new starts.");
                actualContent.Should().ContainAll("<a role=\"button\" draggable=\"false\" class=\"govuk-button\" data-module=\"govuk-button\" href=\"/courses\">", "View apprenticeship training courses", "</a>");           
            }
            else
            {
                actualContent.Should().NotContain("This apprenticeship training course is no longer available for new starts.");
                actualContent.Should().NotContainAll("<a role=\"button\" draggable=\"false\" class=\"govuk-button\" data-module=\"govuk-button\" href=\"/courses\">", "View apprenticeship training courses", "</a>");
            }
        }
    }
}
