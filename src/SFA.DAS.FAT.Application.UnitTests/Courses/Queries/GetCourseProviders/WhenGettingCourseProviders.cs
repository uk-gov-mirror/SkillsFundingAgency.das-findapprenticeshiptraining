using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Application.UnitTests.Courses.Queries.GetCourseProviders
{
    public class WhenGettingCourseProviders
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Results_From_Service(
            GetCourseProvidersQuery query,
            TrainingCourseProviders providersFromService,
            [Frozen] Mock<ICourseService> mockService,
            GetCourseProvidersQueryHandler handler)
        {
            mockService
                .Setup(service => service.GetCourseProviders(query.CourseId, query.Location))
                .ReturnsAsync(providersFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Course.Should().BeEquivalentTo(providersFromService.Course);
            result.Providers.Should().BeEquivalentTo(providersFromService.CourseProviders);
            result.Total.Should().Be(providersFromService.Total);
        }
    }
}
