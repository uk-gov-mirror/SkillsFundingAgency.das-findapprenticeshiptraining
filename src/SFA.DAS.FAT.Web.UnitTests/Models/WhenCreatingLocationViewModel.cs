﻿using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models
{
    public class WhenCreatingLocationViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(Domain.Locations.Locations.LocationItem source)
        {
            var actual = (LocationViewModel) source;

            source.Should().BeEquivalentTo(actual);
        }
    }
}
