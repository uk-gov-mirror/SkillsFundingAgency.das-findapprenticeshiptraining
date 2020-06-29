using System;
using System.Collections.Generic;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCoursesApiRequest : IGetApiRequest
    {
        public GetCoursesApiRequest( string baseUrl, string keyword, List<Guid> sectors = null)
        {
            BaseUrl = baseUrl;
            Keyword = keyword;
            Sectors = sectors;
        }

        public List<Guid> Sectors { get ; set ; }

        public string BaseUrl { get; }
        public string GetUrl => Sectors == null 
            ? $"{BaseUrl}trainingcourses?keyword={Keyword}" 
            : $"{BaseUrl}trainingcourses?keyword={Keyword}&routeIds={string.Join("&routeIds=", Sectors)}";
        public string Keyword { get; set; }
    }
}