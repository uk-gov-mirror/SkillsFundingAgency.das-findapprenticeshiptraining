using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.FAT.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<int> Ping();
        Task Post<TPostData>(IPostApiRequest<TPostData> request);
        Task Delete(IDeleteApiRequest request);
    }
}