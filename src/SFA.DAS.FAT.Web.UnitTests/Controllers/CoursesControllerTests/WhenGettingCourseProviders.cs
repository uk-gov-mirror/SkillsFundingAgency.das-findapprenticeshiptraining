using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Controllers;
using SFA.DAS.FAT.Web.Infrastructure;
using SFA.DAS.FAT.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Web.UnitTests.Controllers.CoursesControllerTests
{
    public class WhenGettingCourseProviders
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Sent_And_Data_Retrieved_And_View_Shown(
            GetCourseProvidersRequest request,
            GetCourseProvidersResult response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CoursesController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(request.Id) 
                    && c.Location.Equals(request.Location)
                    && c.DeliveryModes.SequenceEqual(request.DeliveryModes.Select(type => (Domain.Courses.DeliveryModeType)type))
                    && c.ProviderRatings.SequenceEqual(request.ProviderRatings.Select(type => (Domain.Courses.ProviderRating)type))),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.CourseProviders(request) as ViewResult;
            
            //Assert
            var actualModel = actual.Model as CourseProvidersViewModel;
            actualModel.Should().BeEquivalentTo(new CourseProvidersViewModel(request, response));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Location_Is_Added_To_The_Cookie_If_Set(
            GetCourseProvidersRequest request,
            GetCourseProvidersResult response,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICookieStorageService<LocationCookieItem>> cookieStorageService,
            [Greedy] CoursesController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(request.Id) 
                                                        && c.Location.Equals(request.Location)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.CourseProviders(request) as ViewResult;
            var actualModel = actual.Model as CourseProvidersViewModel;
            
            //Assert
            cookieStorageService.Verify(x=>x.Update(Constants.LocationCookieName,
                It.Is<LocationCookieItem>(c=>c.Name.Equals(response.Location)
                                          && c.Lat.Equals(response.LocationGeoPoint.FirstOrDefault())
                                          && c.Lon.Equals(response.LocationGeoPoint.LastOrDefault())),2));
            actualModel.HasLocation.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Location_Is_Removed_From_The_Cookie_If_Set_To_Minus_One(
            GetCourseProvidersRequest request,
            GetCourseProvidersResult response,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICookieStorageService<LocationCookieItem>> cookieStorageService,
            [Greedy] CoursesController controller)
        {
            //Arrange
            response.Location = string.Empty;
            request.Location = "-1";
            response.Location = null;
            mediator.Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(request.Id) 
                                                        && c.Location.Equals(string.Empty)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.CourseProviders(request) as ViewResult;
            
            //Assert
            var actualModel = actual.Model as CourseProvidersViewModel;
            actualModel.HasLocation.Should().BeFalse();
            cookieStorageService.Verify(x=>x.Delete(Constants.LocationCookieName));
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_Location_Stored_In_Cookie_It_Is_Used_For_Results_And_Cookie_Updated(
            GetCourseProvidersRequest request,
            LocationCookieItem location,
            GetCourseProvidersResult response,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICookieStorageService<LocationCookieItem>> cookieStorageService,
            [Greedy] CoursesController controller)
        {
            //Arrange
            cookieStorageService
                .Setup(x => x.Get(Constants.LocationCookieName))
                .Returns(location);
            mediator
                .Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(request.Id)
                                                        && c.Location.Equals(request.Location)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //Act
            var actual = await controller.CourseProviders(request) as ViewResult;
            
            //Assert
            var actualModel = actual.Model as CourseProvidersViewModel;
            actualModel.Providers.Should().BeEquivalentTo(response.Providers.Select(provider => (ProviderViewModel)provider));
            actualModel.HasLocation.Should().BeTrue();
            cookieStorageService.Verify(x=>x.Update(Constants.LocationCookieName,
                It.Is<LocationCookieItem>(c=>
                    c.Name.Equals(response.Location)
                    && c.Lat.Equals(response.LocationGeoPoint.FirstOrDefault())
                    && c.Lon.Equals(response.LocationGeoPoint.LastOrDefault())
                    )
                ,2));
        }

        [Test, MoqAutoData]
        public async Task And_Error_Then_Redirect_To_Error_Route(
            GetCourseProvidersRequest request,
            Exception exception,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CoursesController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(request.Id)),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);
            
            //Act
            var actual = await controller.CourseProviders(request) as RedirectToRouteResult;
            
            //Assert
            actual.RouteName.Should().Be(RouteNames.Error500);
        }
    }
}
