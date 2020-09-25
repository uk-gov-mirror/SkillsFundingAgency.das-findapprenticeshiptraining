using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCourseApiRequest : IGetApiRequest
    {
        private readonly double _lat;
        private readonly double _lon;

        public GetCourseApiRequest (string baseUrl, int id, double lat, double lon)
        {
            _lat = lat;
            _lon = lon;
            BaseUrl = baseUrl;
            Id = id;
        }

        private int Id { get; }
        public string BaseUrl { get; }
        public string GetUrl => $"{BaseUrl}trainingcourses/{Id}?lat={_lat}&lon={_lon}";
    }
}