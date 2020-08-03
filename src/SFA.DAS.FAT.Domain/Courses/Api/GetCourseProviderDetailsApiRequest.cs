using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCourseProviderDetailsApiRequest :  IGetApiRequest
    {
        private readonly int _providerId;

        private readonly int _courseId;

        public GetCourseProviderDetailsApiRequest(string baseUrl, int courseId, int providerId)
        {
            _providerId = providerId;
            BaseUrl = baseUrl;
            _courseId = courseId;
        }

        public string BaseUrl { get; }
        public string GetUrl => $"{BaseUrl}trainingcourses/{_courseId}/providers/{_providerId}";
    }
}
