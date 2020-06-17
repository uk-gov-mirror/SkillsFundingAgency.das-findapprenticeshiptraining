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
            int courseId,
            string baseUrl,
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingApi>> config,
            [Frozen] Mock<IApiClient> apiClient,
            CourseService courseService)
        {
            //Arrange
            
            var courseApiRequest = new GetCoursesApiRequest(config.Object.Value.BaseUrl);
            
            //Act
            await courseService.GetCourses();
            
            //Assert
            apiClient.Verify(x=>x.GetAll<Course>(
                It.Is<GetCoursesApiRequest>(request => request.GetAllUrl.Equals(courseApiRequest.GetAllUrl))));
        }
    }
}