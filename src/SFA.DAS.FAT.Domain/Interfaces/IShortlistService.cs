using System;
using System.Threading.Tasks;
using SFA.DAS.FAT.Domain.Shortlist;

namespace SFA.DAS.FAT.Domain.Interfaces
{
    public interface IShortlistService
    {
        Task<ShortlistForUser> GetShortlistForUser(Guid shortlistUserId);
        Task DeleteShortlistItemForUser(Guid id, Guid shortlistUserId);
        Task CreateShortlistItemForUser(Guid id, Guid shortlistUserId, int ukprn, int trainingCode, string sectorSubjectArea, double? lat, double? lon, string locationDescription);
    }
}
