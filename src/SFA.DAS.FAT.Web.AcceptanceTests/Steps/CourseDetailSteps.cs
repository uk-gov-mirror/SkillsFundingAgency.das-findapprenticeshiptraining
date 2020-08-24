using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

        [Then("there is a message and button to course list displayed if course last start date has passed")]
        public async Task ThenThePageContentIncludesTheFollowing()
        {
            var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);

            var actualContent = await response.Content.ReadAsStringAsync();

            var json = DataFileManager.GetFile("course-expired.json");
            var expectedApiResponse = JsonConvert.DeserializeObject<TrainingCourse>(json);

            if (DateTime.Now > expectedApiResponse.Course.StandardDates.LastDateStarts)
            {
                actualContent.Should().Contain("This apprenticeship training course is no longer available for new starts.");
                actualContent.Should().ContainAll("<a role=\"button\" draggable=\"false\" class=\"govuk-button\" data-module=\"govuk-button\" href=\"/courses\">", "View apprenticeship training courses", "</a>");
            }
        }
    }
}
