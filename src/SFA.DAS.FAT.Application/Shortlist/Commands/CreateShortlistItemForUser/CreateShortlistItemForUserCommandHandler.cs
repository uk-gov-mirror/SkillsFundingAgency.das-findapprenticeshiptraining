using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Application.Shortlist.Commands.CreateShortlistItemForUser
{
    public class CreateShortlistItemForUserCommandHandler : IRequestHandler<CreateShortlistItemForUserCommand, Unit>
    {
        private readonly IShortlistService _service;

        public CreateShortlistItemForUserCommandHandler (IShortlistService service)
        {
            _service = service;
        }
        
        public async Task<Unit> Handle(CreateShortlistItemForUserCommand request, CancellationToken cancellationToken)
        {
            await _service.CreateShortlistItemForUser(
                request.Id,
                request.ShortlistUserId,
                request.Ukprn,
                request.TrainingCode,
                request.SectorSubjectArea,
                request.Lat,
                request.Lon,
                request.LocationDescription);
            
            return Unit.Value;
            
        }
    }
}