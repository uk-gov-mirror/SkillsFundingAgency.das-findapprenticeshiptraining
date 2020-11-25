using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoFixture;
using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModelTests
{
    public class WhenClearingLevelLinks
    {
        [Test, AutoData]
        public void Then_The_Clear_Filter_Items_Are_Built_From_The_Selected_Levels(Generator<int> selectedLevelsGenerator)
        {
            //Arrange Act
            var selectedLevels = selectedLevelsGenerator.Distinct().Take(3).ToList();

            var model = CoursesViewModelFactory.BuildModel(new List<string>(), "", selectedLevels);

            //Assert
            var clearLinkCount = selectedLevels.Count;
            Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count);

            foreach (var selectedLevel in selectedLevels)
            {
                var levelViewModel = model.Levels.SingleOrDefault(c => c.Code.Equals(selectedLevel));

                Assert.IsNotNull(levelViewModel);
                Assert.IsTrue(model.ClearLevelLinks.ContainsKey(levelViewModel.Title));

                AssertClearLevelLink(model, clearLinkCount);

                Assert.AreEqual(0, model.ClearLevelLinks.Count(c => c.Value.Contains("orderby=")));
            }
        }

        [Test, AutoData]
        public void Then_Has_Keyword_Then_Builds_QueryString_With_Keyword(Generator<int> selectedLevelsGenerator, string keyword)
        {
            //Arrange Act
            var selectedLevels = selectedLevelsGenerator.Distinct().Take(3).ToList();

            var model = CoursesViewModelFactory.BuildModel(new List<string>(), keyword, selectedLevels);

            //Assert
            var clearLinkCount = selectedLevels.Count;
            Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count);

            foreach (var selectedLevel in selectedLevels)
            {
                var levelViewModel = model.Levels.SingleOrDefault(c => c.Code.Equals(selectedLevel));

                Assert.IsNotNull(levelViewModel);
                Assert.IsTrue(model.ClearLevelLinks.ContainsKey(levelViewModel.Title));

                AssertClearLevelLink(model, clearLinkCount);

                Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains($"keyword={keyword}")));
                Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains($"orderby=")));
            }
        }
        
        [Test, AutoData]
        public void Then_Has_Keyword_And_OrderBy_Then_Builds_QueryString_With_Keyword_And_OrderBy(Generator<int> selectedLevelsGenerator, string keyword)
        {
            //Arrange Act
            var selectedLevels = selectedLevelsGenerator.Distinct().Take(3).ToList();

            var model = CoursesViewModelFactory.BuildModel(new List<string>(), keyword, selectedLevels);

            //Assert
            var clearLinkCount = selectedLevels.Count;
            Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count);

            foreach (var selectedLevel in selectedLevels)
            {
                var levelViewModel = model.Levels.SingleOrDefault(c => c.Code.Equals(selectedLevel));

                Assert.IsNotNull(levelViewModel);
                Assert.IsTrue(model.ClearLevelLinks.ContainsKey(levelViewModel.Title));

                AssertClearLevelLink(model, clearLinkCount);

                Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains($"keyword={keyword}")));
                Assert.AreEqual(clearLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains($"orderby={OrderBy.Name}")));
            }
        }

        [Test, AutoData]
        public void Then_The_Clear_Filter_Items_Are_Built_From_The_Selected_Levels_And_Sectors(Generator<int> selectedLevelsGenerator, List<string> selectedRoutes, string keyword)
        {
            //Arrange Act
            var selectedLevels = selectedLevelsGenerator.Distinct().Take(3).ToList();

            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, selectedLevels);

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

                AssertClearLevelLink(model, clearLevelsLinkCount);

                Assert.AreEqual(clearLevelsLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains($"?keyword={keyword}")));
                Assert.AreEqual(clearLevelsLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains("&sectors=" + string.Join("&sectors=", selectedRoutes.Select(HttpUtility.HtmlEncode)))));
                Assert.AreEqual(clearLevelsLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains($"orderby=")));
            }
            foreach (var selectedRoute in selectedRoutes)
            {
                var sector = model.Sectors.SingleOrDefault(c => c.Route.Equals(selectedRoute));

                Assert.IsNotNull(sector);
                Assert.IsTrue(model.ClearSectorLinks.ContainsKey(sector.Route));
                Assert.AreEqual(clearSectorsLinkCount - 1, model.ClearSectorLinks.Count(c => c.Value.Contains($"sectors={HttpUtility.HtmlEncode(selectedRoute)}")));
                Assert.AreEqual(clearSectorsLinkCount, model.ClearSectorLinks.Count(c => c.Value.Contains($"?keyword={keyword}")));
                Assert.AreEqual(clearSectorsLinkCount, model.ClearSectorLinks.Count(c => c.Value.Contains("&levels=" + string.Join("&levels=", model.SelectedLevels))));
                Assert.AreEqual(clearSectorsLinkCount, model.ClearSectorLinks.Count(c => c.Value.Contains($"orderby=")));
            }
        }
        [Test, AutoData]
        public void Then_The_Clear_Filter_Items_Are_Built_From_The_Selected_Levels_And_Sectors_And_OrderBy(Generator<int> selectedLevelsGenerator, List<string> selectedRoutes, string keyword)
        {
            //Arrange Act
            var selectedLevels = selectedLevelsGenerator.Distinct().Take(3).ToList();

            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, selectedLevels);

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

                AssertClearLevelLink(model, clearLevelsLinkCount);

                Assert.AreEqual(clearLevelsLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains($"?keyword={keyword}")));
                Assert.AreEqual(clearLevelsLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains($"&orderby={OrderBy.Name}")));
                Assert.AreEqual(clearLevelsLinkCount, model.ClearLevelLinks.Count(c => c.Value.Contains("&sectors=" + string.Join("&sectors=", model.SelectedSectors.Select(HttpUtility.HtmlEncode)))));
            }
            foreach (var selectedRoute in selectedRoutes)
            {
                var sector = model.Sectors.SingleOrDefault(c => c.Route.Equals(selectedRoute));

                Assert.IsNotNull(sector);
                Assert.IsTrue(model.ClearSectorLinks.ContainsKey(sector.Route));
                Assert.AreEqual(clearSectorsLinkCount - 1, model.ClearSectorLinks.Count(c => c.Value.Contains($"sectors={HttpUtility.HtmlEncode(selectedRoute)}")));
                Assert.AreEqual(clearSectorsLinkCount, model.ClearSectorLinks.Count(c => c.Value.Contains($"?keyword={keyword}")));
                Assert.AreEqual(clearSectorsLinkCount, model.ClearSectorLinks.Count(c => c.Value.Contains($"&orderby={OrderBy.Name}")));
                Assert.AreEqual(clearSectorsLinkCount, model.ClearSectorLinks.Count(c => c.Value.Contains("&levels=" + string.Join("&levels=", model.SelectedLevels))));
            }
        }

        [Test, AutoData]
        public void Then_If_The_Level_Does_Not_Exist_It_Is_Not_Added(List<int> selectedLevels )
        {
            //Arrange
            var fixture = new Fixture();
            var levels = selectedLevels.Take(1)
                .Select(selectedLevel => new LevelViewModel(
                    new Level
                    {
                        Code = selectedLevel,
                        Name = fixture.Create<string>()
                    }, null))
                .ToList();
            
            //Act
            var model = new CoursesViewModel
            {
                Sectors = null,
                Levels = levels,
                Keyword = "",
                SelectedSectors = null,
                SelectedLevels = selectedLevels,
                OrderBy = OrderBy.Name
            };
            
            Assert.AreEqual(1, model.ClearLevelLinks.Count);
        }

        private static void AssertClearLevelLink(CoursesViewModel model, int clearLinkCount)
        {
            foreach (var modelClearLevelLink in model.ClearLevelLinks)
            {
                var queryParams = HttpUtility.ParseQueryString(
                        new Uri("https://test.com/" + modelClearLevelLink.Value).Query)["Levels"].Split(",");
                Assert.AreEqual(clearLinkCount - 1, queryParams.Length);
            }
        }
    }
}
