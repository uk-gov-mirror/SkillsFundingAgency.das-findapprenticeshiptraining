using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCourseApiRequest : IGetApiRequest
    {
        public GetCourseApiRequest (string baseUrl, int id)
        {
            BaseUrl = baseUrl;
            Id = id;
        }

        private int Id { get; }
        public string BaseUrl { get; }
        public string GetUrl => $"{BaseUrl}trainingcourses/{Id}";
    }
}