using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Domain.Extensions;

namespace SFA.DAS.FAT.Web.Models
{
    public class CourseProvidersViewModel
    {
        public CourseProvidersViewModel(GetCourseProvidersRequest request, GetCourseProvidersResult result)
        {
            Course = result.Course;
            Providers = result.Providers.Select(c => (ProviderViewModel)c);
            Total = result.Total;
            TotalFiltered = result.TotalFiltered;
            Location = result.Location;
            SortOrder = request.SortOrder;
            DeliveryModes = BuildDeliveryModeOptionViewModel(request.DeliveryModes);
        }

        public IEnumerable<ProviderViewModel> Providers { get; set; }
        public CourseViewModel Course { get; set; }
        public int Total { get; set; }
        public int TotalFiltered { get; set; }
        public string TotalMessage => GetTotalMessage();
        public string Location { get; set; }
        public ProviderSortBy SortOrder { get; set; }
        public bool HasLocation => !string.IsNullOrWhiteSpace(Location);
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
        public IEnumerable<ProviderRatingOptionViewModel> ProviderRatings { get; set; }

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

        private string GetTotalMessage()
        {
            var totalToUse = DeliveryModes == null || DeliveryModes.All(model => !model.Selected)
                ? Total 
                : TotalFiltered;

            return $"{totalToUse} result{(totalToUse!=1 ? "s": "")}";
        }

        private static IEnumerable<DeliveryModeOptionViewModel> BuildDeliveryModeOptionViewModel(IReadOnlyList<DeliveryModeType> selectedDeliveryModeTypes)
        {
            var deliveryModeOptionViewModels = new List<DeliveryModeOptionViewModel>();

            foreach (DeliveryModeType deliveryModeType in Enum.GetValues(typeof(DeliveryModeType)))
            {
                deliveryModeOptionViewModels.Add(new DeliveryModeOptionViewModel
                {
                    DeliveryModeType = deliveryModeType,
                    Description = deliveryModeType.GetDescription(),
                    Selected = selectedDeliveryModeTypes.Any(type => type == deliveryModeType)
                });
            }

            return deliveryModeOptionViewModels;
        }
    }
}
