using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Web.Extensions;

namespace SFA.DAS.FAT.Web.Models
{
    public class CourseViewModel
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
        public DateTime? LastDateStarts { get; set; }
        public bool AfterLastStartDate { get; set; } = false;

        public static implicit operator CourseViewModel(Course course)
        {
            return new CourseViewModel
            {
                Id = course.Id,
                Sector = course.Route,
                CoreSkills = string.IsNullOrEmpty(course.CoreSkillsCount) ? new List<string>() : course.CoreSkillsCount.Split("|").ToList(),
                Title = course.Title,
                TitleAndLevel = $"{course.Title} (level {course.Level})",
                Level = course.Level,
                IntegratedDegree = course.IntegratedDegree,
                ExternalCourseUrl = course.StandardPageUrl,
                OverviewOfRole = course.OverviewOfRole,
                TypicalJobTitles = string.IsNullOrEmpty(course.TypicalJobTitles) ? new List<string>() : course.TypicalJobTitles.Split("|").ToList(),
                TypicalDuration = course.TypicalDuration,
                MaximumFunding = course.MaxFunding.ToGdsCostFormat(),
                LastDateStarts = course.StandardDates?.LastDateStarts,
                AfterLastStartDate = DateTime.UtcNow > course.StandardDates?.LastDateStarts,
            };
        }
    }
}
