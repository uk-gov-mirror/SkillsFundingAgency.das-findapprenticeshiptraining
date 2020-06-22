using Newtonsoft.Json;

namespace SFA.DAS.FAT.Domain.Interfaces
{
    public interface IGetAllApiRequest : IBaseApiRequest
    {
        [JsonIgnore]
        string GetAllUrl { get; }
    }
}