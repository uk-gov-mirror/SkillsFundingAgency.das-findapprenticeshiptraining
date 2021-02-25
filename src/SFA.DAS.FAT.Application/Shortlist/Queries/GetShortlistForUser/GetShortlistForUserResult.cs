using System.Collections.Generic;
using SFA.DAS.FAT.Domain.Shortlist;

namespace SFA.DAS.FAT.Application.Shortlist.Queries.GetShortlistForUser
{
    public class GetShortlistForUserResult
    {
        public IEnumerable<ShortlistItem> Shortlist { get; set; }
    }
}
