using System;
using MediatR;

namespace SFA.DAS.FAT.Application.Shortlist.Queries.GetShortlistForUser
{
    public class GetShortlistForUserQuery : IRequest<GetShortlistForUserResult>
    {
        public Guid ShortlistUserId { get; set; }
    }
}
