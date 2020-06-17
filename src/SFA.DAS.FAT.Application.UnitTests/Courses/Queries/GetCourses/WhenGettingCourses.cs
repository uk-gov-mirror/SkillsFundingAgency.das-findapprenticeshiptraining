using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourse;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourses;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Application.UnitTests.Courses.Queries.GetCourse
{
    public class WhenGettingCourses
    {
        
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_The_Data_Returned(
            GetCoursesQuery request,
            List<Course> courseResponse,
            [Frozen] Mock<ICourseService> mockService,
            GetCoursesQueryHandler handler)
        {
            //Arrange
            mockService.Setup(x => x.GetCourses()).ReturnsAsync(courseResponse);

            //Act
            var actual = await handler.Handle(request, CancellationToken.None);
            
            //Assert
            mockService.Verify(x=>x.GetCourses(), Times.Once);
            Assert.IsNotNull(actual);
            actual.Courses.Should().BeEquivalentTo(courseResponse);
        }
    }
}