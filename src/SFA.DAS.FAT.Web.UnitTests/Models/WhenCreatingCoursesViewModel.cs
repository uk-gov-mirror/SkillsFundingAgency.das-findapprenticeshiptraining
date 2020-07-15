using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models
{
    public class WhenCreatingCoursesViewModel
    {
        [Test, AutoData]
        public void Then_No_Filter_Items_Builds_Correct_Clear_Links()
        {
            //Arrange
            var viewModel = new Web.Models.CoursesViewModel();
            
            //Assert
            viewModel.ClearKeywordLink.Should().BeEmpty();
            viewModel.ClearSectorLinks.Should().BeEmpty();
        }
        
        [Test, AutoData]
        public void Then_The_Clear_Filter_Items_Are_Built_From_The_Selected_Levels_And_Sectors(List<int> selectedLevels, List<Guid> selectedRoutes, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelTests.CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, selectedLevels, null);

            //Assert
            var clearSectorsLinkCount = selectedRoutes.Count;
            Assert.AreEqual(clearSectorsLinkCount,model.ClearSectorLinks.Count);
            var clearLevelsLinkCount = selectedLevels.Count;
            Assert.AreEqual(clearLevelsLinkCount,model.ClearLevelLinks.Count);
            
            foreach (var selectedLevel in selectedLevels)
            {
                var levelViewModel = model.Levels.SingleOrDefault(c => c.Code.Equals(selectedLevel));
                
                Assert.IsNotNull(levelViewModel);
                Assert.IsTrue(model.ClearLevelLinks.ContainsKey(levelViewModel.Title));
                Assert.AreEqual(clearLevelsLinkCount-1, model.ClearLevelLinks.Count(c=>c.Value.Contains($"levels={selectedLevel}")));    
                Assert.AreEqual(clearLevelsLinkCount, model.ClearLevelLinks.Count(c=>c.Value.Contains($"?keyword={keyword}")));    
                Assert.AreEqual(clearLevelsLinkCount, model.ClearLevelLinks.Count(c=>c.Value.Contains("&sectors=" + string.Join("&sectors=", model.SelectedSectors))));    
            }
            foreach (var selectedRoute in selectedRoutes)
            {
                var sector = model.Sectors.SingleOrDefault(c => c.Id.Equals(selectedRoute));
                
                Assert.IsNotNull(sector);
                Assert.IsTrue(model.ClearSectorLinks.ContainsKey(sector.Route));
                Assert.AreEqual(clearSectorsLinkCount-1, model.ClearSectorLinks.Count(c=>c.Value.Contains($"sectors={selectedRoute}")));    
                Assert.AreEqual(clearSectorsLinkCount, model.ClearSectorLinks.Count(c=>c.Value.Contains($"?keyword={keyword}")));    
                Assert.AreEqual(clearSectorsLinkCount, model.ClearSectorLinks.Count(c=>c.Value.Contains("&levels=" + string.Join("&levels=", model.SelectedLevels))));    
            }
        }
        
    }
}