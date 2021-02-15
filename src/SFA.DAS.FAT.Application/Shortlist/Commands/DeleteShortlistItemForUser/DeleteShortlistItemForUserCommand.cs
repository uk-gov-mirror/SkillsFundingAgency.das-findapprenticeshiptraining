using System;
using MediatR;

namespace SFA.DAS.FAT.Application.Shortlist.Commands.DeleteShortlistItemForUser
{
    public class DeleteShortlistItemForUserCommand : IRequest<Unit>
    {
        public Guid Id { get ; set ; }
        public Guid ShortlistUserId { get ; set ; }
    }
}