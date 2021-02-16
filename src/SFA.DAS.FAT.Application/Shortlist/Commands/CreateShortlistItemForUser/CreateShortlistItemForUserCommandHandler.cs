using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Application.Shortlist.Commands.CreateShortlistItemForUser
{
    public class CreateShortlistItemForUserCommandHandler : IRequestHandler<CreateShortlistItemForUserCommand, Guid>
    {
        private readonly IShortlistService _service;

        public CreateShortlistItemForUserCommandHandler (IShortlistService service)
        {
            _service = service;
        }
        
        public async Task<Guid> Handle(CreateShortlistItemForUserCommand request, CancellationToken cancellationToken)
        {
            var itemId = await _service.CreateShortlistItemForUser(
                request.ShortlistUserId,
                request.Ukprn,
                request.TrainingCode,
                request.SectorSubjectArea,
                request.Lat,
                request.Lon,
                request.LocationDescription);
            
            return itemId;
            
        }
    }
}