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
            mockService.Setup(x => x.GetCourses(null)).ReturnsAsync(courseResponse);

            //Act
            var actual = await handler.Handle(request, CancellationToken.None);
            
            //Assert
            mockService.Verify(x=>x.GetCourses(null), Times.Once);
            Assert.IsNotNull(actual);
            actual.Courses.Should().BeEquivalentTo(courseResponse.Courses);
            actual.TotalFiltered.Should().Be(courseResponse.TotalFiltered);
            actual.Total.Should().Be(courseResponse.Total);
        }

        [Test, MoqAutoData]
        public async Task Then_Keywords_Are_Passed_To_The_Service(
            GetCoursesQuery request,
            TrainingCourses courseResponse,
            [Frozen] Mock<ICourseService> mockService,
            GetCoursesQueryHandler handler)
        {
            //Arrange
            mockService.Setup(x => x.GetCourses(request.Keyword)).ReturnsAsync(courseResponse);

            //Act
            var actual = await handler.Handle(request, CancellationToken.None);

            //Assert
            mockService.Verify(x => x.GetCourses(request.Keyword), Times.Once);
            Assert.IsNotNull(actual);
        }
    }
}