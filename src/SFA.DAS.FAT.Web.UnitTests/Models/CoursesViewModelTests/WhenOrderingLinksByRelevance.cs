using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModelTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModel
{
    class WhenOrderingLinksByRelevance
    {
        [Test]
        public void Then_Adds_OrderBy_Relevance_To_Query_String()
        {
            // Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), null, new List<int>(), "Relevance");
            // Assert
            model.OrderByRelevance.Should().Be("?orderBy=Relevance");
        }

        [Test, AutoData]
        public void And_Then_Adds_Keyword_To_Query_String(string keyword)
        {
            // Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), keyword, new List<int>(), "Relevance");
            // Assert
            model.OrderByRelevance.Should​().Be($"?orderBy=Relevance&keyword={model.Keyword}");
        }

        [Test, AutoData]
        public void And_Then_Adds_Sectors_To_Query_String(List<Guid> selectedRoutes)
        {
            // Arrange 
            var buildSectorLink = "";

            // Act
            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, null, new List<int>(), "Relevance");
            foreach (var selectedRoute in selectedRoutes)
            {
                buildSectorLink += model.SelectedSectors != null && model.SelectedSectors.Any() ? $"&sectors=" + string.Join("&sectors=", selectedRoute) : "";
            }

            // Assert​
            model.OrderByRelevance.Should().Be($"?orderBy=Relevance{buildSectorLink}");
        }
        [Test, AutoData]
        public void And_Then_Adds_Levels_To_Query_String(List<int> selectedLevels)
        {
            // Arrange 
            var buildLevelsLink = "";

            // Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), null, selectedLevels, "Relevance");

            foreach (var selectedLevel in selectedLevels)
            {
                buildLevelsLink += model.SelectedLevels != null && model.SelectedLevels.Any() ? $"&levels=" + string.Join("levels=", selectedLevel) : "";
            }

            // Assert​
            model.OrderByRelevance.Should().Be($"?orderBy=Relevance{buildLevelsLink}");
        }
        [Test, AutoData]
        public void And_Then_Adds_Sectors_And_Levels_To_Query_String(List<int> selectedLevels, List<Guid> selectedRoutes)
        {
            // Arrange 
            var buildLevelsLink = "";
            var buildSectorLink = "";

            // Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), null, new List<int>(), "Relevance");
            foreach (var selectedLevel in selectedLevels)
            {
                buildLevelsLink += model.SelectedLevels != null && model.SelectedLevels.Any() ? $"&levels=" + string.Join("levels=", selectedLevel) : "";
            }

            foreach (var selectedRoute in selectedRoutes)
            {
                buildSectorLink += model.SelectedSectors != null && model.SelectedSectors.Any() ? $"&sectors=" + string.Join("&sectors=", selectedRoute) : "";
            }

            // Assert
            model.OrderByRelevance.Should().Be($"?orderBy=Relevance{buildLevelsLink}{buildSectorLink}");
        }
    }
}
