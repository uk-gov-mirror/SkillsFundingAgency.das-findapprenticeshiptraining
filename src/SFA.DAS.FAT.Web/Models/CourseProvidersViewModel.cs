using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class CourseProvidersViewModel
    {
        public IEnumerable<ProviderViewModel> Providers { get; set; }
        public CourseViewModel Course { get; set; }
        public int Total { get; set; }
        public string TotalMessage => Total == 1 ? $"{Total} result" : $"{Total} results";
        public string Location { get; set; }
        public ProviderSortBy SortOrder { get; set; }
        public bool HasLocations => Providers.Sum(c => c.DeliveryModes.ToList().Count) > 0;
        public string BuildSortLink()
        {
            var newOrder = SortOrder == ProviderSortBy.Distance ? 
                ProviderSortBy.Name : ProviderSortBy.Distance;

            return $"?locations={Location}&sortorder={newOrder}";
        }
    }
}
