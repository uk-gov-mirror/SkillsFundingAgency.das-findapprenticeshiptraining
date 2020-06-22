using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
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
            GetCoursesResult response,
            [Frozen] Mock<IMediator> mediator,
            CoursesController controller)
        {
            //Arrange
            mediator.Setup(x => 
                    x.Send(It.IsAny<GetCoursesQuery>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.Courses();
            
            //Assert
            Assert.IsNotNull(actual);
            var actualResult = actual as ViewResult;
            Assert.IsNotNull(actualResult);
            var actualModel = actualResult.Model as CoursesViewModel;
            Assert.IsNotNull(actualModel);
        }
    }
}