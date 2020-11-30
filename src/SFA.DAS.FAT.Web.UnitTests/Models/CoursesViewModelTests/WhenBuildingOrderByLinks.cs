using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModelTests
{
    public class WhenBuildingOrderByLinks
    {
        [Test]
        public void Then_Adds_OrderBy_Name_To_Query_String()
        {
            // Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<string>(), null, new List<int>());
            
            // Assert
            model.OrderByName.Should().Be($"?orderby={OrderBy.Name}");
            model.OrderByRelevance.Should().Be($"?orderby={OrderBy.Relevance}");
        }

        [Test, AutoData]
        public void Then_Adds_Keyword_To_Query_String(string keyword)
        {
            // Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<string>(), keyword, new List<int>());
            
            // Assert
            model.OrderByName.Should().Be($"?keyword={model.Keyword}&orderby={OrderBy.Name}");
            model.OrderByRelevance.Should().Be($"?keyword={model.Keyword}&orderby={OrderBy.Relevance}");
        }
        
        
        [Test, AutoData]
        public void Then_Adds_Sectors_To_Query_String(List<string> selectedRoutes)
        {
            // Act
            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, null, new List<int>());
            
            // Assert
            model.OrderByName.Should().Be($"?orderby={OrderBy.Name}&sectors={string.Join("&sectors=", selectedRoutes.Select(HttpUtility.HtmlEncode))}");
            model.OrderByRelevance.Should().Be($"?orderby={OrderBy.Relevance}&sectors={string.Join("&sectors=", selectedRoutes.Select(HttpUtility.HtmlEncode))}");
        }
        [Test, AutoData]
        public void Then_Adds_Levels_To_Query_String(List<int> selectedLevels)
        {
            
            // Act
            var model = CoursesViewModelFactory.BuildModel(new List<string>(), null, selectedLevels);
            
            // Assert
            model.OrderByName.Should().Be($"?orderby={OrderBy.Name}&levels={string.Join("&levels=", selectedLevels)}");
            model.OrderByRelevance.Should().Be($"?orderby={OrderBy.Relevance}&levels={string.Join("&levels=", selectedLevels)}");
        }
        [Test, AutoData]
        public void Then_Adds_Sectors_And_Levels_To_Query_String(List<int> selectedLevels, List<string> selectedRoutes)
        {
            // Act
            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, null, selectedLevels);
            
            // Assert
            model.OrderByName.Should().Be($"?orderby={OrderBy.Name}&sectors={string.Join("&sectors=", selectedRoutes.Select(HttpUtility.HtmlEncode))}&levels={string.Join("&levels=", selectedLevels)}");
            model.OrderByRelevance.Should().Be($"?orderby={OrderBy.Relevance}&sectors={string.Join("&sectors=", selectedRoutes.Select(HttpUtility.HtmlEncode))}&levels={string.Join("&levels=", selectedLevels)}");
        }
    }
}
