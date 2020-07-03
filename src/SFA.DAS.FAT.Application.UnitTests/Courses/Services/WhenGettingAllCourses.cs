using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Services;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Domain.Courses.Api;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Application.UnitTests.Courses.Services
{
    public class WhenGettingAllCourses
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Client_Is_Called_With_The_Request(
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingApi>> config,
            [Frozen] Mock<IApiClient> apiClient,
            CourseService courseService)
        {
            //Arrange
            var courseApiRequest = new GetCoursesApiRequest(config.Object.Value.BaseUrl, null);
            
            //Act
            await courseService.GetCourses(null, null, null);
            
            //Assert
            apiClient.Verify(x=>x.Get<TrainingCourses>(
                It.Is<GetCoursesApiRequest>(request => request.GetUrl.Equals(courseApiRequest.GetUrl))));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Keyword_And_RouteIds_Are_Added_To_The_Request(
            string keyword,
            List<Guid> routeIds,
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingApi>> config,
            [Frozen] Mock<IApiClient> apiClient,
            CourseService courseService)
        {
            //Arrange
            var coursesApiRequest = new GetCoursesApiRequest(config.Object.Value.BaseUrl, keyword, routeIds);

            //Act
            await courseService.GetCourses(keyword, routeIds, null);

            //Assert
            apiClient.Verify(x =>
                x.Get<TrainingCourses>(It.Is<GetCoursesApiRequest>(request =>
                    request.Keyword.Equals(coursesApiRequest.Keyword)
                    && request.Sectors.Equals(coursesApiRequest.Sectors))));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Keyword_And_Levels_Are_Added_To_The_Request(
        string keyword,
        List<int> levels,
        [Frozen] Mock<IOptions<FindApprenticeshipTrainingApi>> config,
        [Frozen] Mock<IApiClient> apiClient,
        CourseService courseService)
        {
            //Arrange
            var coursesApiRequest = new GetCoursesApiRequest(config.Object.Value.BaseUrl, keyword, null, levels);

            //Act
            await courseService.GetCourses(keyword, null, levels);

            //Assert
            apiClient.Verify(x =>
                x.Get<TrainingCourses>(It.Is<GetCoursesApiRequest>(request =>
                    request.Keyword.Equals(coursesApiRequest.Keyword)
                    && request.Levels.Equals(coursesApiRequest.Levels))));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Keyword_And_Levels_And_Sectors_Are_Added_To_The_Request(
        string keyword,
        List<Guid> routeIds,
        List<int> levels,
        [Frozen] Mock<IOptions<FindApprenticeshipTrainingApi>> config,
        [Frozen] Mock<IApiClient> apiClient,
        CourseService courseService)
        {
            //Arrange
            var coursesApiRequest = new GetCoursesApiRequest(config.Object.Value.BaseUrl, keyword, routeIds, levels);

            //Act
            await courseService.GetCourses(keyword, routeIds, levels);

            //Assert
            apiClient.Verify(x =>
                x.Get<TrainingCourses>(It.Is<GetCoursesApiRequest>(request =>
                    request.Keyword.Equals(coursesApiRequest.Keyword)
                    && request.Levels.Equals(coursesApiRequest.Levels)
                    && request.Sectors.Equals(coursesApiRequest.Sectors))));
        }
    }
}