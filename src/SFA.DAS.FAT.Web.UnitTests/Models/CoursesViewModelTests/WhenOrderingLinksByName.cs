using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        
        [Test, AutoData]
        public void And_Then_Adds_Sectors_To_Query_String(List<Guid> selectedRoutes)
        {
            // Arrange 
            var buildSectorLink = "";
            
            // Act
            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, null, new List<int>(), "Name");
            foreach (var selectedRoute in selectedRoutes)
            {
                buildSectorLink += model.SelectedSectors != null && model.SelectedSectors.Any() ? $"&Sectors=" + string.Join("&Sectors=", selectedRoute) : "";
            }

            // Assert​
            model.OrderByName.Should().Be($"?OrderBy=Name{buildSectorLink}");
        }
        [Test, AutoData]
        public void And_Then_Adds_Levels_To_Query_String(List<int> selectedLevels)
        {
            // Arrange 
            var buildLevelsLink = "";

            // Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), null, selectedLevels, "Name");
            
            // Assert​
            foreach (var selectedLevel in selectedLevels)
            {
                buildLevelsLink += model.SelectedLevels != null && model.SelectedLevels.Any() ? $"&Levels=" + string.Join("Levels=", selectedLevel) : "";
            }

            model.OrderByName.Should().Be($"?OrderBy=Name{buildLevelsLink}");
        }
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
