using System;
using System.Threading.Tasks;
using SFA.DAS.FAT.Domain.Shortlist;

namespace SFA.DAS.FAT.Domain.Interfaces
{
    public interface IShortlistService
    {
        Task<ShortlistForUser> GetShortlistForUser(Guid shortlistUserId);
    }
}
