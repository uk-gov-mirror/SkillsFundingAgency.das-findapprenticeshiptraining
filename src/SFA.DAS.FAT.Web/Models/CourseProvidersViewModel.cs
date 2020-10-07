﻿using System;
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
        public string ClearLocationLink => BuildClearLocationFilterLink();
        public Dictionary<string, string> ClearDeliveryModeLinks => BuildClearDeliveryModeLinks();
        public Dictionary<string, string> ClearProviderRatingLinks => BuildClearProviderRatingLinks();
        public ProviderSortBy SortOrder { get; set; }
        public bool HasLocation => !string.IsNullOrWhiteSpace(Location);
        public bool HasProviderRatings => ProviderRatings != null && ProviderRatings.Any(model => model.Selected);
        public bool HasDeliveryModes => DeliveryModes !=null && DeliveryModes.Any(model => model.Selected);
        public bool ShowFilters => ShouldShowFilters();

        public IEnumerable<DeliveryModeOptionViewModel> DeliveryModes { get; set; }
        public IEnumerable<ProviderRatingOptionViewModel> ProviderRatings { get; set; }

        private bool ShouldShowFilters()
        {
            var result = !string.IsNullOrWhiteSpace(Location) ||
                         HasDeliveryModes ||
                         HasProviderRatings;
            return result;
        }

        public string BuildSortLink()
        {
            var newOrder = SortOrder == ProviderSortBy.Distance ? 
                ProviderSortBy.Name : ProviderSortBy.Distance;
             
            return $"?location={Location}" + BuildDeliveryModeLinks("location") + BuildProviderRatingLinks("location") + $"&sortorder={newOrder}";
        }

        private Dictionary<string, string> BuildClearDeliveryModeLinks()
        {
            var clearDeliveryModeLinks = new Dictionary<string, string>();

            if (DeliveryModes == null)
            {
                return clearDeliveryModeLinks;
            }

            var location = BuildLocationLink();
            var providerRatings = BuildProviderRatingLinks(location);
            var sortOrder = BuildSortOrder(location);

            foreach (var deliveryMode in DeliveryModes.Where(model => model.Selected))
            {
                var otherSelected = DeliveryModes
                    .Where(viewModel =>
                        viewModel.Selected &&
                        viewModel.DeliveryModeType != deliveryMode.DeliveryModeType)
                    .Select(viewModel => viewModel.DeliveryModeType);

                var link = $"{location}&deliveryModes={string.Join("&deliveryModes=", otherSelected)}{providerRatings}{sortOrder}";

                clearDeliveryModeLinks.Add(deliveryMode.Description, link);
            }

            return clearDeliveryModeLinks;
        }
        private Dictionary<string, string> BuildClearProviderRatingLinks()
        {
            var providerRatingLinks = new Dictionary<string, string>();

            if (ProviderRatings == null)
            {
                return providerRatingLinks;
            }

            var location = BuildLocationLink();
            var deliveryModes = BuildDeliveryModeLinks(location);
            var sortOrder = BuildSortOrder(location);

            foreach (var providerRating in ProviderRatings.Where(model => model.Selected))
            {
                var otherSelected = ProviderRatings
                    .Where(viewModel =>
                        viewModel.Selected &&
                        viewModel.ProviderRatingType != providerRating.ProviderRatingType)
                    .Select(viewModel => viewModel.ProviderRatingType);
                var link = $"{location}{deliveryModes}&providerRatings={string.Join("&providerRatings=", otherSelected)}{sortOrder}";

                providerRatingLinks.Add(providerRating.Description, link);
            }
            return providerRatingLinks;
        }

        private string BuildClearLocationFilterLink()
        {
            var location = "?location=-1";
            var providerRatings = BuildProviderRatingLinks(location);

            var link = $"{location}{providerRatings}";

            return link;
        }

        private string BuildDeliveryModeLinks(string linkToAppendTo)
        {
            if (HasDeliveryModes)
            {
                var deliveryModes = DeliveryModes.Where(dm => dm.Selected).Select(dm => dm.DeliveryModeType);
                return $"{GetSeparator(linkToAppendTo)}deliveryModes={string.Join("&deliveryModes=", deliveryModes)}";
            }
            return null;
        }

        private string BuildSortOrder(string linkToAppendTo)
        {
            return $"{GetSeparator(linkToAppendTo)}sortorder={HttpUtility.UrlEncode($"{SortOrder}")}";
        }

        private string BuildLocationLink()
        {
            return $"?location={HttpUtility.UrlEncode($"{Location}")}";
        }

        private string BuildProviderRatingLinks(string linkToAppendTo)
        {
            if (HasProviderRatings)
            {
                var providerRatings = ProviderRatings.Where(pr => pr.Selected).Select(pr => pr.ProviderRatingType);
                return $"{GetSeparator(linkToAppendTo)}providerRatings={string.Join("&providerRatings=", providerRatings)}";
            }
            return null;
        }


        private string GetTotalMessage()
        {
            var totalToUse = !HasDeliveryModes && !HasProviderRatings
                ? Total 
                : TotalFiltered;

            return $"{totalToUse} result{(totalToUse!=1 ? "s": "")}";
        }

        private static IEnumerable<DeliveryModeOptionViewModel> BuildDeliveryModeOptionViewModel(IReadOnlyList<DeliveryModeType> selectedDeliveryModeTypes)
        {
            var deliveryModeOptionViewModels = new List<DeliveryModeOptionViewModel>();

            foreach (DeliveryModeType deliveryModeType in Enum.GetValues(typeof(DeliveryModeType)))
            {
                if (deliveryModeType == DeliveryModeType.NotFound)
                {
                    continue;
                }
                
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
        private static string GetSeparator(string url)
        {
            return string.IsNullOrEmpty(url) ? "?" : "&";
        }
    }
}
