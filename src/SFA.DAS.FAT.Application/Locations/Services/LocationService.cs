using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Domain.Locations.Api;

namespace SFA.DAS.FAT.Application.Locations.Services
{
    public class LocationService : ILocationService
    {
        private readonly IApiClient _client;
        private readonly FindApprenticeshipTrainingApi _config;

        public LocationService (IApiClient client, IOptions<FindApprenticeshipTrainingApi> config)
        {
            _client = client;
            _config = config.Value;
        }
        public async Task<Domain.Locations.Locations> GetLocations(string searchTerm)
        {
            var request = new GetLocationsApiRequest(_config.BaseUrl, searchTerm);

            var result = await _client.Get<Domain.Locations.Locations>(request);

            return result;
        }
    }
}