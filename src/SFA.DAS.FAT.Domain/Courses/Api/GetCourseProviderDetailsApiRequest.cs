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
        private readonly double _lat;
        private readonly double _lon;

        private readonly int _courseId;

        public GetCourseProviderDetailsApiRequest(string baseUrl, int courseId, int providerId, string location, double lat = 0, double lon = 0)
        {
            _providerId = providerId;
            _location = location;
            _lat = lat;
            _lon = lon;
            BaseUrl = baseUrl;
            _courseId = courseId;
        }

        public string BaseUrl { get; }
        public string GetUrl => $"{BaseUrl}trainingcourses/{_courseId}/providers/{_providerId}?location={HttpUtility.UrlEncode(_location)}&lat={_lat}&lon={_lon}";
    }
}
