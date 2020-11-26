using System.Collections.Generic;
using System.Net;
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
            
            AddHeaders();

            var response = await _httpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                return default;
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<TResponse>(json);    
            }
            
            response.EnsureSuccessStatusCode();
            
            return default;
        }

        public async Task<int> Ping()
        {
            AddHeaders();

            var result = await _httpClient.GetAsync($"{_config.PingUrl}ping");
            
            return (int)result.StatusCode;
        }

        private void AddHeaders()
        {
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config.Key);
            _httpClient.DefaultRequestHeaders.Add("X-Version", "1");
        }
    }
}