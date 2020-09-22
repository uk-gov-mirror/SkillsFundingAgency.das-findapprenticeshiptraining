using System.Collections.Generic;
using System.Web;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCourseProvidersApiRequest : IGetApiRequest
    {
        private readonly string _location;
        private readonly IEnumerable<DeliveryModeType> _deliveryModeTypes;
        private readonly int _sortOrder;
        private readonly int _id;

        public GetCourseProvidersApiRequest (string baseUrl, int id, string location, IEnumerable<DeliveryModeType> deliveryModeTypes, int sortOrder = 0)
        {
            _location = location;
            _deliveryModeTypes = deliveryModeTypes;
            _sortOrder = sortOrder;
            BaseUrl = baseUrl;
            _id = id;
        }

        public string BaseUrl { get; }
        public string GetUrl => $"{BaseUrl}trainingcourses/{_id}/providers?location={HttpUtility.UrlEncode(_location)}&deliveryModes={string.Join("&deliveryModes=", _deliveryModeTypes)}&sortOrder={_sortOrder}";
    }
}
