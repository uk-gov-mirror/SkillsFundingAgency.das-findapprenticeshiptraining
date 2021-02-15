using System;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Shortlist.Api
{
    public class DeleteShortlistForUserRequest : IDeleteApiRequest
    {
        private readonly Guid _id;
        private readonly Guid _shortlistUserid;

        public DeleteShortlistForUserRequest(string baseUrl, Guid id, Guid shortlistUserid)
        {
            _id = id;
            _shortlistUserid = shortlistUserid;
            BaseUrl = baseUrl;
        }

        public string BaseUrl { get; }
        public string DeleteUrl => $"{BaseUrl}shortlist/users/{_shortlistUserid}/items/{_id}";
    }
}