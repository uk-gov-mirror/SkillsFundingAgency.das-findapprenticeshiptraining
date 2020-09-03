using System.Linq;

namespace SFA.DAS.FAT.Web.Models
{
    public class LocationViewModel
    {
        public string LocationName { get; set; }
        
        public static implicit operator LocationViewModel(Domain.Locations.Locations.LocationItem source)
        {
            return new LocationViewModel
            {
                LocationName = $"{source.LocationName}, {source.LocalAuthorityName}"
            };
        }
    }
}