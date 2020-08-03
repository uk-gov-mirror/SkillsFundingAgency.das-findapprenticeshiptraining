using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.FAT.Application.Courses.Queries.GetProvider;
using Moq;
using AutoFixture.NUnit3;
using MediatR;
using System.Threading;
using SFA.DAS.FAT.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Controllers.CoursesControllerTests
{
    public class WhenGettingCourseProviderDetails
    {

        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Sent_And_Provider_Detail_Retrieved_And_Shown(
            int providerId, 
            int courseId,
            GetCourseProviderResult response, 
            [Frozen] Mock<IMediator> mediator,
            CoursesController controller
            )
        {
            //Arrange
            mediator.Setup(x => x.Send(It.Is<GetCourseProviderQuery>(c =>
                c.ProviderId.Equals(providerId) && c.CourseId.Equals(courseId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //Act
            var actual = await controller.CourseProviderDetail(courseId,providerId);

            //Assert
            Assert.IsNotNull(actual);
            var actualResult = actual as ViewResult;
            Assert.IsNotNull(actualResult);
            var actualModel = actualResult.Model as CourseProviderViewModel;
            Assert.IsNotNull(actualModel);
        }
    }
}
