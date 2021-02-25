using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Application.Shortlist.Queries.GetShortlistForUser
{
    public class GetShortlistForUserQueryHandler : IRequestHandler<GetShortlistForUserQuery, GetShortlistForUserResult> 
    {
        private readonly IShortlistService _shortlistService;

        public GetShortlistForUserQueryHandler(IShortlistService shortlistService)
        {
            _shortlistService = shortlistService;
        }

        public async Task<GetShortlistForUserResult> Handle(GetShortlistForUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _shortlistService.GetShortlistForUser(request.ShortlistUserId);

            return new GetShortlistForUserResult {Shortlist = result.Shortlist};
        }
    }
}
