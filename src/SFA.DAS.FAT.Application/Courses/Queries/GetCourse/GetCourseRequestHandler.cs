using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Domain.Validation;

namespace SFA.DAS.FAT.Application.Courses.Queries.GetCourse
{
    public class GetCourseRequestHandler : IRequestHandler<GetCourseRequest, GetCourseResult>
    {
        private readonly ICourseService _courseService;
        private readonly IValidator<GetCourseRequest> _validator;

        public GetCourseRequestHandler(ICourseService courseService, IValidator<GetCourseRequest> validator)
        {
            _courseService = courseService;
            _validator = validator;
        }
        public async Task<GetCourseResult> Handle(GetCourseRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }

            var response = await _courseService.GetCourse(request.CourseId);
            
            return new GetCourseResult{Course = response};
        }
    }
}