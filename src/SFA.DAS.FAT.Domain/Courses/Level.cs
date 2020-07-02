using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.FAT.Domain.Courses
{
    public class Level
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
