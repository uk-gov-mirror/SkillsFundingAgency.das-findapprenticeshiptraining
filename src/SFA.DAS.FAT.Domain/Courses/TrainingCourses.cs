using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.FAT.Domain.Courses
{
    public class TrainingCourses
    {
        [JsonProperty("trainingCourses")]
        public List<Course> Courses { get; set; }
    }
}