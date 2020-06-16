using System.Linq;
using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models
{
    public class WhenCreatingCourseDetailViewModel
    {
        [Test, AutoData]
        public void Then_The_Model_Is_Mapped_Correctly(Course course)
        {
            //Act
            var actual = (CourseDetailViewModel) course;
            
            //Assert
            Assert.AreEqual(course.Id, actual.Id);
            Assert.AreEqual($"{course.Title} ({course.Level})", actual.Title);
            Assert.AreEqual(course.Level, actual.Level);
            Assert.AreEqual(course.Route, actual.Sector);
            Assert.AreEqual(course.IntegratedDegree, actual.IntegratedDegree);
            Assert.AreEqual(course.OverviewOfRole, actual.OverviewOfRole);
            Assert.AreEqual(course.CoreSkills.Split("|").ToList(), actual.CoreSkills);
            Assert.AreEqual(course.TypicalJobTitles.Split("|").ToList(), actual.TypicalJobTitles);
            Assert.AreEqual(course.ExternalCourseUrl, actual.ExternalCourseUrl);
            Assert.AreEqual(course.TypicalDuration, actual.TypicalDuration);
        }
    }
}