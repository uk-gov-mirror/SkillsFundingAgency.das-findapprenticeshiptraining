using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Shortlist.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Domain.Shortlist.Api;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Application.UnitTests.Shortlist.Services
{
    public class WhenCreatingShortlistItemForUser
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Sent_To_The_Api(
            Guid id,
            Guid shortlistUserId,
            int ukprn, 
            int trainingCode,
            string sectorSubjectArea,
            double? lat,
            double? lon,
            string locationDescription,
            PostShortlistForUserRequest postShortlistForUserRequest,
            [Frozen] Mock<IApiClient> apiClient,
            ShortlistService service)
        {
            //Act
            await service.CreateShortlistItemForUser(id, shortlistUserId, ukprn, trainingCode, sectorSubjectArea, lat, lon, locationDescription);
            
            //Assert
            apiClient.Verify(x =>
                x.Post(
                    It.Is<CreateShortlistForUserRequest>(c => 
                        c.PostUrl.Contains($"shortlist", StringComparison.InvariantCultureIgnoreCase) 
                        && c.Data.Lat.Equals(lat)
                        && c.Data.Lon.Equals(lon)
                        && c.Data.LocationDescription.Equals(locationDescription)
                        && c.Data.Ukprn.Equals(ukprn)
                        && c.Data.StandardId.Equals(trainingCode)
                        && c.Data.SectorSubjectArea.Equals(sectorSubjectArea)
                        && c.Data.ShortlistUserId.Equals(shortlistUserId)
                    )), Times.Once);
        }
    }
}