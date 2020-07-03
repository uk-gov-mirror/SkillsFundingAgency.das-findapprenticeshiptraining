using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourses;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Application.UnitTests.Courses.Queries.GetCourses
{
    public class WhenGettingCourses
    {
        
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_The_Data_Returned(
            GetCoursesQuery request,
            TrainingCourses courseResponse,
            [Frozen] Mock<ICourseService> mockService,
            GetCoursesQueryHandler handler)
        {
            //Arrange
            request.Keyword = null;
            request.RouteIds = null;
            request.Codes = null;
            mockService.Setup(x => x.GetCourses(null, null, null)).ReturnsAsync(courseResponse);

            //Act
            var actual = await handler.Handle(request, CancellationToken.None);
            
            //Assert
            mockService.Verify(x=>x.GetCourses(null, null, null), Times.Once);
            Assert.IsNotNull(actual);
            actual.Courses.Should().BeEquivalentTo(courseResponse.Courses);
            actual.Sectors.Should().BeEquivalentTo(courseResponse.Sectors);
            actual.TotalFiltered.Should().Be(courseResponse.TotalFiltered);
            actual.Total.Should().Be(courseResponse.Total);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_Keywords_Are_Returned(
            GetCoursesQuery request,
            TrainingCourses courseResponse,
            [Frozen] Mock<ICourseService> mockService,
            GetCoursesQueryHandler handler)
        {
            //Arrange
            request.RouteIds = null;
            request.Codes = null;
            mockService.Setup(x => x.GetCourses(request.Keyword, null, null)).ReturnsAsync(courseResponse);

            //Act
            var actual = await handler.Handle(request, CancellationToken.None);

            //Assert
            mockService.Verify(x => x.GetCourses(request.Keyword, null, null), Times.Once);
            Assert.IsNotNull(actual);
            actual.Courses.Should().BeEquivalentTo(courseResponse.Courses);
            actual.Sectors.Should().BeEquivalentTo(courseResponse.Sectors);
            actual.TotalFiltered.Should().Be(courseResponse.TotalFiltered);
            actual.Total.Should().Be(courseResponse.Total);
        }

        [Test, MoqAutoData]
        public async Task Then_Keywords_And_Sectors_Are_Passed_To_The_Service_And_Levels_Is_Null(
            GetCoursesQuery request,
            TrainingCourses courseResponse,
            [Frozen] Mock<ICourseService> mockService,
            GetCoursesQueryHandler handler)
        {
            //Arrange
            request.Codes = null;
            mockService.Setup(x => x.GetCourses(request.Keyword, request.RouteIds, null)).ReturnsAsync(courseResponse);

            //Act
            var actual = await handler.Handle(request, CancellationToken.None);

            //Assert
            mockService.Verify(x => x.GetCourses(request.Keyword, request.RouteIds, null), Times.Once);
            Assert.IsNotNull(actual);
        }

        [Test, MoqAutoData]
        public async Task Then_Keywords_And_Levels_Are_Passed_To_The_Service_And_Sectors_Is_Null(
            GetCoursesQuery request,
            TrainingCourses courseResponse,
            [Frozen] Mock<ICourseService> mockService,
            GetCoursesQueryHandler handler)
        {
            //Arrange
            request.RouteIds = null;
            mockService.Setup(x => x.GetCourses(request.Keyword, null, request.Codes)).ReturnsAsync(courseResponse);

            //Act
            var actual = await handler.Handle(request, CancellationToken.None);

            //Assert
            mockService.Verify(x => x.GetCourses(request.Keyword, null, request.Codes), Times.Once);
            Assert.IsNotNull(actual);
        }
    }
}