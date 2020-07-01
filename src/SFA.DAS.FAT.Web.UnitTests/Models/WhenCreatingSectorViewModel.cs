using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models
{
    public class WhenCreatingSectorViewModel
    {
        [Test, AutoData]
        public void Then_The_Values_Are_Mapped_Correctly(Sector sector)
        {
            var actual = (SectorViewModel) sector;
            
            actual.Should().BeEquivalentTo(sector);
        }
    }
}