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
        [TestCase(10, 5, "", "10 results")]
        [TestCase(10, 5, "test", "5 results")]
        [TestCase(1, 5, "", "1 result")]
        [TestCase(5, 1, "test", "1 result")]
        [TestCase(0, 5, "", "0 results")]
        [TestCase(5, 0, "test", "0 results")]
        public void Then_The_Total_Message_Is_Created_Correctly(int totalCount,int filterTotal, string keyword, string expectedMessage)
        {
            var viewModel = new CoursesViewModel
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
            var viewModel = new CoursesViewModel
            {
                Total = 10,
                TotalFiltered = 5,
                SelectedSectors = new List<Guid>{new Guid()}
            };

            viewModel.TotalMessage.Should().Be("5 results");
        }

        [Test, AutoData]
        public void Then_No_Filter_Items_Builds_Correct_Clear_Links()
        {
            //Arrange
            var viewModel = new CoursesViewModel();
            
            //Assert
            viewModel.ClearKeywordLink.Should().BeEmpty();
            viewModel.ClearSectorLinks.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_The_Clear_Keyword_Link_Is_Generated_If_Filtered_By_Keyword(string keyword)
        {
            //Arrange Act
            var model = new CoursesViewModel
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
            var model = BuildCoursesViewModel(selectedRoutes, keyword);

            //Assert
            Assert.IsNotNull(model.ClearKeywordLink);
            Assert.AreEqual("?sectors=" + string.Join("&sectors=", model.SelectedSectors), model.ClearKeywordLink);
        }
        
        [Test, AutoData]
        public void Then_The_Clear_Filter_Items_Are_Built_From_The_Selected_Items(List<Guid> selectedRoutes, string keyword)
        {
            //Arrange Act
            var model = BuildCoursesViewModel(selectedRoutes, keyword);

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

        private static CoursesViewModel BuildCoursesViewModel(List<Guid> selectedRoutes, string keyword)
        {
            var fixture = new Fixture();
            var sectors = selectedRoutes
                .Select(selectedRoute => new SectorViewModel(
                    new Sector
                    {
                        Id = selectedRoute,
                        Route = fixture.Create<string>()
                    }, null))
                .ToList();

            var model = new CoursesViewModel
            {
                Sectors = sectors,
                Keyword = keyword,
                SelectedSectors = selectedRoutes,
            };
            return model;
        }
    }
}