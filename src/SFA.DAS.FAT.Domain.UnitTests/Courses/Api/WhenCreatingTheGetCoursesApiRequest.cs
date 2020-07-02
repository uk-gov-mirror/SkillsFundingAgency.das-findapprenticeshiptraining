using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses.Api;

namespace SFA.DAS.FAT.Domain.UnitTests.Courses.Api
{
    public class WhenCreatingTheGetCoursesApiRequest
    {
        [Test, AutoData]
        public void Then_The_Get_Url_Is_Constructed_Correctly(string baseUrl, string keyword, List<Guid> sectors)
        {
            //Arrange Act
            var actual = new GetCoursesApiRequest(baseUrl, keyword, sectors);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses?keyword={keyword}&routeIds={string.Join("&routeIds=", sectors)}");
        }

        [Test, AutoData]
        public void Then_If_The_List_Of_Sectors_Is_Null_The_Url_Is_Constructed_Correctly(string baseUrl, string keyword)
        {
            //Arrange Act
            var actual = new GetCoursesApiRequest(baseUrl, keyword);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses?keyword={keyword}");
        }
        
        [Test, AutoData]
        public void Then_If_The_List_Of_Sectors_Is_Empty_The_Url_Is_Constructed_Correctly(string baseUrl, string keyword)
        {
            //Arrange Act
            var actual = new GetCoursesApiRequest(baseUrl, keyword, new List<Guid>());
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses?keyword={keyword}");
        }
    }
}