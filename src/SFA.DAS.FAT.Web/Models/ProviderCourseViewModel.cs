using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class ProviderCourseViewModel
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string TitleAndLevel { get; set; }

        public static implicit operator ProviderCourseViewModel(AdditionalCourse course)
        {
            return new ProviderCourseViewModel
            {
                Id = course.Id,
                Level = course.Level,
                Title = course.Title, 
                TitleAndLevel = $"{course.Title} (level {course.Level})",
            };
        }
    }
}
