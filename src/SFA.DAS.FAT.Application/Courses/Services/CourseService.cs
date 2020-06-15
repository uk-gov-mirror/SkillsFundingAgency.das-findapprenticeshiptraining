using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Domain.Courses.Api;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Application.Courses.Services
{
    public class CourseService : ICourseService
    {
        private readonly IApiClient _apiClient;
        private readonly FindApprenticeshipTrainingConfiguration _config;

        public CourseService (IApiClient apiClient, IOptions<FindApprenticeshipTrainingConfiguration> config)
        {
            _apiClient = apiClient;
            _config = config.Value;
        }
        public async Task<Course> GetCourse(int courseId)
        {
            var request = new GetCourseApiRequest(_config.ApiBaseUrl, courseId);

            var response = await _apiClient.Get<Course>(request);

            return response;
        }
    }
}