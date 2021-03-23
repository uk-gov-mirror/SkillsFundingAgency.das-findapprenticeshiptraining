using Newtonsoft.Json;

namespace SFA.DAS.FAT.Domain.Courses
{
    public class Sector
    {
        [JsonProperty("route")]
        public string Route { get; set; }
    }
}