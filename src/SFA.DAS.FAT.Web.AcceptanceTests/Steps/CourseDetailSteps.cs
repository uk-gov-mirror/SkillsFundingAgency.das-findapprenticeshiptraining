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

        [Then("the regulated occupation header and message is displayed")]
        public async Task ThenThePageContentIncludesTheFollowing()
        {
            var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);
            var json = DataFileManager.GetFile("course-regulated.json");
            var expectedApiResponse = JsonConvert.DeserializeObject<TrainingCourse>(json);

            var actualContent = await response.Content.ReadAsStringAsync();

            actualContent.Should().Contain("<h3 class=\"govuk-heading-m govuk-!-margin-bottom-2\">Regulated occupation</h3>");
            actualContent.Should().Contain($"<p class=\"govuk-body\">{expectedApiResponse.Course.Title} (level {expectedApiResponse.Course.Level}) needs a training provider who is approved by the appropriate regulatory body.</p>");
        }
    }
}
