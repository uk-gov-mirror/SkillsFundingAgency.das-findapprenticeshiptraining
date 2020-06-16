using Newtonsoft.Json;

namespace SFA.DAS.FAT.Domain.Interfaces
{
    public interface IBaseApiRequest
    {
        [JsonIgnore]
        string BaseUrl { get; }
    }
}