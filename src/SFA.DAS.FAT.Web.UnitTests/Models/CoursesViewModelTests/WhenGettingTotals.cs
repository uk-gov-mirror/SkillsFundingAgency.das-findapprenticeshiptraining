using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.FAT.Web.Models;
using FluentAssertions;
using AutoFixture.NUnit3;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModel
{
    public class WhenGettingTotals
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
    }
}
