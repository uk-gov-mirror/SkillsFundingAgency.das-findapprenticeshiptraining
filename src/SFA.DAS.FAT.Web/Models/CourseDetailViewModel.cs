using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class CourseDetailViewModel
    {
        public int Id { get ; private set ; }
        public string Title { get ; set ; }
        public string TitleAndLevel { get; private set; }
        public string Sector { get; private set; }
        public string IntegratedDegree { get ; private set ; }
        public string OverviewOfRole { get ; private set ; }
        public List<string> CoreSkills { get ; private set ; }
        public List<string> TypicalJobTitles { get ; private set ; }
        public string ExternalCourseUrl { get ; private set ; }
        public int TypicalDuration { get ; private set ; }
        public int Level { get ; private set ; }
        public string MaximumFunding { get ; set ; }

        public static implicit operator CourseDetailViewModel(Course course)
        {
            return new CourseDetailViewModel
            {
                Id = course.Id,
                Sector = course.Route,
                CoreSkills = course.CoreSkills.Split("|").ToList(),
                Title = course.Title,
                TitleAndLevel = $"{course.Title} ({course.Level})",
                Level = course.Level,
                IntegratedDegree = course.IntegratedDegree,
                ExternalCourseUrl = course.ExternalCourseUrl,
                OverviewOfRole = course.OverviewOfRole,
                TypicalJobTitles =course.TypicalJobTitles.Split("|").ToList(),
                TypicalDuration = course.TypicalDuration,
                MaximumFunding = course.MaximumFunding.ToString()
            };
        }
    }
}