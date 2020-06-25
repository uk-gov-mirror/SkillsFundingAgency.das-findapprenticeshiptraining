using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourses;
using SFA.DAS.FAT.Web.Controllers;
using SFA.DAS.FAT.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Web.UnitTests.Controllers.CoursesControllerTests
{
    public class WhenGettingCourses
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Sent_And_Data_Retrieved_And_View_Shown(
            CoursesRouteModel routeModel,
            GetCoursesResult response,
            [Frozen] Mock<IMediator> mediator,
            CoursesController controller)
        {
            //Arrange
            mediator.Setup(x => 
                    x.Send(It.Is<GetCoursesQuery>(c => c.Keyword.Equals(routeModel.Keyword)),It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.Courses(routeModel);
            var actualResult = actual as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actualResult);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Keyword_Is_Added_To_The_Query_And_Returned_To_The_View(
            CoursesRouteModel routeModel,
            GetCoursesResult response,
            [Frozen] Mock<IMediator> mediator)
        {
            //Arrange
            var controller = new CoursesController(mediator.Object);
            mediator.Setup(x => x.Send(It.IsAny<GetCoursesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            //Act
            var actual = await controller.Courses(routeModel);

            //Assert
            Assert.IsNotNull(actual);
            var actualResult = actual as ViewResult;
            Assert.IsNotNull(actualResult);
            var actualModel = actualResult.Model as CoursesViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.Keyword.Should().Be(routeModel.Keyword);
            mediator.Verify(
                x => x.Send(It.Is<GetCoursesQuery>(query => query.Keyword == routeModel.Keyword), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}