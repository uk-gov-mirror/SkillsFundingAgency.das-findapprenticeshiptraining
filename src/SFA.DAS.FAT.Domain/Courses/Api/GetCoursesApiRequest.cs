using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCoursesApiRequest : IGetAllApiRequest
    {
        public GetCoursesApiRequest(string baseUrl)
        {
            BaseUrl = baseUrl;
        }
        public string BaseUrl { get; }
        public string GetAllUrl => $"{BaseUrl}findapprenticeshiptraining/trainingcourses";
    }
}