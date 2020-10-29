using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCourseProviderDetailsApiRequest :  IGetApiRequest
    {
        private readonly int _providerId;
        private readonly string _location;

        private readonly int _courseId;

        public GetCourseProviderDetailsApiRequest(string baseUrl, int courseId, int providerId, string location)
        {
            _providerId = providerId;
            _location = location;
            BaseUrl = baseUrl;
            _courseId = courseId;
        }

        public string BaseUrl { get; }
        public string GetUrl => $"{BaseUrl}trainingcourses/{_courseId}/providers/{_providerId}?location={HttpUtility.UrlEncode(_location)}";
    }
}
