using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using NUnit.Framework;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModelTests
{
    class WhenClearingLevelLinks
    {
        [Test, AutoData]
        public void Then_The_Clear_Filter_Items_Are_Built_From_The_Selected_Levels(List<int> selectedLevels, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), keyword, selectedLevels, null);

            //Assert
            var clearLinkCount = selectedLevels.Count;
            Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count);

            foreach (var selectedLevel in selectedLevels)
            {
                var levelViewModel = model.Levels.SingleOrDefault(c => c.Code.Equals(selectedLevel));

                Assert.IsNotNull(levelViewModel);
                Assert.IsTrue(model.ClearLevelLinks.ContainsKey(levelViewModel.Title));
                Assert.AreEqual(clearLinkCount - 1, model.ClearLevelLinks.Count(c => c.Value.Contains($"levels={selectedLevel}")));
                Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains($"keyword={keyword}")));
            }
        }

        [Test, AutoData, Ignore("todo")]//todo: fix this
        public void And_Has_Keyword_And_OrderBy_Then_Builds_QueryString_With_Keyword_And_OrderBy(List<int> selectedLevels, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), keyword, selectedLevels, "name");

            //Assert
            var clearLinkCount = selectedLevels.Count;
            Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count);

            foreach (var selectedLevel in selectedLevels)
            {
                var levelViewModel = model.Levels.SingleOrDefault(c => c.Code.Equals(selectedLevel));

                Assert.IsNotNull(levelViewModel);
                Assert.IsTrue(model.ClearLevelLinks.ContainsKey(levelViewModel.Title));
                Assert.AreEqual(clearLinkCount - 1, model.ClearLevelLinks.Count(c => c.Value.Contains($"levels={selectedLevel}")));
                Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains($"keyword={keyword}")));
                Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains($"orderby=name")));
            }
        }

        [Test, AutoData]
        public void Then_The_Clear_Filter_Items_Are_Built_From_The_Selected_Levels_And_Sectors(List<int> selectedLevels, List<Guid> selectedRoutes, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, selectedLevels, null);

            //Assert
            var clearSectorsLinkCount = selectedRoutes.Count;
            Assert.AreEqual(clearSectorsLinkCount, model.ClearSectorLinks.Count);
            var clearLevelsLinkCount = selectedLevels.Count;
            Assert.AreEqual(clearLevelsLinkCount, model.ClearLevelLinks.Count);

            foreach (var selectedLevel in selectedLevels)
            {
                var levelViewModel = model.Levels.SingleOrDefault(c => c.Code.Equals(selectedLevel));

                Assert.IsNotNull(levelViewModel);
                Assert.IsTrue(model.ClearLevelLinks.ContainsKey(levelViewModel.Title));
                Assert.AreEqual(clearLevelsLinkCount - 1, model.ClearLevelLinks.Count(c => c.Value.Contains($"levels={selectedLevel}")));
                Assert.AreEqual(clearLevelsLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains($"?keyword={keyword}")));
                Assert.AreEqual(clearLevelsLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains("&sectors=" + string.Join("&sectors=", model.SelectedSectors))));
            }
            foreach (var selectedRoute in selectedRoutes)
            {
                var sector = model.Sectors.SingleOrDefault(c => c.Id.Equals(selectedRoute));

                Assert.IsNotNull(sector);
                Assert.IsTrue(model.ClearSectorLinks.ContainsKey(sector.Route));
                Assert.AreEqual(clearSectorsLinkCount - 1, model.ClearSectorLinks.Count(c => c.Value.Contains($"sectors={selectedRoute}")));
                Assert.AreEqual(clearSectorsLinkCount, model.ClearSectorLinks.Count(c => c.Value.Contains($"?keyword={keyword}")));
                Assert.AreEqual(clearSectorsLinkCount, model.ClearSectorLinks.Count(c => c.Value.Contains("&levels=" + string.Join("&levels=", model.SelectedLevels))));
            }
        }
    }
}
