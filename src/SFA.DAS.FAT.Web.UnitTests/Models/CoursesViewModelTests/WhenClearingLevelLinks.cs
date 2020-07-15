using AutoFixture.NUnit3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModel
{
    class WhenClearingLevelLinks
    {
        [Test, AutoData]
        public void Then_The_Clear_Filter_Items_Are_Built_From_The_Selected_Levels(List<int> selectedLevels, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelTests.CoursesViewModelFactory.BuildModel(new List<Guid>(), keyword, selectedLevels);

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
    }
}
