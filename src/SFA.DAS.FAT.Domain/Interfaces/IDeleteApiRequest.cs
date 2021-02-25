using Newtonsoft.Json;

namespace SFA.DAS.FAT.Domain.Interfaces
{
    public interface IDeleteApiRequest : IBaseApiRequest
    {
        [JsonIgnore]
        string DeleteUrl { get; }
    }
}