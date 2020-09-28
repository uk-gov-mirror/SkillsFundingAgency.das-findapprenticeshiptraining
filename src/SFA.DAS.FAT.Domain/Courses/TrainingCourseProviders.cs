using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.FAT.Domain.Courses
{
    public class TrainingCourseProviders
    {
        [JsonProperty("trainingCourse")]
        public Course Course { get; set; }
        [JsonProperty("trainingCourseProviders")]
        public IEnumerable<Provider> CourseProviders { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("totalFiltered")] 
        public int TotalFiltered { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
    }
}
