using System;
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
        public async Task<TrainingCourse> GetCourse(int courseId, double lat, double lon)
        {
            var request = new GetCourseApiRequest(_config.BaseUrl, courseId, lat, lon);

            var response = await _apiClient.Get<TrainingCourse>(request);

            return response;
        }

        public async Task<TrainingCourses> GetCourses(string keyword, List<Guid> requestRouteIds, List<int> requestLevelCodes, OrderBy orderBy)
        {
            var request = new GetCoursesApiRequest(_config.BaseUrl, keyword, requestRouteIds, requestLevelCodes, orderBy);

            var response = await _apiClient.Get<TrainingCourses>(request);

            return response;
        }
        public async Task<TrainingCourseProviderDetails> GetCourseProviderDetails(int providerId, int courseId, string location)
        {
            var request = new GetCourseProviderDetailsApiRequest(_config.BaseUrl,courseId, providerId, location);
            var response = await _apiClient.Get<TrainingCourseProviderDetails>(request);
            return response;
        }

        public async Task<TrainingCourseProviders> GetCourseProviders(
            int courseId,
            string queryLocation, 
            IEnumerable<DeliveryModeType> queryDeliveryModes,
            ProviderSortBy sortBy)
        {
            var request = new GetCourseProvidersApiRequest(_config.BaseUrl, courseId, queryLocation, queryDeliveryModes, (short)sortBy);

            var response = await _apiClient.Get<TrainingCourseProviders>(request);

            return response;
        }
    }
}
