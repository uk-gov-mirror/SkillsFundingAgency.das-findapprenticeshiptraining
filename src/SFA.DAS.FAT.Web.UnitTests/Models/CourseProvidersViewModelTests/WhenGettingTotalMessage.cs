using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CourseProvidersViewModelTests
{
    public class WhenGettingTotalMessage
    {
        [TestCase(10, 5, false, "10 results")]
        [TestCase(10, 5, true, "5 results")]
        [TestCase(1, 5, false, "1 result")]
        [TestCase(5, 1, true, "1 result")]
        [TestCase(0, 5, false, "0 results")]
        [TestCase(5, 0, true, "0 results")]
        public void Then_The_Total_Message_Is_Created_Correctly(int totalCount, int filterTotal, bool hasFilter, string expectedMessage)
        {
            var viewModel = new CourseProvidersViewModel
            {
                Total = totalCount,
                TotalFiltered = filterTotal,
                DeliveryModes = new List<DeliveryModeOptionViewModel>
                {
                    new DeliveryModeOptionViewModel
                    {
                        Selected = hasFilter, DeliveryModeType = DeliveryModeType.BlockRelease
                    }
                }
            };

            viewModel.TotalMessage.Should().Be(expectedMessage);
        }
    }
}
