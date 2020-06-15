using System.Threading.Tasks;
using SFA.DAS.FAT.Domain.Validation;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourse
{
    public class GetCourseRequestValidator : IValidator<GetCourseRequest>
    {
        public Task<ValidationResult> ValidateAsync(GetCourseRequest item)
        {
            var validationResult = new ValidationResult();

            if (item.CourseId < 1)
            {
                validationResult.AddError(nameof(item.CourseId));
            }

            return Task.FromResult(validationResult);
        }
    }
}