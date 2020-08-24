using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCourseProvidersApiRequest : IGetApiRequest
    {
        private readonly string _location;
        private readonly int _id;

        public GetCourseProvidersApiRequest (string baseUrl, int id, string location)
        {
            _location = location;
            BaseUrl = baseUrl;
            _id = id;
        }

        public string BaseUrl { get; }
        public string GetUrl => $"{BaseUrl}trainingcourses/{_id}/providers?location={_location}";
    }
}
