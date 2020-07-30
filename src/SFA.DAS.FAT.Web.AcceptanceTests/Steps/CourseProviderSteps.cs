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
    public sealed class CourseProviderSteps
    {
        private readonly ScenarioContext _context;

        public CourseProviderSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Then("there is a row for each course provider")]
        public async Task ThenThePageContentIncludesTheFollowing()
        {
            var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);

            var actualContent = await response.Content.ReadAsStringAsync();

            var json = DataFileManager.GetFile("course-providers.json");
            var expectedApiResponse = JsonConvert.DeserializeObject<TrainingCourseProviders>(json);

            foreach (var courseProvider in expectedApiResponse.CourseProviders)
            {
                actualContent.Should().Contain(courseProvider.Name);
            }
        }
    }
}
