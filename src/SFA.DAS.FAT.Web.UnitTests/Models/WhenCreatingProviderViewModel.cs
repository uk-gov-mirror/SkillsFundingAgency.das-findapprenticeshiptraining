using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models
{
    public class WhenCreatingProviderViewModel
    {
        [Test, AutoData]
        public void Then_Maps_The_Fields(Provider source)
        {
            var actual = (ProviderViewModel) source;
            
            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c=>c.OverallAchievementRate)
                .Excluding(c=>c.NationalOverallAchievementRate)
                .Excluding(c=>c.NationalOverallCohort));

            actual.OverallAchievementRatePercentage.Should().Be($"{(Math.Round(source.OverallAchievementRate.Value)/100):0%}");
            actual.NationalOverallAchievementRatePercentage.Should().Be($"{(Math.Round(source.NationalOverallAchievementRate.Value)/100):0%}");
        }

        [Test, AutoData]
        public void Then_No_Achievement_Data_Shows_Empty_String(Provider source)
        {
            source.OverallAchievementRate = null;
            source.NationalOverallAchievementRate = null;
            
            var actual = (ProviderViewModel) source;

            actual.OverallAchievementRatePercentage.Should().BeEmpty();
            actual.NationalOverallAchievementRatePercentage.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_Rounds_Values_For_Achievement_Data(Provider source)
        {
            source.OverallAchievementRate = 38.9m;
            source.NationalOverallAchievementRate = 78.9m;
            
            var actual = (ProviderViewModel) source;
            
            actual.OverallAchievementRatePercentage.Should().Be("39%");
            actual.NationalOverallAchievementRatePercentage.Should().Be("79%");
        }
    }
}