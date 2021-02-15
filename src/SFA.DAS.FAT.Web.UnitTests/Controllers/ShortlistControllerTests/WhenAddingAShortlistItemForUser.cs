using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Shortlist.Commands.CreateShortlistItemForUser;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Controllers;
using SFA.DAS.FAT.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Web.UnitTests.Controllers.ShortlistControllerTests
{
    public class WhenAddingAShortlistItemForUser
    {
        [Test, MoqAutoData]
        public async Task And_Cookie_Exists_Then_Adds_To_Shortlist_For_User(
            CreateShortListItemRequest request,
            ShortlistCookieItem shortlistCookie,
            [Frozen] Mock<ICookieStorageService<ShortlistCookieItem>> mockShortlistCookieService,
            [Frozen] Mock<ICookieStorageService<LocationCookieItem>> mockLocationCookieService,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            //Arrange
            mockShortlistCookieService
                .Setup(service => service.Get(Constants.ShortlistCookieName))
                .Returns(shortlistCookie);
            mockLocationCookieService.Setup(x => x.Get(Constants.LocationCookieName))
                .Returns((LocationCookieItem) null);
            
            
            //Act
            var actual = await controller.CreateShortlistItem(request) as AcceptedResult;
            
            //Assert
            actual.Should().NotBeNull();
            mockMediator.Verify(x=>x.Send(It.Is<CreateShortlistItemForUserCommand>(c=>
                c.ShortlistUserId.Equals(shortlistCookie.ShortlistUserId)
                && c.Lat == null
                && c.Lon == null
                && c.LocationDescription == null
                && c.Ukprn.Equals(request.Ukprn)
                && c.TrainingCode.Equals(request.TrainingCode)
                && c.SectorSubjectArea.Equals(request.SectorSubjectArea)
                ), It.IsAny<CancellationToken>()), Times.Once);
            
        }

        [Test, MoqAutoData]
        public async Task And_The_Cookie_Does_Not_Exist_Then_A_New_Cookie_Is_Created_And_Used_For_Shortlist(
            CreateShortListItemRequest request,
            [Frozen] Mock<ICookieStorageService<ShortlistCookieItem>> mockShortlistCookieService,
            [Frozen] Mock<ICookieStorageService<LocationCookieItem>> mockLocationCookieService,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            //Arrange
            mockShortlistCookieService
                .Setup(service => service.Get(Constants.ShortlistCookieName))
                .Returns((ShortlistCookieItem) null);
            mockLocationCookieService.Setup(x => x.Get(Constants.LocationCookieName))
                .Returns((LocationCookieItem) null);
            
            //Act
            var actual = await controller.CreateShortlistItem(request) as AcceptedResult;
            
            //Assert
            actual.Should().NotBeNull();
            mockMediator.Verify(x=>x.Send(It.Is<CreateShortlistItemForUserCommand>(c=>
                  c.Lat == null
                  && c.Lon == null 
                  && c.LocationDescription == null
                  && c.Ukprn.Equals(request.Ukprn)
                  && c.TrainingCode.Equals(request.TrainingCode)
                  && c.SectorSubjectArea.Equals(request.SectorSubjectArea)
            ), It.IsAny<CancellationToken>()), Times.Once);
            mockShortlistCookieService.Verify(x=>
                x.Create(
                    It.Is<ShortlistCookieItem>(c=>c.ShortlistUserId!=Guid.Empty),
                    Constants.ShortlistCookieName, 
                    30), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_The_Location_Cookie_Exists_Then_Location_Is_Added_To_Shortlist(
            CreateShortListItemRequest request,
            ShortlistCookieItem shortlistCookie,
            LocationCookieItem locationCookieItem,
            [Frozen] Mock<ICookieStorageService<ShortlistCookieItem>> mockShortlistCookieService,
            [Frozen] Mock<ICookieStorageService<LocationCookieItem>> mockLocationCookieService,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            //Arrange
            mockShortlistCookieService
                .Setup(service => service.Get(Constants.ShortlistCookieName))
                .Returns(shortlistCookie);
            mockLocationCookieService.Setup(x => x.Get(Constants.LocationCookieName))
                .Returns(locationCookieItem);
            
            
            //Act
            var actual = await controller.CreateShortlistItem(request) as AcceptedResult;
            
            //Assert
            actual.Should().NotBeNull();
            mockMediator.Verify(x=>x.Send(It.Is<CreateShortlistItemForUserCommand>(c=>
                c.ShortlistUserId.Equals(shortlistCookie.ShortlistUserId)
                && c.Lat.Equals(locationCookieItem.Lat)
                && c.Lon.Equals(locationCookieItem.Lon)
                && c.LocationDescription.Equals(locationCookieItem.Name)
                && c.Ukprn.Equals(request.Ukprn)
                && c.TrainingCode.Equals(request.TrainingCode)
                && c.SectorSubjectArea.Equals(request.SectorSubjectArea)
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}