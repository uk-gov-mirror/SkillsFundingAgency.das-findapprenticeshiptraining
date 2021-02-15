using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Application.Shortlist.Commands.DeleteShortlistItemForUser
{
    public class DeleteShortlistItemForUserCommandHandler : IRequestHandler<DeleteShortlistItemForUserCommand, Unit>
    {
        private readonly IShortlistService _service;

        public DeleteShortlistItemForUserCommandHandler (IShortlistService service)
        {
            _service = service;
        }
        public async Task<Unit> Handle(DeleteShortlistItemForUserCommand request, CancellationToken cancellationToken)
        {
            await _service.DeleteShortlistItemForUser(request.Id, request.ShortlistUserId);
            
            return Unit.Value;
        }
    }
}