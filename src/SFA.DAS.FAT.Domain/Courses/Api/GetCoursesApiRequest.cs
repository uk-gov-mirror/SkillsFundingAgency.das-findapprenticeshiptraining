using System;
using System.Collections.Generic;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCoursesApiRequest : IGetApiRequest
    {
        public GetCoursesApiRequest( string baseUrl, string keyword, List<Guid> sectors = null, List<int> levels = null)
        {
            BaseUrl = baseUrl;
            Keyword = keyword;
            Sectors = sectors;
            Levels = levels;
        }

        public List<Guid> Sectors { get ; set ; }
        public List<int> Levels { get; set; }

        public string BaseUrl { get; }
        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var url = $"{BaseUrl}trainingcourses?keyword={Keyword}";
            if (Sectors != null)
            {
                url += "&routeIds=" + string.Join("&routeIds=", Sectors);
            }
            if (Levels != null)
            {
                url += "&levels=" + string.Join("&levels=", Levels);
            }
            return url;
        }

        public string Keyword { get; set; }
    }

}

