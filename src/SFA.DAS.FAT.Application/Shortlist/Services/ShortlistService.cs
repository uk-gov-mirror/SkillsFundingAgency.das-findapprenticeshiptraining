using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Domain.Shortlist;
using SFA.DAS.FAT.Domain.Shortlist.Api;

namespace SFA.DAS.FAT.Application.Shortlist.Services
{
    public class ShortlistService : IShortlistService
    {
        private readonly IApiClient _apiClient;
        private readonly FindApprenticeshipTrainingApi _configValue;

        public ShortlistService(IApiClient apiClient, IOptions<FindApprenticeshipTrainingApi> config)
        {
            _apiClient = apiClient;
            _configValue = config.Value;
        }

        public async Task<ShortlistForUser> GetShortlistForUser(Guid shortlistUserId)
        {
            var apiRequest = new GetShortlistForUserApiRequest(_configValue.BaseUrl, shortlistUserId);

            var apiResponse = await _apiClient.Get<ShortlistForUser>(apiRequest);

            return apiResponse;
        }

        public async Task DeleteShortlistItemForUser(Guid id, Guid shortlistUserId)
        {
            await _apiClient.Delete(new DeleteShortlistForUserRequest(_configValue.BaseUrl, id, shortlistUserId));
        }

        public async Task CreateShortlistItemForUser(Guid shortlistUserId, int ukprn, int trainingCode, string sectorSubjectArea, double? lat, double? lon, string locationDescription)
        {
            var request = new PostShortlistForUserRequest
            {
                Lat = lat,
                Lon = lon,
                LocationDescription = locationDescription,
                Ukprn = ukprn,
                StandardId = trainingCode,
                SectorSubjectArea = sectorSubjectArea,
                ShortlistUserId = shortlistUserId
            };
            
            await _apiClient.Post(new CreateShortlistForUserRequest(_configValue.BaseUrl) {Data = request});
        }
    }
}
