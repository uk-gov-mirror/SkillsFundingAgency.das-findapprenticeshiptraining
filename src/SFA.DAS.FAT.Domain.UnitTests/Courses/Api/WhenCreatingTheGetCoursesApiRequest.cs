using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses.Api;

namespace SFA.DAS.FAT.Domain.UnitTests.Courses.Api
{
    public class WhenCreatingTheGetCoursesApiRequest
    {
        [Test, AutoData]
        public void Then_The_Get_Url_Is_Constructed_Correctly(string baseUrl, string keyword)
        {
            //Arrange Act
            var actual = new GetCoursesApiRequest(baseUrl, keyword);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses?keyword={keyword}");
        }
    }
}