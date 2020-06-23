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
            await courseService.GetCourses(null);
            
            //Assert
            apiClient.Verify(x=>x.Get<TrainingCourses>(
                It.Is<GetCoursesApiRequest>(request => request.GetUrl.Equals(courseApiRequest.GetUrl))));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Keyword_Is_Added_To_The_Request(
            string keyword,
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingApi>> config,
            [Frozen] Mock<IApiClient> apiClient,
            CourseService courseService)
        {
            //Arrange
            var coursesApiRequest = new GetCoursesApiRequest(config.Object.Value.BaseUrl, keyword);

            //Act
            await courseService.GetCourses(keyword);

            //Assert
            apiClient.Verify(x =>
                x.Get<TrainingCourses>(It.Is<GetCoursesApiRequest>(request =>
                    request.Keyword.Equals(coursesApiRequest.Keyword))));
        }
    }
}