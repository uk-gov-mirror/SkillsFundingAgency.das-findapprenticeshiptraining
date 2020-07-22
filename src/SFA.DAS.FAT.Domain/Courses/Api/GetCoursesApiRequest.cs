using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCoursesApiRequest : IGetApiRequest
    {
        public GetCoursesApiRequest( string baseUrl, string keyword, List<Guid> sectors = null, List<int> levels = null, OrderBy orderBy = OrderBy.None)
        {
            BaseUrl = baseUrl;
            Keyword = keyword;
            Sectors = sectors;
            Levels = levels;
            OrderBy = orderBy;
        }

        public List<Guid> Sectors { get ; set ; }
        public List<int> Levels { get; set; }

        public string BaseUrl { get; }
        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var url = $"{BaseUrl}trainingcourses?keyword={Keyword}";
            if (OrderBy != OrderBy.None)
            {
                url += $"orderby={OrderBy}";
            }
            if (Sectors != null && Sectors.Any())
            {
                url += "&routeIds=" + string.Join("&routeIds=", Sectors);
            }
            if (Levels != null && Levels.Any())
            {
                url += "&levels=" + string.Join("&levels=", Levels);
            }
            return url;
        }

        public string Keyword { get; set; }
        public OrderBy OrderBy { get; set; }
    }

}

