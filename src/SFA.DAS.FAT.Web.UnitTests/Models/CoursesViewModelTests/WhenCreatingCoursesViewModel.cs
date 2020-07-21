using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModelTests
{
    public class WhenCreatingCoursesViewModel
    {
        [TestCase(10, 5, "", "10 results")]
        [TestCase(10, 5, "test", "5 results")]
        [TestCase(1, 5, "", "1 result")]
        [TestCase(5, 1, "test", "1 result")]
        [TestCase(0, 5, "", "0 results")]
        [TestCase(5, 0, "test", "0 results")]
        public void Then_The_Total_Message_Is_Created_Correctly(int totalCount, int filterTotal, string keyword, string expectedMessage)
        {
            var viewModel = new Web.Models.CoursesViewModel
            {
                Total = totalCount,
                TotalFiltered = filterTotal,
                Keyword = keyword
            };

            viewModel.TotalMessage.Should().Be(expectedMessage);
        }

        [Test, AutoData]
        public void Then_The_Total_Message_Uses_Filtered_Total_If_There_Are_Selected_Filters()
        {
            var viewModel = new Web.Models.CoursesViewModel
            {
                Total = 10,
                TotalFiltered = 5,
                SelectedSectors = new List<Guid> { new Guid() }
            };

            viewModel.TotalMessage.Should().Be("5 results");
        }

        [Test, AutoData]
        public void Then_The_Total_Message_Uses_Filtered_Total_If_There_Are_Selected_Levels()
        {
            var viewModel = new Web.Models.CoursesViewModel
            {
                Total = 10,
                TotalFiltered = 5,
                SelectedLevels = new List<int> { 1 }
            };

            viewModel.TotalMessage.Should().Be("5 results");
        }

        [Test, AutoData]
        public void Then_The_Total_Message_Uses_Filtered_Total_If_There_Are_Selected_Levels_Sectors_And_Keyword()
        {
            var viewModel = new Web.Models.CoursesViewModel
            {
                Total = 10,
                TotalFiltered = 5,
                SelectedLevels = new List<int> { 1 },
                SelectedSectors = new List<Guid> { new Guid() },
                Keyword = "Test"
            };

            viewModel.TotalMessage.Should().Be("5 results");
        }
        
        [Test, AutoData]
        public void Then_If_There_Are_Filtered_Levels_Sectors_Keywords_The_ShowFilterOptions_Property_Is_True(List<int> selectedLevels, List<Guid> selectedRoutes, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, selectedLevels);
            
            //Assert
            Assert.IsTrue(model.ShowFilterOptions); 
        }
        [Test, AutoData]
        public void Then_If_There_Are_Filtered_Sectors_Keywords_The_ShowFilterOptions_Property_Is_True(List<Guid> selectedRoutes, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, new List<int>());
            
            //Assert
            Assert.IsTrue(model.ShowFilterOptions); 
        }
        
        [Test, AutoData]
        public void Then_If_There_Are_Filtered_Keywords_The_ShowFilterOptions_Property_Is_True(string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), keyword, new List<int>());
            
            //Assert
            Assert.IsTrue(model.ShowFilterOptions); 
        }
        [Test]
        public void Then_If_There_Are_No_Filtered_Options_The_ShowFilterOptions_Property_Is_False()
        {
            //Arrange Act
            var model = CoursesViewModelFactory.BuildModel(new List<Guid>(), "", new List<int>());
            
            //Assert
            Assert.IsFalse(model.ShowFilterOptions); 
        }
    }
}
