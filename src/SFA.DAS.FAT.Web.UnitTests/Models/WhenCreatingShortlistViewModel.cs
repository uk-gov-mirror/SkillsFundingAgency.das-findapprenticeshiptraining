using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Shortlist;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models
{
    public class WhenCreatingShortlistViewModel
    {
        [Test, AutoData]
        public void Then_The_Values_Are_Mapped_Correctly(ShortlistItem source)
        {
            var actual = (ShortlistItemViewModel)source;
            
            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(item => item.ShortlistUserId)
                .Excluding(item => item.Course)
                .Excluding(item => item.Provider));
            actual.Course.Should().BeEquivalentTo((CourseViewModel)source.Course);
            actual.Provider.Should().BeEquivalentTo((ProviderViewModel)source.Provider);
        }
    }
}
