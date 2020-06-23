using System.Collections.Generic;
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
        private readonly FindApprenticeshipTrainingApi _config;

        public CourseService (IApiClient apiClient, IOptions<FindApprenticeshipTrainingApi> config)
        {
            _apiClient = apiClient;
            _config = config.Value;
        }
        public async Task<TrainingCourse> GetCourse(int courseId)
        {
            var request = new GetCourseApiRequest(_config.BaseUrl, courseId);

            var response = await _apiClient.Get<TrainingCourse>(request);

            return response;
        }

        public async Task<TrainingCourses> GetCourses(string keyword)
        {
            var request = new GetCoursesApiRequest(_config.BaseUrl, keyword);

            var response = await _apiClient.Get<TrainingCourses>(request);

            return response;
        }
    }
}