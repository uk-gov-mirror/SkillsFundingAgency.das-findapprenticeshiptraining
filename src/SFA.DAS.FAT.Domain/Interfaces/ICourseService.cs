using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Domain.Interfaces
{
    public interface ICourseService
    {
        Task<Course> GetCourse(int courseId);
        Task<IEnumerable<Course>> GetCourses();
    }
}