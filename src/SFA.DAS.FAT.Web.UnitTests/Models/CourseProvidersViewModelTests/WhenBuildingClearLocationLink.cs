using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CourseProvidersViewModelTests
{
    public class WhenBuildingClearLocationLink
    {
        [Test, AutoData]
        public void Then_The_Location_Is_Set_To_Minus_One_And_No_Delivery_Modes(CourseProvidersViewModel model)
        {
            var actual = model.ClearLocationLink;

            actual.Should().StartWith("?location=-1");
        }

        [Test, AutoData]
        public void Then_Any_Selected_Employer_Reviews_Are_Maintained(CourseProvidersViewModel model)
        {
            model.DeliveryModes = model.DeliveryModes.Select(c =>
            {
                c.Selected = true;
                return c;
            }).ToList();
            model.ProviderRatings = model.ProviderRatings.Select(c =>
            {
                c.Selected = true;
                return c;
            }).ToList();
            
            var actual = model.ClearDeliveryModeLinks;

            foreach (var actualItem in actual)
            {
                actualItem.Value.Should().Contain($"&providerRatings={string.Join("&providerRatings=", model.ProviderRatings.Select(c => c.ProviderRatingType))}");
                actualItem.Value.Should().StartWith($"?location={model.Location}");
            }
            
        }
    }
}