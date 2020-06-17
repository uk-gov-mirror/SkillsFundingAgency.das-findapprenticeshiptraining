namespace SFA.DAS.FAT.Domain.Courses
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string OverviewOfRole { get; set; }
        public string Route { get; set; }
        public string IntegratedDegree { get; set; }
        public string CoreSkills { get; set; }
        public string TypicalJobTitles { get; set; }
        public string ExternalCourseUrl { get; set; }
        public int TypicalDuration { get; set; }
        public decimal MaximumFunding { get; set; }
    }
}