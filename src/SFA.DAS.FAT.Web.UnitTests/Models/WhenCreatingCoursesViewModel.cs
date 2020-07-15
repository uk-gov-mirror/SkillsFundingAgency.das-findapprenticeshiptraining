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
        public void Then_The_Clear_Keyword_Link_Is_Generated_If_Filtered_By_Keyword(string keyword)
        {
            //Arrange Act
            var model = new Web.Models.CoursesViewModel
            {
                Keyword = keyword,
            };
            
            //Assert
            Assert.IsNotNull(model.ClearKeywordLink);
            Assert.AreEqual("",model.ClearKeywordLink);
        }
        
        [Test, AutoData]
        public void Then_The_Clear_Keyword_Link_Is_Generated_If_Filtered_By_Keyword_With_Sectors(List<Guid> selectedRoutes, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelTests.CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, new List<int>());

            //Assert
            Assert.IsNotNull(model.ClearKeywordLink);
            Assert.AreEqual("?sectors=" + string.Join("&sectors=", model.SelectedSectors), model.ClearKeywordLink);
        }

        [Test, AutoData]
        public void Then_The_Clear_Keyword_Link_Is_Generated_If_Filtered_By_Keyword_With_Levels(List<int> selectedLevels, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelTests.CoursesViewModelFactory.BuildModel(new List<Guid>(), keyword, selectedLevels);

            //Assert
            Assert.IsNotNull(model.ClearKeywordLink);
            Assert.AreEqual("?levels=" + string.Join("&levels=", model.SelectedLevels), model.ClearKeywordLink);
        }
        [Test, AutoData]
        public void Then_The_Clear_Keyword_Link_Is_Generated_If_Filtered_By_Keyword_With_Sectors_And_Levels(List<Guid> selectedRoutes,List<int> selectedLevels, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelTests.CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, selectedLevels);

            //Assert
            Assert.IsNotNull(model.ClearKeywordLink);
            Assert.AreEqual("?sectors=" + string.Join("&sectors=", model.SelectedSectors) + "&levels=" + string.Join("&levels=", model.SelectedLevels), model.ClearKeywordLink);
        }
        
        [Test, AutoData]
        public void Then_The_Clear_Filter_Items_Are_Built_From_The_Selected_Sectors(List<Guid> selectedRoutes, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelTests.CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, new List<int>());

            //Assert
            var clearLinkCount = selectedRoutes.Count;
            Assert.AreEqual(clearLinkCount,model.ClearSectorLinks.Count);
            
            foreach (var selectedRoute in selectedRoutes)
            {
                var sector = model.Sectors.SingleOrDefault(c => c.Id.Equals(selectedRoute));
                
                Assert.IsNotNull(sector);
                Assert.IsTrue(model.ClearSectorLinks.ContainsKey(sector.Route));
                Assert.AreEqual(clearLinkCount-1, model.ClearSectorLinks.Count(c=>c.Value.Contains($"sectors={selectedRoute}")));    
                Assert.AreEqual(clearLinkCount, model.ClearSectorLinks.Count(c=>c.Value.Contains($"keyword={keyword}")));    
            }

            
        }

        [Test, AutoData]
        public void Then_The_Clear_Filter_Items_Are_Built_From_The_Selected_Levels(List<int> selectedLevels, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelTests.CoursesViewModelFactory.BuildModel(new List<Guid>(), keyword, selectedLevels);

            //Assert
            var clearLinkCount = selectedLevels.Count;
            Assert.AreEqual(clearLinkCount,model.ClearLevelLinks.Count);
            
            foreach (var selectedLevel in selectedLevels)
            {
                var levelViewModel = model.Levels.SingleOrDefault(c => c.Code.Equals(selectedLevel));
                
                Assert.IsNotNull(levelViewModel);
                Assert.IsTrue(model.ClearLevelLinks.ContainsKey(levelViewModel.Title));
                Assert.AreEqual(clearLinkCount-1, model.ClearLevelLinks.Count(c=>c.Value.Contains($"levels={selectedLevel}")));    
                Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count(c=>c.Value.Contains($"keyword={keyword}")));    
            }
        }
        
        [Test, AutoData]
        public void Then_The_Clear_Filter_Items_Are_Built_From_The_Selected_Levels_And_Sectors(List<int> selectedLevels, List<Guid> selectedRoutes, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelTests.CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, selectedLevels);

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