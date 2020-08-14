using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourseProviders;
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
            int standardCode,
            GetCourseProvidersResult response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CoursesController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(standardCode)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.CourseProviders(standardCode) as ViewResult;
            
            //Assert
            var actualModel = actual.Model as CourseProvidersViewModel;
            actualModel.Providers.Should().BeEquivalentTo(response.Providers, options=>options.ExcludingMissingMembers());
            actualModel.Course.Should().BeEquivalentTo((CourseViewModel)response.Course);
            actualModel.Total.Should().Be(response.Total);
        }

        [Test, MoqAutoData]
        public async Task And_Error_Then_Redirect_To_Error_Route(
            int standardCode,
            Exception exception,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CoursesController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(
                    It.Is<GetCourseProvidersQuery>(c => c.CourseId.Equals(standardCode)),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);
            
            //Act
            var actual = await controller.CourseProviders(standardCode) as RedirectToRouteResult;
            
            //Assert
            actual.RouteName.Should().Be(RouteNames.Error500);
        }
    }
}