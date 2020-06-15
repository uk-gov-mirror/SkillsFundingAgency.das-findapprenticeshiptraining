using System.Threading.Tasks;
using SFA.DAS.FAT.Domain.Validation;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourse
{
    public class GetCourseRequestValidator : IValidator<GetCourseRequest>
    {
        public Task<ValidationResult> ValidateAsync(GetCourseRequest item)
        {
            throw new System.NotImplementedException();
        }
    }
}