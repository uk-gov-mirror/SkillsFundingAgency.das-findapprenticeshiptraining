using System.Threading.Tasks;

namespace SFA.DAS.FAT.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<TResponse> GetAll<TResponse>(IGetAllApiRequest request);
    }
}