using System.Collections.Generic;
using System.Linq;
using System.Web;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Domain.Courses.Api
{
    public class GetCourseProvidersApiRequest : IGetApiRequest
    {
        private readonly string _location;
        private readonly IEnumerable<DeliveryModeType> _deliveryModeTypes;
        private readonly IEnumerable<ProviderRating> _providerRatingTypes;

        private readonly int _sortOrder;
        private readonly double _lat;
        private readonly double _lon;
        private readonly int _id;

        public GetCourseProvidersApiRequest(string baseUrl, int id, string location, IEnumerable<DeliveryModeType> deliveryModeTypes, IEnumerable<ProviderRating> providerRatingTypes, int sortOrder = 0, double lat = 0, double lon = 0)
        {
            _location = location;
            _deliveryModeTypes = deliveryModeTypes;
            _sortOrder = sortOrder;
            _lat = lat;
            _lon = lon;
            BaseUrl = baseUrl;
            _id = id;
            _providerRatingTypes = providerRatingTypes;
        }

        public string BaseUrl { get; }
        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var buildUrl = $"{BaseUrl}trainingcourses/{_id}/providers?location={HttpUtility.UrlEncode(_location)}&sortOrder={_sortOrder}";
            if (_deliveryModeTypes!= null && _deliveryModeTypes.Any())
            {
                buildUrl += $"&deliveryModes={string.Join("&deliveryModes=", _deliveryModeTypes)}";
            }
            if (_providerRatingTypes != null && _providerRatingTypes.Any())
            {
                buildUrl += $"&providerRatings={string.Join("&providerRatings=", _providerRatingTypes)}";
            }

            if (_lat != 0)
            {
                buildUrl += $"&lat={_lat}";
            }
            if (_lon != 0)
            {
                buildUrl += $"&lon={_lon}";
            }
            
            return buildUrl;
        }
    }
}
