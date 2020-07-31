using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SFA.DAS.FAT.Domain.Courses
{
    public class TrainingCourseProviderDetails
    {
        [JsonProperty("courseproviderdetails")]
        public Provider CourseProviderDetails { get; set; }
    }
}
