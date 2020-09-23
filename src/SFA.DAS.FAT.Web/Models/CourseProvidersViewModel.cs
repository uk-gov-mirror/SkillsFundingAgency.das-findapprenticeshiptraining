using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class CourseProvidersViewModel
    {
        public IEnumerable<ProviderViewModel> Providers { get; set; }
        public CourseViewModel Course { get; set; }
        public int Total { get; set; }
        public string TotalMessage => Total == 1 ? $"{Total} result" : $"{Total} results";
        public string Location { get; set; }
        public ProviderSortBy SortOrder { get; set; }
        public bool HasLocations => !string.IsNullOrWhiteSpace(Location);
        public string BuildSortLink()
        {
            var newOrder = SortOrder == ProviderSortBy.Distance ? 
                ProviderSortBy.Name : ProviderSortBy.Distance;
            var selectedDeliveryModes = DeliveryModes
                .Where(viewModel => viewModel.Selected)
                .Select(viewModel => viewModel.DeliveryModeType);

            return $"?location={Location}&deliveryModes={string.Join("&deliveryModes=", selectedDeliveryModes)}&sortorder={newOrder}";
        }

        public IEnumerable<DeliveryModeOptionViewModel> DeliveryModes { get; set; }

        public Dictionary<string, string> BuildClearDeliveryModeLinks()
        {
            var links = new Dictionary<string, string>();

            foreach (var deliveryMode in DeliveryModes.Where(model => model.Selected))
            {
                var otherSelected = DeliveryModes
                    .Where(viewModel =>
                        viewModel.Selected &&
                        viewModel.DeliveryModeType != deliveryMode.DeliveryModeType)
                    .Select(viewModel => viewModel.DeliveryModeType);
                var link = $"?location={Location}&deliveryModes={string.Join("&deliveryModes=", otherSelected)}&sortorder={SortOrder}";

                links.Add(deliveryMode.Description, link);
            }

            return links;
        }
    }
}
