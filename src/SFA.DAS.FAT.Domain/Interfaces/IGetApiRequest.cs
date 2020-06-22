using Newtonsoft.Json;

namespace SFA.DAS.FAT.Domain.Interfaces
{
    public interface IGetApiRequest : IBaseApiRequest
    {
        [JsonIgnore]
        string GetUrl { get; }
    }
}