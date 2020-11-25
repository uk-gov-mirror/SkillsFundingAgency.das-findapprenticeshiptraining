using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses;

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
    }
}
