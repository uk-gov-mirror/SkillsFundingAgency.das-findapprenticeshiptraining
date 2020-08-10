using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetProvider;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models
{
    public class WhenCreatingCourseProviderViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetCourseProviderResult source)
        {
            var actual = (CourseProviderViewModel) source;
            
            actual.Course.Should().BeAssignableTo<CourseViewModel>();
            actual.AdditionalCourses.Should().BeAssignableTo<ProviderCoursesViewModel>();
            actual.Should().BeEquivalentTo(source.Provider, options => options.Excluding(c=>c));
        }
    }
}
