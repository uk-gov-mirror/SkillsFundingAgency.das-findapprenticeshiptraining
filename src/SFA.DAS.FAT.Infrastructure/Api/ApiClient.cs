using System.Net;
using System.Net.Http;
using System.Text;
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

        public async Task<TResponse> Post<TResponse,TPostData>(IPostApiRequest<TPostData> request)
        {
            AddHeaders();
            
            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await _httpClient.PostAsync(request.PostUrl, stringContent)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);    
        }

        public async Task Delete(IDeleteApiRequest request)
        {
            AddHeaders();
            var response = await _httpClient.DeleteAsync(request.DeleteUrl)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
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