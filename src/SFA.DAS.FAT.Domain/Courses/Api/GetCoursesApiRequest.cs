using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCoursesApiRequest : IGetApiRequest
    {
        public GetCoursesApiRequest(string baseUrl, string keyword)
        {
            BaseUrl = baseUrl;
            Keyword = keyword;
        }
        public string BaseUrl { get; }
        public string GetUrl => $"{BaseUrl}/trainingcourses";
        public string Keyword { get; set; }
    }
}