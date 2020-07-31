using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCourseProviderDetailsApiRequest :  IGetApiRequest
    {
        public GetCourseProviderDetailsApiRequest(string baseUrl, int id)
        {
            BaseUrl = baseUrl;
            Id = id;
        }
        private int Id { get; }
        public string BaseUrl { get; }
        public string GetUrl => $"{BaseUrl}trainingcourses/{Id}";
    }
}
