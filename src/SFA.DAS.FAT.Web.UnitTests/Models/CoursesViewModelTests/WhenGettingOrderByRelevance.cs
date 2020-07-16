using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModelTests
{
    class WhenGettingOrderByRelevance
    {
        [Test]
        public void Then_Adds_OrderBy_Relevance_To_Query_String()
        {
            // Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), null, new List<int>(), OrderBy.Relevance);
            // Assert
            model.OrderByRelevance.Should().Be($"?orderby={model.OrderBy}");
        }

        [Test, AutoData]
        public void Then_Adds_Keyword_To_Query_String(string keyword)
        {
            // Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), keyword, new List<int>(), OrderBy.Relevance);
            // Assert
            model.OrderByRelevance.Should​().Be($"?keyword={model.Keyword}&orderby={model.OrderBy}");
        }

        [Test, AutoData]
        public void And_Then_Adds_Sectors_To_Query_String(List<Guid> selectedRoutes)
        {
            // Arrange 
            var buildSectorLink = "";

            // Act
            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, null, new List<int>(), OrderBy.Relevance);
            foreach (var selectedRoute in selectedRoutes)
            {
                var separator = string.IsNullOrEmpty(buildSectorLink) ? "?" : "&";
                buildSectorLink += model.SelectedSectors != null && model.SelectedSectors.Any() ? $"{separator}sectors=" + string.Join("&sectors=", selectedRoute) : "";
            }

            // Assert​
            model.OrderByRelevance.Should().Be($"{buildSectorLink}&orderby={model.OrderBy}");
        }

        [Test, AutoData]
        public void And_Then_Adds_Levels_To_Query_String(List<int> selectedLevels)
        {
            // Arrange 
            var buildLevelsLink = "";

            // Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), null, selectedLevels, OrderBy.Relevance);

            foreach (var selectedLevel in selectedLevels)
            {
                var separator = string.IsNullOrEmpty(buildLevelsLink) ? "?" : "&";
                buildLevelsLink += model.SelectedLevels != null && model.SelectedLevels.Any() ? $"{separator}levels=" + string.Join("levels=", selectedLevel) : "";
            }

            // Assert​
            model.OrderByRelevance.Should().Be($"{buildLevelsLink}&orderby={model.OrderBy}");
        }

        [Test, AutoData]
        public void Then_Adds_Sectors_And_Levels_To_Query_String(List<int> selectedLevels, List<Guid> selectedRoutes)
        {
            // Arrange 
            var buildLevelsLink = "";
            var buildSectorLink = "";

            // Act
            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, null, selectedLevels, OrderBy.Relevance);
            foreach (var selectedLevel in selectedLevels)
            {
                var separator = string.IsNullOrEmpty(buildLevelsLink) ? "?" : "&";
                buildLevelsLink += model.SelectedLevels != null && model.SelectedLevels.Any() ? $"{separator}levels=" + string.Join("levels=", selectedLevel) : "";
            }

            foreach (var selectedRoute in selectedRoutes)
            {
                var separator = string.IsNullOrEmpty(buildSectorLink) ? "?" : "&";
                buildSectorLink += model.SelectedSectors != null && model.SelectedSectors.Any() ? $"{separator}sectors=" + string.Join("&sectors=", selectedRoute) : "";
            }

            // Assert
            model.OrderByRelevance.Should().Be($"{buildSectorLink}{buildLevelsLink}&orderby={model.OrderBy}");
        }
    }
}
