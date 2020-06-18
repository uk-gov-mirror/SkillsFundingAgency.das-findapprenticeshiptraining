using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCoursesApiRequest : IGetApiRequest
    {
        public GetCoursesApiRequest(string baseUrl)
        {
            BaseUrl = baseUrl;
        }
        public string BaseUrl { get; }
        public string GetUrl => $"{BaseUrl}findapprenticeshiptraining/trainingcourses";
    }
}