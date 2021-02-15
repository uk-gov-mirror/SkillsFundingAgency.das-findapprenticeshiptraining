using System;
using MediatR;

namespace SFA.DAS.FAT.Application.Shortlist.Commands.CreateShortlistItemForUser
{
    public class CreateShortlistItemForUserCommand : IRequest<Unit>
    {
        public Guid Id { get ; set ; }
        public Guid ShortlistUserId { get ; set ; }
        public int Ukprn { get ; set ; }
        public int TrainingCode { get ; set ; }
        public string SectorSubjectArea { get ; set ; }
        public double? Lat { get ; set ; }
        public double? Lon { get ; set ; }
        public string LocationDescription { get ; set ; }
    }
}