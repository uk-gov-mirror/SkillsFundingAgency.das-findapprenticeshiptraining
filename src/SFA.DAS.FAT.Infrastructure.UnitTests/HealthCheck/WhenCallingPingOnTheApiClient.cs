using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Infrastructure.Api;
using SFA.DAS.FAT.Infrastructure.HealthCheck;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Infrastructure.UnitTests.HealthCheck
{
    public class WhenCallingPingOnTheApiClient
    {
        [Test, MoqAutoData]
        public async Task Then_The_Ping_Endpoint_Is_Called_For_OuterApi(
            [Frozen] Mock<ApiClient> client,
            HealthCheckContext healthCheckContext,
            FatOuterApiHealthCheck healthCheck)
        {
            //Act
            await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);
            //Assert
            client.Verify(x => x.Ping(), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_It_Is_Successful_200_Is_Returned(
            [Frozen] Mock<ApiClient> client,
            HealthCheckContext healthCheckContext,
            FatOuterApiHealthCheck healthCheck)
        {
            //Arrange
            client.Setup(x => x.Ping())
                .ReturnsAsync(200);
            //Act
            var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);
            //Assert
            Assert.AreEqual(200, actual);
        }

        [Test, MoqAutoData]
        public async Task Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown(
            [Frozen] Mock<ApiClient> client,
            HealthCheckContext healthCheckContext,
            FatOuterApiHealthCheck healthCheck)
        {
            //Arrange
            client.Setup(x => x.Ping())
                .ReturnsAsync(404);
            //Act
            var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);
            //Assert
            Assert.AreNotEqual(200, actual);
        }
    }
}