using System;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class SectorViewModel
    {
        public string Route { get ; set ; }

        public Guid Id { get ; set ; }

        public static implicit operator SectorViewModel(Sector sector)
        {
            return new SectorViewModel
            {
                Id = sector.Id,
                Route = sector.Route
            };
        }
    }
}