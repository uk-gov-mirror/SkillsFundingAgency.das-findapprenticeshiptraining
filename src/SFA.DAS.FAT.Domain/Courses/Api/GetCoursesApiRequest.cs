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
            string url = "";
            if (Sectors == null && Levels == null)
            {
                url = $"{BaseUrl}trainingcourses?keyword={Keyword}";
                return url;
            }
            else if (Sectors != null && Levels != null)
            {
                url = $"{BaseUrl}trainingcourses?keyword={Keyword}&routeIds={string.Join("&routeIds=", Sectors)}&levels={string.Join("&levels=", Levels)}";
                return url;
            }
            else if (Sectors != null)
            {
                url = $"{BaseUrl}trainingcourses?keyword={Keyword}&routeIds={string.Join("&routeIds=", Sectors)}";
                return url;
            }
            else if (Levels != null)
            {
                url = $"{BaseUrl}trainingcourses?keyword={Keyword}&levels={string.Join("&levels=", Levels)}";
                return url;
            }
            else 
            {
                return null;
            }
        }

        public string Keyword { get; set; }
    }

}

