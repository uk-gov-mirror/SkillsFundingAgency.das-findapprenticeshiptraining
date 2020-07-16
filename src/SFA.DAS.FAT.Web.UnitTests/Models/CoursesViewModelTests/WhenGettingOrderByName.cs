using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModelTests
{
    class WhenGettingOrderByName
    {
        [Test]
        public void Then_Adds_OrderBy_Name_To_Query_String()
        {
            // Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), null, new List<int>(), "name");
            // Assert
            model.OrderByName.Should().Be("?orderby=name");
        }

        [Test, AutoData]
        public void And_Then_Adds_Keyword_To_Query_String(string keyword)
        {
            // Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), keyword, new List<int>(), "name");
            // Assert
            model.OrderByName.Should​().Be($"?keyword={model.Keyword}&orderby=name");
        }
        
        [Test, AutoData]
        public void And_Then_Adds_Sectors_To_Query_String(List<Guid> selectedRoutes)
        {
            // Arrange 
            var buildSectorLink = "";
            
            // Act
            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, null, new List<int>(), "name");
            foreach (var selectedRoute in selectedRoutes)
            {
                var separator = string.IsNullOrEmpty(buildSectorLink) ? "?" : "&";
                buildSectorLink += model.SelectedSectors != null && model.SelectedSectors.Any() ? $"{separator}sectors=" + string.Join("&sectors=", selectedRoute) : "";
            }

            // Assert​
            model.OrderByName.Should().Be($"{buildSectorLink}&orderby=name");
        }
        [Test, AutoData]
        public void And_Then_Adds_Levels_To_Query_String(List<int> selectedLevels)
        {
            // Arrange 
            var buildLevelsLink = "";

            // Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), null, selectedLevels, "name");
            
            foreach (var selectedLevel in selectedLevels)
            {
                var separator = string.IsNullOrEmpty(buildLevelsLink) ? "?" : "&";
                buildLevelsLink += model.SelectedLevels != null && model.SelectedLevels.Any() ? $"{separator}levels=" + string.Join("levels=", selectedLevel) : "";
            }

            // Assert​
            model.OrderByName.Should().Be($"{buildLevelsLink}&orderby=name");
        }
        [Test, AutoData]
        public void And_Then_Adds_Sectors_And_Levels_To_Query_String(List<int> selectedLevels, List<Guid> selectedRoutes)
        {
            // Arrange 
            var buildLevelsLink = "";
            var buildSectorLink = "";
           
            // Act
            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, null, selectedLevels, "name");

            foreach (var selectedRoute in selectedRoutes)
            {
                var separator = string.IsNullOrEmpty(buildSectorLink) ? "?" : "&";
                buildSectorLink += model.SelectedSectors != null && model.SelectedSectors.Any() ? $"{separator}sectors=" + string.Join("&sectors=", selectedRoute) : "";
            }

            foreach (var selectedLevel in selectedLevels)
            {
                var separator = string.IsNullOrEmpty(buildLevelsLink) ? "?" : "&";
                buildLevelsLink += model.SelectedLevels != null && model.SelectedLevels.Any() ? $"{separator}levels=" + string.Join("&levels=", selectedLevel) : "";
            }

            // Assert
            model.OrderByName.Should().Be($"{buildSectorLink}{buildLevelsLink}&orderby=name");
        }
    }
}
