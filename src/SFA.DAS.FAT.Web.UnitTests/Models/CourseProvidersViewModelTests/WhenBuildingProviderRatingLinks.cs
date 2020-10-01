using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CourseProvidersViewModelTests
{
    public class WhenBuildingProviderRatingLinks
    {
        [Test, AutoData]
        public void And_ProviderRating_Selected_Then_Link_Returned(CourseProvidersViewModel model)
        {
            // Arrange
            foreach (var providerRating in model.ProviderRatings )
            {
                providerRating.Selected = true;
            }

            // Act
            var links = model.BuildClearProviderRatingLinks();

            // Assert
            foreach (var providerRating in model.ProviderRatings.Where(vm => vm.Selected))
            {
                var link = links.Single(pair => pair.Key == providerRating.Description);
                var otherSelected = model.ProviderRatings
                    .Where(vm =>
                        vm.Selected &&
                        vm.ProviderRatingType != providerRating.ProviderRatingType)
                    .Select(vm => vm.ProviderRatingType);

                link.Value.Should().Be($"?location={model.Location}&providerRatings={string.Join("&providerRatings=", otherSelected)}&sortorder={model.SortOrder}");
            }
        }

        [Test, AutoData]
        public void And_ProviderType_Not_Selected_Then_Empty_List_Returned(CourseProvidersViewModel model)
        {
            foreach (var providerRating in model.ProviderRatings)
            {
                providerRating.Selected = false;
            }

            var links = model.BuildClearProviderRatingLinks();

            links.Should().BeEmpty();
        }
    }
}
