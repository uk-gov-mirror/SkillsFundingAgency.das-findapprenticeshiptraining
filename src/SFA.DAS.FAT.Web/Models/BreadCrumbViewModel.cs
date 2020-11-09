using System.Collections.Generic;

namespace SFA.DAS.FAT.Web.Models
{
    public class BreadCrumbViewModel
    {
        public BreadCrumbLevel Level { get; set; }
        public string CourseId { get; set; }
        public string CourseDescription { get; set; }
        public Dictionary<string, string> ProvidersFilters { get; set; }
    }

    public enum BreadCrumbLevel
    {
        Home,
        Error,
        CourseList,
        CourseDetails,
        CourseProvidersList,
        CourseProviderDetails
    }
}
