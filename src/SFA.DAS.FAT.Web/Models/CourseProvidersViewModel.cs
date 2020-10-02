using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            ProviderRatings = BuildProviderRatingOptionViewModel(request.ProviderRatings);
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
            var selectedProviderRatings = ProviderRatings
                .Where(viewModel => viewModel.Selected)
                .Select(viewModel => viewModel.ProviderRatingType);
             
            return $"?location={Location}" + "&deliveryModes=" + $"{string.Join("&deliveryModes=", selectedDeliveryModes)}" + "&providerRatings=" + $"{string.Join("&providerRatings=", selectedProviderRatings)}" + $"&sortorder={newOrder}";
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
                var link = $"?location={HttpUtility.UrlEncode($"{Location}")}&deliveryModes={HttpUtility.UrlEncode($"{string.Join("&deliveryModes=", otherSelected)}")}&sortorder={HttpUtility.UrlEncode($"{SortOrder}")}";

                links.Add(deliveryMode.Description, link);
            }

            return links;
        }

        public Dictionary<string, string> BuildClearProviderRatingLinks()
        {
            var links = new Dictionary<string, string>();

            foreach (var providerRating in ProviderRatings.Where(model => model.Selected))
            {
                var otherSelected = ProviderRatings
                    .Where(viewModel =>
                        viewModel.Selected &&
                        viewModel.ProviderRatingType != providerRating.ProviderRatingType)
                    .Select(viewModel => viewModel.ProviderRatingType);
                var link = $"?location={HttpUtility.UrlEncode($"{Location}")}&providerRatings={HttpUtility.UrlEncode($"{string.Join("&providerRatings=", otherSelected)}")}&sortorder={HttpUtility.UrlEncode($"{SortOrder}")}";

                links.Add(providerRating.Description, link);
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

        private static IEnumerable<ProviderRatingOptionViewModel> BuildProviderRatingOptionViewModel(IReadOnlyList<ProviderRating> selectedProviderRatingTypes)
        {
            var providerRatingOptionViewModel = new List<ProviderRatingOptionViewModel>();

            foreach (ProviderRating providerRatingType in Enum.GetValues(typeof(ProviderRating)))
            {
                providerRatingOptionViewModel.Add(new ProviderRatingOptionViewModel
                {
                    ProviderRatingType = providerRatingType,
                    Description = providerRatingType.GetDescription(),
                    Selected = selectedProviderRatingTypes.Any(type => type == providerRatingType)
                });
            }

            return providerRatingOptionViewModel;
        }
    }
}
