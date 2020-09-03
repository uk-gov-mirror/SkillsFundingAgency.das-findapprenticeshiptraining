using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Locations.Services;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Domain.Locations.Api;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Application.UnitTests.Locations.Services
{
    public class WhenGettingLocationsByQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Data_Is_Returned_From_The_Api(
            string searchTerm,
            string baseUrl,
            Domain.Locations.Locations apiResponse,
            Mock<IOptions<FindApprenticeshipTrainingApi>> config,
            [Frozen] Mock<IApiClient> apiClient,
            LocationService service)
        {
            //Arrange
            apiClient.Setup(x =>
                    x.Get<Domain.Locations.Locations>(
                        It.Is<GetLocationsApiRequest>(c => c.GetUrl.Contains(searchTerm))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await service.GetLocations(searchTerm);
            
            //Assert
            actual.Should().BeEquivalentTo(apiResponse);
        }
        
    }
}