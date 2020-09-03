using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses.Api;

namespace SFA.DAS.FAT.Domain.UnitTests.Courses.Api
{
    public class WhenCreatingAGetCourseProvidersApiRequest
    {
        [Test, AutoData]
        public void Then_The_Get_Url_Is_Constructed_Correctly(string baseUrl, int id, string location, int sortOrder)
        {
            //Arrange Act
            var actual = new GetCourseProvidersApiRequest(baseUrl, id, location, sortOrder);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{id}/providers?location={location}&sortOrder={sortOrder}");
        }

        [Test, AutoData]
        public void Then_The_Location_Is_Url_Encoded(string baseUrl, int id, string location, int sortOrder)
        {
            //Arrange Act
            var actual = new GetCourseProvidersApiRequest(baseUrl, id, $"{location} & {location}", sortOrder);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{id}/providers?location={HttpUtility.UrlEncode($"{location} & {location}")}&sortOrder={sortOrder}");
        }
    }
}
