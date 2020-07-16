using AutoFixture;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CoursesViewModelTests
{
    public static class CoursesViewModelFactory
    {
        public static CoursesViewModel BuildModel(List<Guid> selectedRoutes, string keyword, List<int> selectedLevels, string order)
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
            var levels = selectedLevels
                .Select(selectedLevel => new LevelViewModel(
                    new Level
                    {
                        Code = selectedLevel,
                        Name = fixture.Create<string>()
                    }, null))
                .ToList();

            var model = new CoursesViewModel
            {
                Sectors = sectors,
                Levels = levels,
                Keyword = keyword,
                SelectedSectors = selectedRoutes,
                SelectedLevels = selectedLevels,
                OrderBy = order
            };
            return model;
        }
    }
}
