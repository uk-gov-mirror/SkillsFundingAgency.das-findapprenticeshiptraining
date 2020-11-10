using System.Web;
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
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{courseId}/providers/{providerId}?location={location}&lat=0&lon=0");

        }

        [Test, AutoData]
        public void Then_The_Get_Url_Is_Constructed_Correctly_With_No_Location(string baseUrl, int courseId,
            int providerId)
        {
            //Arrange Act
            var actual = new GetCourseProviderDetailsApiRequest(baseUrl, courseId, providerId, "");

            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{courseId}/providers/{providerId}?location=&lat=0&lon=0");
        }
        
        [Test, AutoData]
        public void Then_The_Location_Is_Url_Encoded(string baseUrl, int courseId,
            int providerId, string location)
        {
            //Arrange Act
            var actual = new GetCourseProviderDetailsApiRequest(baseUrl, courseId, providerId, $"{location} & {location}");
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{courseId}/providers/{providerId}?location={HttpUtility.UrlEncode($"{location} & {location}")}&lat=0&lon=0");
        }

        [Test, AutoData]
        public void Then_The_Lat_Lon_Is_Added(string baseUrl, int courseId, int providerId,
            string location, double lat, double lon)
        {
            //Arrange Act
            var actual = new GetCourseProviderDetailsApiRequest(baseUrl, courseId, providerId, $"{location} & {location}", lat, lon);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{courseId}/providers/{providerId}?location={HttpUtility.UrlEncode($"{location} & {location}")}&lat={lat}&lon={lon}");
        }
    }
}
