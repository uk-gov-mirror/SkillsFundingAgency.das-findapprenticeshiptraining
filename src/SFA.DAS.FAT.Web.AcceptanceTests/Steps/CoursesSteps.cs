using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Domain.Courses.Api;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.MockServer;
using SFA.DAS.FAT.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

namespace SFA.DAS.FAT.Web.AcceptanceTests.Steps
{
    [Binding]
    public class CoursesSteps
    {
        private readonly ScenarioContext _context;
        
        public CoursesSteps (ScenarioContext context)
        {
            _context = context;
        }

        [Then("there is a row for each course")]
        public async Task ThenThereIsARowForEachCourse()
        {
            var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);

            var actualContent = await response.Content.ReadAsStringAsync();

            var json = DataFileManager.GetFile("courses.json");
            var expectedApiResponse = JsonConvert.DeserializeObject<TrainingCourses>(json);

            foreach (var course in expectedApiResponse.Courses)
            {
                actualContent.Should().Contain(HttpUtility.HtmlEncode(course.Title));
            }
        }

        [Then("the sectors and levels are available to filter on")]
        public async Task ThenTheSectorsAndLevelsAreAvailableToFilter()
        {
            var response = _context.Get<HttpResponseMessage>(ContextKeys.HttpResponse);

            var actualContent = await response.Content.ReadAsStringAsync();

            var json = DataFileManager.GetFile("courses.json");
            var expectedApiResponse = JsonConvert.DeserializeObject<TrainingCourses>(json);
            foreach (var sector in expectedApiResponse.Sectors)
            {
                actualContent.Should().Contain(HttpUtility.HtmlEncode(sector.Route));
            }
            foreach (var level in expectedApiResponse.Levels)
            {
                actualContent.Should().Contain($"Level {level.Code} - {HttpUtility.HtmlEncode(level.Name)}");
            }
        }

        [Then("the Api is called with the keyword baker")]
        public void WhenISearchForAField()
        {
            var apiClient = _context.Get<Mock<IApiClient>>(ContextKeys.MockApiClient);
            
            apiClient.Verify(x =>
                x.Get<TrainingCourses>(It.Is<GetCoursesApiRequest>(request =>
                    request.Keyword.Equals("baker")
                    )), Times.Once);
            
        }
    }
}