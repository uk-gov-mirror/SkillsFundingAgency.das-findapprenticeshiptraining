﻿using System;
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
        public async Task<TrainingCourse> GetCourse(int courseId, double lat, double lon, Guid? shortlistUserId)
        {
            var request = new GetCourseApiRequest(_config.BaseUrl, courseId, lat, lon, shortlistUserId);

            var response = await _apiClient.Get<TrainingCourse>(request);

            return response;
        }

        public async Task<TrainingCourses> GetCourses(string keyword, List<string> requestRouteIds, List<int> requestLevelCodes, OrderBy orderBy, Guid? shortlistUserId)
        {
            var request = new GetCoursesApiRequest(_config.BaseUrl, keyword, requestRouteIds, requestLevelCodes, orderBy, shortlistUserId);

            var response = await _apiClient.Get<TrainingCourses>(request);

            return response;
        }
        public async Task<TrainingCourseProviderDetails> GetCourseProviderDetails(    int providerId, int courseId,
            string location, double lat, double lon, Guid shortlistUserId)
        {
            var request = new GetCourseProviderDetailsApiRequest(_config.BaseUrl,courseId, providerId, location,shortlistUserId, lat, lon);
            var response = await _apiClient.Get<TrainingCourseProviderDetails>(request);
            return response;
        }

        public async Task<TrainingCourseProviders> GetCourseProviders(int courseId,
            string queryLocation,
            IEnumerable<DeliveryModeType> queryDeliveryModes,
            IEnumerable<ProviderRating> queryProviderRatings,
            double lat,
            double lon, Guid? shortlistUserId)
        {
            var request = new GetCourseProvidersApiRequest(_config.BaseUrl, courseId, queryLocation, queryDeliveryModes, queryProviderRatings,0, lat, lon, shortlistUserId);

            var response = await _apiClient.Get<TrainingCourseProviders>(request);

            return response;
        }
    }
}
