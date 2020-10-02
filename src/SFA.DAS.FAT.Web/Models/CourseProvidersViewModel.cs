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
            var clearDeliveryModeLinks = new Dictionary<string, string>();

            if (DeliveryModes == null)
            {
                return clearDeliveryModeLinks;
            }

            var providerRatings = BuildProviderRatingLinks("appendTo");
            var location = BuildLocationLink();
            var sortOrder = BuildSortOrder("appendTo");

            foreach (var deliveryMode in DeliveryModes.Where(model => model.Selected))
            {
                var otherSelected = DeliveryModes
                    .Where(viewModel =>
                        viewModel.Selected &&
                        viewModel.DeliveryModeType != deliveryMode.DeliveryModeType)
                    .Select(viewModel => viewModel.DeliveryModeType);

                var link = $"{location}&deliveryModes={$"{string.Join("&deliveryModes=", otherSelected)}"}{providerRatings}{sortOrder}";

                clearDeliveryModeLinks.Add(deliveryMode.Description, link);
            }

            return clearDeliveryModeLinks;
        }
        public Dictionary<string, string> BuildClearProviderRatingLinks()
        {
            var providerRatingLinks = new Dictionary<string, string>();

            if (ProviderRatings == null)
            {
                return providerRatingLinks;
            }

            var deliveryModes = BuildDeliveryModeLinks("appendTo");
            var location = BuildLocationLink();
            var sortOrder = BuildSortOrder("appendTo");

            foreach (var providerRating in ProviderRatings.Where(model => model.Selected))
            {
                var otherSelected = ProviderRatings
                    .Where(viewModel =>
                        viewModel.Selected &&
                        viewModel.ProviderRatingType != providerRating.ProviderRatingType)
                    .Select(viewModel => viewModel.ProviderRatingType);
                var link = $"{location}{deliveryModes}&providerRatings={$"{string.Join("&providerRatings=", otherSelected)}"}{sortOrder}";

                providerRatingLinks.Add(providerRating.Description, link);
            }
            return providerRatingLinks;
        }

        private string BuildDeliveryModeLinks(string linkToAppendTo)
        {
            if (DeliveryModes != null && DeliveryModes.Any())
            {
                var deliveryModes = DeliveryModes.Where(dm => dm.Selected).Select(dm => dm.DeliveryModeType);
                return $"{GetSeparator(linkToAppendTo)}deliveryModes={$"{string.Join("&deliveryModes=", deliveryModes)}"}";
            }
            return null;
        }

        private string BuildSortOrder(string linkToAppendTo)
        {
            return $"{GetSeparator(linkToAppendTo)}sortorder={ HttpUtility.UrlEncode($"{SortOrder}")}";
        }

        private string BuildLocationLink()
        {
            return $"{GetSeparator("")}location={ HttpUtility.UrlEncode($"{Location}")}";
        }

        private string BuildProviderRatingLinks(string linkToAppendTo)
        {
            if (ProviderRatings != null && ProviderRatings.Any())
            {
                var providerRatings = ProviderRatings.Where(pr => pr.Selected).Select(pr => pr.ProviderRatingType);
                return $"{GetSeparator(linkToAppendTo)}providerRatings={$"{string.Join("&providerRatings=", providerRatings)}"}";
            }
            return null;
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
        private object GetSeparator(string url)
        {
            return string.IsNullOrEmpty(url) ? "?" : "&";
        }
    }
}
