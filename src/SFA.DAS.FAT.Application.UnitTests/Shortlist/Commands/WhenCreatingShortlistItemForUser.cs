using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Shortlist.Commands.CreateShortlistItemForUser;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Application.UnitTests.Shortlist.Commands
{
    public class WhenCreatingShortlistItemForUser
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Service_Called(
            CreateShortlistItemForUserCommand command,
            [Frozen] Mock<IShortlistService> service,
            CreateShortlistItemForUserCommandHandler handler)
        {
            //Act
            await handler.Handle(command, CancellationToken.None);
            
            //Assert
            service.Verify(x=>x.CreateShortlistItemForUser(command.Id, 
                command.ShortlistUserId, 
                command.Ukprn, 
                command.TrainingCode, 
                command.SectorSubjectArea, 
                command.Lat, 
                command.Lon, 
                command.LocationDescription));
        }
    }
}