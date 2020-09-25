using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourse;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Controllers;
using SFA.DAS.FAT.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Web.UnitTests.Controllers.CoursesControllerTests
{
    public class WhenGettingCourseDetails
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Sent_And_Data_Retrieved_And_View_Shown(
            int standardCode,
            GetCourseResult response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]CoursesController controller)
        {
            //Arrange
            mediator.Setup(x => 
                    x.Send(It.Is<GetCourseQuery>(c => 
                        c.CourseId.Equals(standardCode)),It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.CourseDetail(standardCode);
            
            //Assert
            Assert.IsNotNull(actual);
            var actualResult = actual as ViewResult;
            Assert.IsNotNull(actualResult);
            var actualModel = actualResult.Model as CourseViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.TotalProvidersCount.Should().Be(response.ProvidersCount.TotalProviders);
            actualModel.ProvidersAtLocationCount.Should().Be(response.ProvidersCount.ProvidersAtLocation);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Location_Cookie_The_Lat_Lon_Are_Passed_To_The_Query(
            int standardCode,
            GetCourseResult response,
            LocationCookieItem locationCookieItem,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICookieStorageService<LocationCookieItem>> cookieStorageService, 
            [Greedy]CoursesController controller)
        {
            //Arrange
            cookieStorageService.Setup(x => x.Get(Constants.LocationCookieName))
                .Returns(locationCookieItem);
            mediator.Setup(x => 
                    x.Send(It.Is<GetCourseQuery>(c => 
                        c.CourseId.Equals(standardCode)
                        && c.Lat.Equals(locationCookieItem.Lat)
                        && c.Lon.Equals(locationCookieItem.Lon)
                        ),It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.CourseDetail(standardCode);
            
            //Assert
            Assert.IsNotNull(actual);
            var actualResult = actual as ViewResult;
            Assert.IsNotNull(actualResult);
            var actualModel = actualResult.Model as CourseViewModel;
            Assert.IsNotNull(actualModel);
        }
    }
}
