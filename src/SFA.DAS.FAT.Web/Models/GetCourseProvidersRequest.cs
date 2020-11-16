using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAT.Web.Models
{
    public class GetCourseProvidersRequest
    {
        public int Id { get; set; }
        [FromQuery]
        public string Location { get; set; }
        [FromQuery]
        public IReadOnlyList<DeliveryModeType> DeliveryModes { get; set; } = new List<DeliveryModeType>();
        [FromQuery]
        public IReadOnlyList<ProviderRating> ProviderRatings { get; set; } = new List<ProviderRating>();
        

        public Dictionary<string, string> ToDictionary()
        {
            var result = new Dictionary<string, string>
            {
                {nameof(Id), Id.ToString()},
                {nameof(Location), Location}
            };

            for (var i = 0; i < DeliveryModes.Count; i++)
            {
                var deliveryModeType = DeliveryModes[i];
                result.Add($"{nameof(DeliveryModes)}[{i}]", deliveryModeType.ToString());
            }

            for (var i = 0; i < ProviderRatings.Count; i++)
            {
                var providerRating = ProviderRatings[i];
                result.Add($"{nameof(ProviderRatings)}[{i}]", providerRating.ToString());
            }

            return result;
        }
    }
}
