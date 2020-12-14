using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
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
            response.Course.StandardDates.LastDateStarts = DateTime.UtcNow.AddDays(5);
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
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as CourseProvidersViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.Should().BeEquivalentTo(new CourseProvidersViewModel(request, response, null), options=>options.Excluding(c=>c.ProviderOrder));
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
            response.Course.StandardDates.LastDateStarts = DateTime.UtcNow.AddDays(5);
            mediator.Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(request.Id) 
                                                        && c.Location.Equals(request.Location)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.CourseProviders(request) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as CourseProvidersViewModel;
            Assert.IsNotNull(actualModel);
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
            response.Course.StandardDates.LastDateStarts = DateTime.UtcNow.AddDays(5);
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
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as CourseProvidersViewModel;
            Assert.IsNotNull(actualModel);
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
            response.Course.StandardDates.LastDateStarts = DateTime.UtcNow.AddDays(5);
            request.Location = string.Empty;
            cookieStorageService
                .Setup(x => x.Get(Constants.LocationCookieName))
                .Returns(location);
            mediator
                .Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(request.Id)
                                                        && c.Location.Equals(location.Name)
                                                        && c.Lon.Equals(location.Lon)
                                                        && c.Lat.Equals(location.Lat)
                                                        ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //Act
            var actual = await controller.CourseProviders(request) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as CourseProvidersViewModel;
            Assert.IsNotNull(actualModel);
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
        public async Task Then_The_Request_Is_Added_To_A_Cookie(
            GetCourseProvidersRequest request,
            GetCourseProvidersResult response,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICookieStorageService<GetCourseProvidersRequest>> cookieStorageService,
            [Greedy] CoursesController controller)
        {
            //Arrange
            response.Course.StandardDates.LastDateStarts = DateTime.UtcNow.AddDays(5);
            mediator.Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(request.Id) 
                                                        && c.Location.Equals(request.Location)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            await controller.CourseProviders(request);
            
            //Assert
            cookieStorageService.Verify(x=>x.Update(
                nameof(GetCourseProvidersRequest),
                It.Is<GetCourseProvidersRequest>(c=>c == request),
                1));
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
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.Error500);
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Course_Then_Page_Not_Found_Returned(
            GetCourseProvidersRequest request,
            GetCourseProvidersResult response,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICookieStorageService<GetCourseProvidersRequest>> cookieStorageService,
            [Greedy] CoursesController controller)
        {
            //Arrange
            response.Course = null;
            mediator.Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(request.Id) 
                                                        && c.Location.Equals(request.Location)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.CourseProviders(request) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.Error404);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Course_Is_After_Last_Start_Then_Redirected_To_Course_Page(
            GetCourseProvidersRequest request,
            GetCourseProvidersResult response,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICookieStorageService<GetCourseProvidersRequest>> cookieStorageService,
            [Greedy] CoursesController controller)
        {
            //Arrange
            response.Course.StandardDates.LastDateStarts = DateTime.UtcNow.AddDays(-1);
            mediator.Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(request.Id) 
                                                        && c.Location.Equals(request.Location)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.CourseProviders(request) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.CourseDetails);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Provider_Position_List_Is_Encoded(
            GetCourseProvidersRequest request,
            GetCourseProvidersResult response,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IDataProtector> protector,
            [Frozen] Mock<IDataProtectionProvider> provider,
            [Greedy] CoursesController controller
            )
        {
            //Arrange
            provider.Setup(x => x.CreateProtector(It.IsAny<string>())).Returns(protector.Object);
            response.Course.StandardDates.LastDateStarts = DateTime.UtcNow.AddDays(5);
            mediator.Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(request.Id) 
                                                        && c.Location.Equals(request.Location)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            await controller.CourseProviders(request);
            
            //Assert
            var expectedProviders = response.Providers.ToList();
            foreach (var responseProvider in expectedProviders)
            {
                protector.Verify(c=>c.Protect(It.Is<byte[]>(
                    x=>x[0].Equals(Encoding.UTF8.GetBytes($"{expectedProviders.IndexOf(responseProvider) + 1}|{response.TotalFiltered}")[0]))), Times.Once);    
            }
        }

        [Test, MoqAutoData]
        public async Task Then_The_Provider_Position_Is_Added_To_The_ViewModel(
            string encodedValue,
            GetCourseProvidersRequest request,
            GetCourseProvidersResult response,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICookieStorageService<GetCourseProvidersRequest>> cookieStorageService,
            [Frozen] Mock<IDataProtector> protector,
            [Frozen] Mock<IDataProtectionProvider> provider,
            [Greedy] CoursesController controller)
        {
            //Arrange
            var encodedData = Encoding.UTF8.GetBytes(encodedValue);
            protector.Setup(sut => sut.Protect(It.IsAny<byte[]>())).Returns(encodedData);
            provider.Setup(x => x.CreateProtector(It.IsAny<string>())).Returns(protector.Object);
            response.Course.StandardDates.LastDateStarts = DateTime.UtcNow.AddDays(5);
            mediator.Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(request.Id) 
                                                        && c.Location.Equals(request.Location)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.CourseProviders(request) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as CourseProvidersViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.ProviderOrder.Count.Should().Be(response.Providers.Count());
            actualModel.ProviderOrder.Select(c => c.Value).All(c => c.Equals(Convert.ToBase64String(encodedData))).Should().BeTrue();
            actualModel.ProviderOrder.Select(c => c.Key).ToList().Should()
                .BeEquivalentTo(response.Providers.Select(c => c.ProviderId).ToList());
        }
    }
}
