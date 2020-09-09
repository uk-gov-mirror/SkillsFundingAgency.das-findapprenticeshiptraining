using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses.Api;

namespace SFA.DAS.FAT.Domain.UnitTests.Courses.Api
{
    public class WhenCreatingTheGetCourseProviderDetailsApiRequest
    {
        [Test, AutoData]
        public void Then_The_Get_Url_Is_Constructed_Correctly(string baseUrl, int courseId, int providerId, string location)
        {
            //Arrange Act
            var actual = new GetCourseProviderDetailsApiRequest(baseUrl, courseId, providerId, location);

            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{courseId}/providers/{providerId}?location={location}");

        }

        [Test, AutoData]
        public void Then_The_Get_Url_Is_Constructed_Correctly_With_No_Location(string baseUrl, int courseId,
            int providerId)
        {
            //Arrange Act
            var actual = new GetCourseProviderDetailsApiRequest(baseUrl, courseId, providerId, "");

            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{courseId}/providers/{providerId}?location=");
        }
    }
}
