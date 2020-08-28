using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Extensions;

namespace SFA.DAS.FAT.Domain.UnitTests.Extensions
{
    public class WhenGettingDescription
    {
        [Test]
        public void Then_Returns_Description_From_Attr()
        {
            EnumForTesting.ForTesting.GetDescription().Should().Be("Something for testing");
        }

        [Test]
        public void And_No_Attr_Then_Returns_Empty()
        {
            EnumForTesting.OopsNoDescription.GetDescription().Should().BeEmpty();
        }

        public enum EnumForTesting
        {
            [System.ComponentModel.Description("Something for testing")]
            ForTesting,
            OopsNoDescription
        }
    }
}
