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
        public void Then_The_Get_Url_Is_Constructed_Correctly_With_Levels_And_Routes(string baseUrl, string keyword, List<Guid> sectors, List<int> levels)
        {
            //Arrange Act
            var actual = new GetCoursesApiRequest(baseUrl, keyword, sectors, levels);
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses?keyword={keyword}&routeIds={string.Join("&routeIds=", sectors)}&levels={string.Join("&levels=", levels)}");
        }

        [Test, AutoData]
        public void Then_The_Get_Url_Is_Constructed_Correctly_With_Routes_And_Levels_Is_Null(string baseUrl, string keyword, List<Guid> sectors)
        {
            //Arrange Act
            var actual = new GetCoursesApiRequest(baseUrl, keyword, sectors);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses?keyword={keyword}&routeIds={string.Join("&routeIds=", sectors)}");
        }

        [Test, AutoData]
        public void Then_If_The_List_Of_Sectors_And_Levels_Are_Null_The_Url_Is_Constructed_Correctly(string baseUrl, string keyword)
        {
            //Arrange Act
            var actual = new GetCoursesApiRequest(baseUrl, keyword);
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses?keyword={keyword}");
        }
        
        
        [Test, AutoData]
        public void Then_If_The_List_Of_Sectors_And_Levels_Are_Empty_The_Url_Is_Constructed_Correctly(string baseUrl, string keyword)
        {
            //Arrange Act
            var actual = new GetCoursesApiRequest(baseUrl, keyword, new List<Guid>(), new List<int>());
            
            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses?keyword={keyword}");
        }

        [Test, AutoData]
        public void Then_The_Get_Url_Is_Constructed_Correctly_With_Levels_And_Routes_Null(string baseUrl, string keyword, List<int> levels)
        {
            //Arrange Act
            var actual = new GetCoursesApiRequest(baseUrl, keyword, null, levels);

            //Assert
            actual.GetUrl.Should().Be($"{baseUrl}trainingcourses?keyword={keyword}&levels={string.Join("&levels=", levels)}");
        }
    }
}