using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CourseProvidersViewModelTests
{
    public class WhenGettingTotalMessage
    {
        [Test, AutoData]
        public void And_Total_Greater_Than_1_Then_Returns_Correct_Text(CourseProvidersViewModel model)
        {
            model.TotalMessage.Should().Be($"{model.Total} results");
        }

        [Test, AutoData]
        public void And_Total_Equals_1_Then_Returns_Correct_Text(CourseProvidersViewModel model)
        {
            model.Total = 1;

            model.TotalMessage.Should().Be($"{model.Total} result");
        }
    }
}
