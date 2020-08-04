using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SFA.DAS.FAT.Domain.Courses
{
    public class TrainingCourseProviderDetails
    {
        [JsonProperty("trainingCourseProvider")]
        public Provider CourseProviderDetails { get; set; }
        
        [JsonProperty("trainingCourse")]
        public Course TrainingCourse { get; set; }
    }
}
