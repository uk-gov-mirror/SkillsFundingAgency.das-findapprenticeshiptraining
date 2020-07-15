using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModelTests
{
    class WhenOrderingLinksByName
    {
        [Test]
        public void Then_Adds_OrderBy_Name_To_Query_String()
        {
            // Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), null, new List<int>(), "Name");
            // Assert
            model.OrderByName.Should().Be("?OrderBy=Name");
        }

        [Test, AutoData]
        public void And_Then_Adds_Keyword_To_Query_String(string keyword)
        {
            // Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), keyword, new List<int>(), "Name");
            // Assert
            model.OrderByName.Should​().Be($"?OrderBy=Name&Keyword={model.Keyword}");
        }
//​
//        [Test]
//        public void And_Then_Adds_Sectors_To_Query_String()
//        {
//            // Arrange Act
//            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), null, new List<int>(), "Name");
//            // Assert​
//        }
//​
//        [Test]
//        public void And_Then_Adds_Levels_To_Query_String()
//        {
//            // Arrange Act
//            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), null, new List<int>(), "Name");
//            // Assert​
//        }
//​
//        [Test]
//        public void And_Then_Adds_Sectors_And_Levels_To_Query_String()
//        {
//            // Arrange Act
//            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), null, new List<int>(), "Name");
//            // Assert
//        }
    }
}
