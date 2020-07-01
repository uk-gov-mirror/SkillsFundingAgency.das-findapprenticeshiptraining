using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class CoursesViewModel
    {
        public List<CourseViewModel> Courses { get; set; }
        public string Keyword { get; set; }
        private string GetTotalMessage()
        {
            var totalToUse = string.IsNullOrEmpty(Keyword) ? Total : TotalFiltered;

            return $"{totalToUse} result" + (totalToUse!=1 ? "s": "");
        }
        public int Total { get ; set ; }
        public int TotalFiltered { get ; set ; }
        public string TotalMessage => GetTotalMessage();
    }
    
}