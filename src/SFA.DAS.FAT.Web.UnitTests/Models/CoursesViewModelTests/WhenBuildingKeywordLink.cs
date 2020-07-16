using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModel
{
    class WhenBuildingKeywordLink
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
            Assert.AreEqual("", model.ClearKeywordLink);
        }

        [Test, AutoData]
        public void Then_The_Clear_Keyword_Link_Is_Generated_If_Filtered_By_Keyword_With_Sectors(List<Guid> selectedRoutes, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelTests.CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, new List<int>(), null);

            //Assert
            Assert.IsNotNull(model.ClearKeywordLink);
            Assert.AreEqual("?sectors=" + string.Join("&sectors=", model.SelectedSectors), model.ClearKeywordLink);
        }

        [Test, AutoData]
        public void Then_The_Clear_Keyword_Link_Is_Generated_If_Filtered_By_Keyword_With_Levels(List<int> selectedLevels, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelTests.CoursesViewModelFactory.BuildModel(new List<Guid>(), keyword, selectedLevels, null);

            //Assert
            Assert.IsNotNull(model.ClearKeywordLink);
            Assert.AreEqual("?levels=" + string.Join("&levels=", model.SelectedLevels), model.ClearKeywordLink);
        }
        [Test, AutoData]
        public void Then_The_Clear_Keyword_Link_Is_Generated_If_Filtered_By_Keyword_With_Sectors_And_Levels(List<Guid> selectedRoutes, List<int> selectedLevels, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelTests.CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, selectedLevels, null);

            //Assert
            Assert.IsNotNull(model.ClearKeywordLink);
            Assert.AreEqual("?sectors=" + string.Join("&sectors=", model.SelectedSectors) + "&levels=" + string.Join("&levels=", model.SelectedLevels), model.ClearKeywordLink);
        }
    }
}
