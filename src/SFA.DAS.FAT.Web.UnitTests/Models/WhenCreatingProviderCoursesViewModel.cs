using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetProvider;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models
{
    public class WhenCreatingProviderCoursesViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(AdditionalCourses source)
        {
            var actual = (ProviderCoursesViewModel)source;

            actual.Courses.Should().BeAssignableTo<List<ProviderCourseViewModel>>();
            actual.Should().BeEquivalentTo(source);
        }
    }
}
