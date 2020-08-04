using System;
using System.Collections.Generic;
using System.Text;
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
    public class WhenGettingACourseProviderDetails
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Client_Is_Called_With_The_Request(
            int providerId,
            int courseId,
            string baseUrl,
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingApi>> config,
            [Frozen] Mock<IApiClient> apiClient,
            CourseService courseService)
        {
            //Arrange
            var courseApiRequest = new GetCourseProviderDetailsApiRequest(config.Object.Value.BaseUrl, courseId, providerId);

            //Act
            await courseService.GetCourseProviderDetails(providerId, courseId);

            //Assert
            apiClient.Verify(x => x.Get<TrainingCourseProviderDetails>(
                It.Is<GetCourseProviderDetailsApiRequest>(request => request.GetUrl.Equals(courseApiRequest.GetUrl))));
        }
    }
}
