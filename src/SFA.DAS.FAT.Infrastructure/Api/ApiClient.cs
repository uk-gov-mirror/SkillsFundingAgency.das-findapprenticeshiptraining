using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Infrastructure.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly FindApprenticeshipTrainingApi _config;

        public ApiClient (HttpClient httpClient, IOptions<FindApprenticeshipTrainingApi> config)
        {
            _httpClient = httpClient;
            _config = config.Value;
        }
        
        public async Task<TResponse> Get<TResponse>(IGetApiRequest request) 
        {
            
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key",_config.Key);

            var response = await _httpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);
        
        }

        public async Task<TResponse> GetAll<TResponse>(IGetAllApiRequest request)
        {
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config.ApiKey);

            var response = await _httpClient.GetAsync(request.GetAllUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);
        }
    }
}