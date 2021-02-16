using System;
using System.Collections.Generic;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Domain.Courses.Api;

namespace SFA.DAS.FAT.Domain.UnitTests.Courses.Api
{
    public class WhenCreatingAGetCourseProvidersApiRequest
    {
        [Test, AutoData]
        public void Then_The_Get_Url_Is_Constructed_Correctly(string baseUrl, int id, string location, List<DeliveryModeType> deliveryModeTypes, List<ProviderRating> providerRatingTypes, int sortOrder, Guid shortlistUserId)
        {
            //Arrange Act
            var actual = new GetCourseProvidersApiRequest(baseUrl, id, location, deliveryModeTypes, providerRatingTypes, sortOrder, shortlistUserId: shortlistUserId);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{id}/providers?location={location}&sortOrder={sortOrder}&deliveryModes={string.Join("&deliveryModes=", deliveryModeTypes)}&providerRatings={string.Join("&providerRatings=", providerRatingTypes)}&shortlistUserId={shortlistUserId}");
        }

        [Test, AutoData]
        public void Then_If_No_DelvieryModes_Its_Not_Added_To_Url_And_ProviderRating_Added(string baseUrl, int id, string location, List<ProviderRating> providerRatingTypes, int sortOrder)
        {
            //Arrange Act
            var actual = new GetCourseProvidersApiRequest(baseUrl, id, location, new List<DeliveryModeType>(), providerRatingTypes, sortOrder);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{id}/providers?location={location}&sortOrder={sortOrder}&providerRatings={string.Join("&providerRatings=", providerRatingTypes)}");
        }

        [Test, AutoData]
        public void Then_If_No_DelvieryModes_Is_Null_Its_Not_Added_To_Url_And_ProviderRating_Added(string baseUrl, int id, string location, List<ProviderRating> providerRatingTypes, int sortOrder)
        {
            //Arrange Act
            var actual = new GetCourseProvidersApiRequest(baseUrl, id, location, null, providerRatingTypes, sortOrder);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{id}/providers?location={location}&sortOrder={sortOrder}&providerRatings={string.Join("&providerRatings=", providerRatingTypes)}");
        }

        [Test, AutoData]
        public void Then_If_No_ProviderRating_Its_Not_Added_To_Url_And_DeliveryMode_Added(string baseUrl, int id, string location, List<DeliveryModeType> deliveryModeTypes, int sortOrder)
        {
            //Arrange Act
            var actual = new GetCourseProvidersApiRequest(baseUrl, id, location, deliveryModeTypes, new List<ProviderRating>(), sortOrder);

            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{id}/providers?location={location}&sortOrder={sortOrder}&deliveryModes={string.Join("&deliveryModes=", deliveryModeTypes)}");
        }

        [Test, AutoData]
        public void Then_If_No_ProviderRating_Is_Null_Its_Not_Added_To_Url_And_DeliveryMode_Added(string baseUrl, int id, string location, List<DeliveryModeType> deliveryModeTypes, int sortOrder)
        {
            //Arrange Act
            var actual = new GetCourseProvidersApiRequest(baseUrl, id, location, deliveryModeTypes, null, sortOrder);

            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{id}/providers?location={location}&sortOrder={sortOrder}&deliveryModes={string.Join("&deliveryModes=", deliveryModeTypes)}");
        }

        [Test, AutoData]
        public void Then_The_Location_Is_Url_Encoded(string baseUrl, int id, string location, List<DeliveryModeType> deliveryModeTypes, List<ProviderRating> providerRatingTypes, int sortOrder)
        {
            //Arrange Act
            var actual = new GetCourseProvidersApiRequest(baseUrl, id, $"{location} & {location}", deliveryModeTypes, providerRatingTypes, sortOrder);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{id}/providers?location={HttpUtility.UrlEncode($"{location} & {location}")}&sortOrder={sortOrder}&deliveryModes={string.Join("&deliveryModes=", deliveryModeTypes)}&providerRatings={string.Join("&providerRatings=", providerRatingTypes)}");
        }

        [Test, AutoData]
        public void Then_The_Lat_Lon_Is_Added_If_Not_Zero(string baseUrl, int id, string location,
            List<DeliveryModeType> deliveryModeTypes, List<ProviderRating> providerRatingTypes, int sortOrder,
            double lat, double lon)
        {
            //Arrange Act
            var actual = new GetCourseProvidersApiRequest(baseUrl, id, $"{location} & {location}", deliveryModeTypes, providerRatingTypes, sortOrder, lat, lon);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses/{id}/providers?location={HttpUtility.UrlEncode($"{location} & {location}")}&sortOrder={sortOrder}&deliveryModes={string.Join("&deliveryModes=", deliveryModeTypes)}&providerRatings={string.Join("&providerRatings=", providerRatingTypes)}&lat={lat}&lon={lon}");
        }
    }
}
