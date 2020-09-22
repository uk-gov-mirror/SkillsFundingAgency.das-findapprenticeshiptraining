using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
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
    public class WhenGettingCourseProviders
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Client_Is_Called_With_The_Request_Url(
            int courseId,
            string location,
            List<DeliveryModeType> deliveryModes,
            ProviderSortBy sortBy,
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingApi>> mockConfig,
            [Frozen] Mock<IApiClient> mockApiClient,
            CourseService service)
        {
            var expectedUrl = new GetCourseProvidersApiRequest(mockConfig.Object.Value.BaseUrl, courseId, location, deliveryModes).GetUrl;

            await service.GetCourseProviders(courseId, location, deliveryModes, sortBy);

            mockApiClient.Verify(client => client.Get<TrainingCourseProviders>(
                It.Is<GetCourseProvidersApiRequest>(request => request.GetUrl == expectedUrl)));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Response_Is_Returned(
            int courseId,
            string location,
            List<DeliveryModeType> deliveryModes,
            ProviderSortBy sortBy,
            TrainingCourseProviders providersFromApi,
            [Frozen] Mock<IApiClient> mockApiClient,
            CourseService service)
        {
            mockApiClient
                .Setup(client => client.Get<TrainingCourseProviders>(
                    It.IsAny<GetCourseProvidersApiRequest>()))
                .ReturnsAsync(providersFromApi);

            var response = await service.GetCourseProviders(courseId, location, deliveryModes, sortBy);

            response.Course.Should().Be(providersFromApi.Course);
            response.CourseProviders.Should().BeEquivalentTo(providersFromApi.CourseProviders);
            response.Total.Should().Be(providersFromApi.Total);
        }
    }
}
