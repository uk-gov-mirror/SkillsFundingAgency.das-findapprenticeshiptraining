using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetProvider;
using SFA.DAS.FAT.Web.Controllers;
using SFA.DAS.FAT.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.FAT.Web.Models;
using System.Collections;
using System.Collections.Generic;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Web.UnitTests.Controllers.CoursesControllerTests
{
    public class WhenGettingCourseProviderDetails
    {

        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Sent_And_Provider_Detail_Retrieved_And_Shown(
            int providerId,
            int courseId,
            string location,
            GetCourseProviderResult response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CoursesController controller
            )
        {
            //Arrange
            mediator.Setup(x => x.Send(It.Is<GetCourseProviderQuery>(c =>
                c.ProviderId.Equals(providerId) && c.CourseId.Equals(courseId) && c.Location.Equals(location)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //Act
            var actual = await controller.CourseProviderDetail(courseId, providerId, location);

            //Assert
            Assert.IsNotNull(actual);
            var actualResult = actual as ViewResult;
            Assert.IsNotNull(actualResult);
            var actualModel = actualResult.Model as CourseProviderViewModel;
            Assert.IsNotNull(actualModel);
            Assert.IsNotNull(actualModel.AdditionalCourses);
            Assert.IsNotNull(actualModel.AdditionalCourses.Courses);
        }
        
        
        [Test, MoqAutoData]
        public async Task Then_The_Location_Is_Added_To_The_Cookie_If_Set(
            int providerId,
            int courseId,
            string location,
            GetCourseProviderResult response,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICookieStorageService<string>> cookieStorageService,
            [Greedy] CoursesController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.Is<GetCourseProviderQuery>(c =>
                    c.ProviderId.Equals(providerId) && c.CourseId.Equals(courseId) && c.Location.Equals(location)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            await controller.CourseProviderDetail(courseId, providerId, location);
            
            //Assert
            cookieStorageService.Verify(x=>x.Update(Constants.LocationCookieName,location,2));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Location_Is_Removed_From_The_Cookie_If_Set_To_Minus_One(
            int providerId,
            int courseId,
            GetCourseProviderResult response,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICookieStorageService<string>> cookieStorageService,
            [Greedy] CoursesController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.Is<GetCourseProviderQuery>(c =>
                    c.ProviderId.Equals(providerId) && c.CourseId.Equals(courseId) && c.Location.Equals("")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            await controller.CourseProviderDetail(courseId, providerId, "-1");
            
            //Assert
            cookieStorageService.Verify(x=>x.Delete(Constants.LocationCookieName));
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_Location_Stored_In_Cookie_And_No_Location_In_Query_It_Is_Used_For_Results_And_Cookie_Updated(
            int providerId,
            int courseId,
            string location,
            GetCourseProviderResult response,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICookieStorageService<string>> cookieStorageService,
            [Greedy] CoursesController controller)
        {
            //Arrange
            cookieStorageService.Setup(x => x.Get(Constants.LocationCookieName)).Returns(location);
            mediator.Setup(x => x.Send(It.Is<GetCourseProviderQuery>(c =>
                    c.ProviderId.Equals(providerId) && c.CourseId.Equals(courseId) && c.Location.Equals(location)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //Act
            var actual = await controller.CourseProviderDetail(courseId, providerId, "");
            
            //Assert
            Assert.IsNotNull(actual);
            var actualResult = actual as ViewResult;
            Assert.IsNotNull(actualResult);
            var actualModel = actualResult.Model as CourseProviderViewModel;
            Assert.IsNotNull(actualModel);
            cookieStorageService.Verify(x=>x.Update(Constants.LocationCookieName,location,2));
        }

        [Test, MoqAutoData]
        public async Task And_Error_Then_Redirect_To_Error_Route(
            int providerId,
            int courseId,
            string location,
            Exception exception,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CoursesController controller
            )
        {
            // Arrange
            mediator.Setup(x => x.Send(It.Is<GetCourseProviderQuery>(c =>
                c.ProviderId.Equals(providerId) && c.CourseId.Equals(courseId)), 
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            var actual = await controller.CourseProviderDetail(courseId, providerId, location) as RedirectToRouteResult;

            // Assert
            actual.RouteName.Should().Be(RouteNames.Error500);
        }
    }
}
