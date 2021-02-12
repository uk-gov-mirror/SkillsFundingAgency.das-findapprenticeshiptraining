using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Shortlist.Queries.GetShortlistForUser;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Application.UnitTests.Shortlist.Queries
{
    public class WhenGettingShortlistForUser
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Results_From_Service(
            GetShortlistForUserQuery query,
            Domain.Shortlist.ShortlistForUser shortlistFromService,
            [Frozen] Mock<IShortlistService> mockService,
            GetShortlistForUserQueryHandler handler)
        {
            mockService
                .Setup(service => service.GetShortlistForUser(query.ShortlistUserId))
                .ReturnsAsync(shortlistFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Shortlist.Should().BeEquivalentTo(shortlistFromService.Shortlist);
            
        }
    }
}
