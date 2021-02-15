using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Shortlist.Commands.DeleteShortlistItemForUser;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Controllers;
using SFA.DAS.FAT.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Web.UnitTests.Controllers.ShortlistControllerTests
{
    public class WhenDeletingShortlistItemForUser
    {
        [Test, MoqAutoData]
        public async Task And_Cookie_Exists_Then_Deletes_Shortlist_Item_For_User(
            Guid id,
            ShortlistCookieItem shortlistCookie,
            [Frozen] Mock<ICookieStorageService<ShortlistCookieItem>> mockShortlistCookieService,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            //Arrange
            mockShortlistCookieService
                .Setup(service => service.Get(Constants.ShortlistCookieName))
                .Returns(shortlistCookie);
            
            //Act
            var actual = await controller.DeleteShortlistItemForUser(id) as AcceptedResult;
            
            //Assert
            actual.Should().NotBeNull();
            mockMediator.Verify(x=>x.Send(It.Is<DeleteShortlistItemForUserCommand>(c=>
                c.ShortlistUserId.Equals(shortlistCookie.ShortlistUserId)
                && c.Id.Equals(id)
            ), It.IsAny<CancellationToken>()), Times.Once);
            
        }
        
        [Test, MoqAutoData]
        public async Task And_Cookie_Does_Not_Exists_Then_Deletes_Command_Is_Not_Called(
            Guid id,
            [Frozen] Mock<ICookieStorageService<ShortlistCookieItem>> mockShortlistCookieService,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            //Arrange
            mockShortlistCookieService
                .Setup(service => service.Get(Constants.ShortlistCookieName))
                .Returns((ShortlistCookieItem)null);
            
            //Act
            var actual = await controller.DeleteShortlistItemForUser(id) as AcceptedResult;
            
            //Assert
            actual.Should().NotBeNull();
            mockMediator.Verify(x=>x.Send(It.IsAny<DeleteShortlistItemForUserCommand>(),
                It.IsAny<CancellationToken>()), Times.Never);
            
        }
        
    }
}