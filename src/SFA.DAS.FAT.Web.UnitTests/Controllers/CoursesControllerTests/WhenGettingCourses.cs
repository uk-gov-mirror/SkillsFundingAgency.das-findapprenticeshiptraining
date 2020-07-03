using System.Linq;
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
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Web.Controllers;
using SFA.DAS.FAT.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Web.UnitTests.Controllers.CoursesControllerTests
{
    public class WhenGettingCourses
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Sent_And_Data_Retrieved_And_View_Shown(
            GetCoursesRequest request,
            GetCoursesResult response,
            [Frozen] Mock<IMediator> mediator,
            CoursesController controller)
        {
            //Arrange
            mediator.Setup(x => 
                    x.Send(It.Is<GetCoursesQuery>(c => c.Keyword.Equals(request.Keyword)),It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.Courses(request);
            var actualResult = actual as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actualResult);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Keyword_And_Sectors_And_Levels_Are_Added_To_The_Query_And_Returned_To_The_View(
            GetCoursesRequest request,
            GetCoursesResult response,
            [Frozen] Mock<IMediator> mediator)
        {
            //Arrange
            var controller = new CoursesController(mediator.Object);
            mediator.Setup(x => 
                    x.Send(It.Is<GetCoursesQuery>(c 
                        => c.Keyword.Equals(request.Keyword)
                        && c.RouteIds.Equals(request.Sectors)
                        && c.Levels.Equals(request.Levels)),It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //Act
            var actual = await controller.Courses(request);

            //Assert
            Assert.IsNotNull(actual);
            var actualResult = actual as ViewResult;
            Assert.IsNotNull(actualResult);
            var actualModel = actualResult.Model as CoursesViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.Courses.Should().NotBeEmpty();
            actualModel.Sectors.Should().NotBeEmpty();
            actualModel.Keyword.Should().Be(request.Keyword);
            actualModel.SelectedLevels.Should().BeEquivalentTo(request.Levels);
            actualModel.SelectedSectors.Should().BeEquivalentTo(request.Sectors);
            actualModel.Total.Should().Be(response.Total);
            actualModel.TotalFiltered.Should().Be(response.TotalFiltered);
        }

        [Test, MoqAutoData]
        public async Task Then_Any_Sectors_In_The_Request_Are_Marked_As_Selected_On_The_ViewModel(
            GetCoursesRequest request,
            GetCoursesResult response,
            [Frozen] Mock<IMediator> mediator)
        {
            //Arrange
            response.Sectors.Add(new Sector
            {
                Id=request.Sectors.First(), 
                Route = "Selected"
            });
            response.Sectors.Add(new Sector
            {
                Id=request.Sectors.Skip(1).First(), 
                Route = "Selected"
            });
            var controller = new CoursesController(mediator.Object);
            mediator.Setup(x => 
                    x.Send(It.Is<GetCoursesQuery>(c 
                        => c.Keyword.Equals(request.Keyword)
                           && c.RouteIds.Equals(request.Sectors)),It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //Act
            var actual = await controller.Courses(request);
            
            //Assert
            Assert.IsNotNull(actual);
            var actualResult = actual as ViewResult;
            Assert.IsNotNull(actualResult);
            var actualModel = actualResult.Model as CoursesViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.Sectors.Should().NotBeEmpty();
            Assert.AreEqual(2, actualModel.Sectors.Count(c=>c.Selected));
            Assert.IsNotNull(actualModel.Sectors.SingleOrDefault(c=>c.Id.Equals(request.Sectors.First())));
            Assert.IsNotNull(actualModel.Sectors.SingleOrDefault(c=>c.Id.Equals(request.Sectors.Skip(1).First())));
        }
    }
}