﻿using System;
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
    public class WhenClearingSectorLinks
    {
        [Test, AutoData]
        public void Then_The_Clear_Filter_Items_Are_Built_From_The_Selected_Sectors(List<string> selectedRoutes, string keyword)
        {
            //Arrange Act
            var model = CoursesViewModelFactory.BuildModel(selectedRoutes, keyword, new List<int>());

            //Assert
            var clearLinkCount = selectedRoutes.Count;
            Assert.AreEqual(clearLinkCount, model.ClearSectorLinks.Count);

            foreach (var selectedRoute in selectedRoutes)
            {
                var sector = model.Sectors.SingleOrDefault(c => c.Route.Equals(selectedRoute));

                Assert.IsNotNull(sector);
                Assert.IsTrue(model.ClearSectorLinks.ContainsKey(sector.Route));
                Assert.AreEqual(clearLinkCount - 1, model.ClearSectorLinks.Count(c => c.Value.Contains($"sectors={HttpUtility.HtmlEncode(selectedRoute)}")));
                Assert.AreEqual(clearLinkCount, model.ClearSectorLinks.Count(c => c.Value.Contains($"keyword={keyword}")));
            }
        }
        
        [Test, AutoData]
        public void Then_If_The_Sector_Does_Not_Exist_It_Is_Not_Added(List<string> selectedRoutes )
        {
            //Arrange
            var fixture = new Fixture();
            var sectors = selectedRoutes.Take(1)
                .Select(selectedRoute => new SectorViewModel(
                    new Sector
                    {
                        Id = fixture.Create<Guid>(),
                        Route = selectedRoute
                    }, null))
                .ToList();
            
            //Act
            var model = new CoursesViewModel
            {
                Sectors = sectors,
                Levels = null,
                Keyword = "",
                SelectedSectors = selectedRoutes,
                SelectedLevels = null,
                OrderBy = OrderBy.Name
            };
            
            Assert.AreEqual(1, model.ClearSectorLinks.Count);
        }
    }
}
