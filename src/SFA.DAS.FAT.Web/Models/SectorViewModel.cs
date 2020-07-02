using System;
using System.Collections.Generic;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class SectorViewModel
    {
        public SectorViewModel ()
        {
            
        }

        public SectorViewModel (Sector sector, ICollection<Guid> selectedIds)
        {
            Selected =selectedIds?.Contains(sector.Id) ?? false;
            Id = sector.Id;
            Route = sector.Route;

        }
        public bool Selected { get;  }
        public string Route { get ;  }
        public Guid Id { get ;  }
    }
}