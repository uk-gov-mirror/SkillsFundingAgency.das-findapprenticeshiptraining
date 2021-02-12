using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Shortlist.Queries.GetShortlistForUser;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Controllers;
using SFA.DAS.FAT.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Web.UnitTests.Controllers.ShortlistControllerTests
{
    public class WhenGettingShortlistForUser
    {
        [Test, MoqAutoData]
        public async Task And_Cookie_Exists_Then_Reads_Cookie_And_Builds_ViewModel_From_Mediator_Result(
            ShortlistCookieItem shortlistCookie,
            GetShortlistForUserResult resultFromMediator,
            [Frozen] Mock<ICookieStorageService<ShortlistCookieItem>> mockCookieService,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            //Arrange
            mockCookieService
                .Setup(service => service.Get(Constants.ShortlistCookieName))
                .Returns(shortlistCookie);
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetShortlistForUserQuery>(c => c.ShortlistUserId.Equals(shortlistCookie.ShortlistUserId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultFromMediator);
            
            //Act
            var actual = await controller.Index() as ViewResult;
            
            //Assert
            actual.Should().NotBeNull();
            var model = actual.Model as ShortlistViewModel;
            model.Should().NotBeNull();
            model.Shortlist.Should()
                .BeEquivalentTo(
                    resultFromMediator.Shortlist.Select(item => (ShortlistItemViewModel)item));
        }

        //todo: and no cookie??
    }
}
