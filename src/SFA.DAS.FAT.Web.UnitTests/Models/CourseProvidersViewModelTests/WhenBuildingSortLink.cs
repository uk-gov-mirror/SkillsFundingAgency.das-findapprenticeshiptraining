using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CourseProvidersViewModelTests
{
    public class WhenBuildingSortLink
    {
        [Test, AutoData]
        public void And_SortOrder_Distance_Then_Link_To_Name(CourseProvidersViewModel model)
        {
            model.SortOrder = ProviderSortBy.Distance;
            var selectedDeliveryModes = model.DeliveryModes
                .Where(viewModel => viewModel.Selected)
                .Select(viewModel => viewModel.DeliveryModeType);
            var selectedProviderRatings = model.ProviderRatings
                .Where(vm => vm.Selected)
                .Select(vm => vm.ProviderRatingType);

            var link = model.BuildSortLink();
            link.Should().Be($"?location={model.Location}&deliveryModes={string.Join("&deliveryModes=", selectedDeliveryModes)}&sortorder={ProviderSortBy.Name}&providerRatings={string.Join("&provideRatings=", selectedProviderRatings)}");
        }

        [Test, AutoData]
        public void And_SortOrder_Name_Then_Link_To_Distance(CourseProvidersViewModel model)
        {
            model.SortOrder = ProviderSortBy.Name;
            var selectedDeliveryModes = model.DeliveryModes
                .Where(viewModel => viewModel.Selected)
                .Select(viewModel => viewModel.DeliveryModeType);
            var selectedProviderRatings = model.ProviderRatings
                .Where(vm => vm.Selected)
                .Select(vm => vm.ProviderRatingType);

            var link = model.BuildSortLink();

            link.Should().Be($"?location={model.Location}&deliveryModes={string.Join("&deliveryModes=", selectedDeliveryModes)}&sortorder={ProviderSortBy.Distance}&providerRatings={string.Join("&provideRatings=", selectedProviderRatings)}");
        }
    }
}
